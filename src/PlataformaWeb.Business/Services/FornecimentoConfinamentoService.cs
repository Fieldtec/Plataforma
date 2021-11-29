using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class FornecimentoConfinamentoService : Service, IFornecimentoConfinamentoService
    {
        private readonly IFornecimentoConfinamentoRepositorio _repositorio;
        private readonly IPlanejamentoNutricionalRepositorio _planejamentoNutricionalRepositorio;
        private readonly ILoteEntradaRepositorio _loteEntradaRepositorio;
        private readonly IRacaoRepositorio _racaoRepositorio;
        private readonly IPastoCurralRepositorio _pastoCurralRepositorio;
        private readonly IFuncoesRepositorio _funcoesRepositorio;

        public FornecimentoConfinamentoService(INotificador notificador,
            IUser appUser,
            IFornecimentoConfinamentoRepositorio repositorio,
            IPlanejamentoNutricionalRepositorio planejamentoNutricionalRepositorio,
            ILoteEntradaRepositorio loteEntradaRepositorio,
            IRacaoRepositorio racaoRepositorio,
            IPastoCurralRepositorio pastoCurralRepositorio, 
            IFuncoesRepositorio funcoesRepositorio) : base(notificador, appUser)
        {
            _repositorio = repositorio;
            _planejamentoNutricionalRepositorio = planejamentoNutricionalRepositorio;
            _loteEntradaRepositorio = loteEntradaRepositorio;
            _racaoRepositorio = racaoRepositorio;
            _pastoCurralRepositorio = pastoCurralRepositorio;
            _funcoesRepositorio = funcoesRepositorio;
        }

        public async Task InserirKgRealizado(List<FornecimentoConfinamentoDTO> models)
        {
            foreach (var item in models)
            {
                //if (!item.KgRealizado.HasValue) continue;

                var fornecimento = await _repositorio.ObterPorId(item.Id);
                if (fornecimento is null)
                {
                    Notificar($"Fornecimento do dia {item.DataFornecimento.ToShortDateString()} para o Curral {item.Curral} não encontrado");
                    break;
                }

                fornecimento.KgRealizado = item.KgRealizado;

                await _repositorio.Atualizar(fornecimento);
            }

            if (TemNotificacao()) return;

            await _repositorio.UnitOfWork.Commit();
        }

        public async Task<List<FornecimentoConfinamentoDTO>> ObterPorData(DateTime dataFornecimento)
        {
            return await _repositorio.ObterPorData(dataFornecimento);
        }

        public async Task<List<FornecimentoConfinamentoDTO>> ObterTodosFiltro(FiltroFornecimentoConfinamentoDTO filtro)
        {
            return await _repositorio.ObterTodosFiltro(filtro);
        }

        public async Task<FornecimentoConfinamentoDTO> RecalcularPrevisto(int id)
        {
            var fornecimento = await _repositorio.ObterPorId(id);

            if (fornecimento is null)
            {
                Notificar("Fornecimento não encontrado no banco de dados");
                return null;
            }

            if (!await _repositorio.EhPrimeiroFornecimentoDoLote(fornecimento.IdCurral, fornecimento.DataFornecimento))
            {
                Notificar($"Apenas é possível recalcular o KG Previsto para o primeiro dia de confinamento.");
                return null;
            }

            var local = await _pastoCurralRepositorio.ObterPorId(fornecimento.IdCurral);
            if (local is null)
            {
                Notificar("Curral não encontrado no banco de dados");
                return null;
            }

            await _funcoesRepositorio.AtualizaGmd(AppUser.ObterIdCliente());

            await _funcoesRepositorio.AtualizaLote(AppUser.ObterIdCliente(), 0, fornecimento.DataFornecimento);

            var lote = await _loteEntradaRepositorio.ObterLotePorLocal(fornecimento.IdCurral);

            if (lote is null)
            {
                Notificar($"Lote não encontrado na base de dados");
                return null;
            }

            var planejamento = await _planejamentoNutricionalRepositorio.ObterPorId(lote.IdPlanejamento);

            if (planejamento is null)
            {
                Notificar("Planejamento do Lote não encontrado na base de dados");
                return null;
            }

            if (planejamento.PlanejamentoValoresConfinamento.Count == 0)
            {
                Notificar("Valores de Confinamento do Planejamento Nutricional não estão definidos");
                return null;
            }

            var valorConfinamento = planejamento.PlanejamentoValoresConfinamento.OrderBy(x => x.DiaInicio).First();

            var racao = await _racaoRepositorio.ObterPorId(valorConfinamento.IdRacao);
            if (racao is null)
            {
                Notificar("Ração do Planejamento Nutricional não encontrado no banco de dados");
                return null;
            }

            decimal consumoPrevistoMS = (valorConfinamento.ImspvEsperado * lote.PesoMedioProjetado.Value) / 100;
            decimal valorKgPrevisto = ((consumoPrevistoMS / racao.MateriaSeca) * 100) * local.Lotacao.Value;

            fornecimento.QuantidadeAnimais = local.Lotacao.Value;
            fornecimento.IdRacao = racao.Id;
            fornecimento.KgPrevisto = valorKgPrevisto;
            fornecimento.MateriaSecaRacao = racao.MateriaSeca;

            await _repositorio.Atualizar(fornecimento);

            await _repositorio.UnitOfWork.Commit();

            return new FornecimentoConfinamentoDTO
            {
                Ajuste = fornecimento.Ajuste,
                Curral = local.Nome,
                DataFornecimento = fornecimento.DataFornecimento,
                EhPrimeiroDia = true,
                KgPrevisto = fornecimento.KgPrevisto,
                KgRealizado = fornecimento.KgRealizado,
                MateriaSecaRacao = fornecimento.MateriaSecaRacao,
                Id = fornecimento.Id,
                NomeRacao = racao.Nome,
                QuantidadeAnimais = fornecimento.QuantidadeAnimais
            };
        }

        public async Task RemoverForncimentos(List<FornecimentoConfinamentoDTO> models)
        {
            foreach (var item in models)
            {
                var fornecimento = await _repositorio.ObterPorId(item.Id);

                if (fornecimento is null)
                {
                    Notificar($"Fornecimento do dia {item.DataFornecimento.ToShortDateString()} para o Curral {item.Curral} não encontrado");
                    break;
                }

                if (await _repositorio.EhPrimeiroFornecimentoDoLote(fornecimento.IdCurral, fornecimento.DataFornecimento))
                {
                    Notificar($"Não é possível excluir o primeiro fornecimento do lote (Curral {item.Curral}).");
                    break;
                }

                await _repositorio.Remover(fornecimento);
            }

            if (TemNotificacao()) return;

            await _repositorio.UnitOfWork.Commit();
        }
    }
}
