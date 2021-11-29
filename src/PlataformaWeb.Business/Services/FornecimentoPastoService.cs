using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Models.Validations;
using PlataformaWeb.Business.Notificacoes;
using PlataformaWeb.Business.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PlataformaWeb.Business.Services
{
    public class FornecimentoPastoService : Service, IFornecimentoPastoService
    {

        private readonly IFornecimentoPastoRepositorio _repositorio;
        private readonly IPastoCurralRepositorio _pastoCurralRepositorio;
        private readonly ILoteEntradaRepositorio _loteEntradaRepositorio;
        private readonly IPlanejamentoNutricionalRepositorio _planejamentoRepositorio;
        private readonly IPrevisaoFornecimentoPastoRepositorio _previsaoFornecimentoRepositorio;

        public FornecimentoPastoService(INotificador notificador,
            IUser appUser,
            IFornecimentoPastoRepositorio repositorio,
            IPastoCurralRepositorio pastoCurralRepositorio,
            ILoteEntradaRepositorio loteEntradaRepositorio,
            IPlanejamentoNutricionalRepositorio planejamentoRepositorio, 
            IPrevisaoFornecimentoPastoRepositorio previsaoFornecimentoRepositorio)
            : base(notificador, appUser)
        {
            _repositorio = repositorio;
            _pastoCurralRepositorio = pastoCurralRepositorio;
            _loteEntradaRepositorio = loteEntradaRepositorio;
            _planejamentoRepositorio = planejamentoRepositorio;
            _previsaoFornecimentoRepositorio = previsaoFornecimentoRepositorio;
        }

        public async Task Adicionar(FornecimentoPasto entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new FornecimentoPastoValidation(), entity)) return;

            await _repositorio.Adicionar(entity);

            await _repositorio.UnitOfWork.Commit();
        }

        public Task Atualizar(FornecimentoPasto entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FornecimentoPastoDTO>> Buscar(FiltroFornecimentoPastoDTO filtro)
        {
            if (filtro.DataInicio.Date.CompareTo(filtro.DataFinal.Date) > 0)
            {
                Notificar("Data Inicial não pode ser maior do que a Data Final");
                return null;
            }

            return await _repositorio.BuscarQuery(filtro);
        }

        public async Task<PreparaDadosFornecimentoPastoDTO> BuscarDadosLancamento(FiltroFornecimentoPastoDTO filtro)
        {
            var pastoCurral = await _pastoCurralRepositorio.ObterPorId(filtro.IdPasto.Value);
            if (pastoCurral is null)
            {
                Notificar("Pasto não encontrado no banco de dados");
                return null;
            }

            var loteEntrada = await _loteEntradaRepositorio.ObterLotePorLocal(pastoCurral.Id);
            if (loteEntrada is null)
            {
                Notificar("Pasto não possui lote ativo");
                return null;
            }
            
            var planejamentoNutricional = await _planejamentoRepositorio.ObterPorId(loteEntrada.IdPlanejamento);
            if (planejamentoNutricional is null)
            {
                Notificar("Planejamento Nutricional do Lote não encontrado");
                return null;
            }

            int quantidadeAnimais = await _loteEntradaRepositorio.ObterQuantidadeNoLoteNaData(loteEntrada.Id, filtro.DataFinal.Date);
            if (quantidadeAnimais == 0)
            {
                Notificar("Lote não contém animais ativos na Data informada");
                return null;
            }

            var pesoMedio = await _loteEntradaRepositorio.ObterPesoMedio(loteEntrada.Id);

            PlanejamentoValoresPasto planejamentoAtual = null;

            foreach (var valoresPasto in planejamentoNutricional.PlanejamentoValoresPasto)
            {
                var dtInicio = valoresPasto.FaseDoAno.DataInicio;
                var dtFim = valoresPasto.FaseDoAno.DataFim;

                if (dtInicio.HasValue && dtFim.HasValue && filtro.DataInicio.EstaEntre(dtInicio.Value, dtFim.Value))
                {
                    planejamentoAtual = valoresPasto;
                    break;
                }
            }

            if (planejamentoAtual is null)
            {
                Notificar("Nenhum Planejamento de Pasto encontrado para a Data informada");
                return null;
            }

            var fornecimento = await _repositorio.Buscar(x => x.IdLote == loteEntrada.Id && x.IdPasto == pastoCurral.Id
                                                           && x.DataRealizado.CompareTo(filtro.DataFinal.Date) == 0 && x.IdSuplemento == planejamentoAtual.IdSuplemento); 

            if (fornecimento.Count > 0)
            {
                Notificar("Já existe fornecimento lançado para a Data informada");
                return null;
            }
            
            var previsaoFornecimento = await _previsaoFornecimentoRepositorio
                .Buscar(x => x.IdLote == loteEntrada.Id && x.IdPasto == pastoCurral.Id && x.DataPrevisao.CompareTo(filtro.DataFinal.Date) == 0 && x.IdSuplemento == planejamentoAtual.IdSuplemento);

            var ultimoFornecimento = await _repositorio.BuscarUltimoFornecimento(loteEntrada.Id, pastoCurral.Id, planejamentoAtual.IdSuplemento);

            PreparaDadosFornecimentoPastoDTO model = new PreparaDadosFornecimentoPastoDTO
            {
                PesoEmbalagem = planejamentoAtual.SuplementoMineral.PesoEmbalagem,
                PesoVivo = planejamentoAtual.SuplementoMineral.ConsumoEsperado,
                PrevisaoKg = previsaoFornecimento.FirstOrDefault()?.PrevisaoKg ?? 0,
                PrevisaoSaco = previsaoFornecimento.FirstOrDefault()?.PrevisaoSaco ?? 0,
                QuantidadeAnimais = quantidadeAnimais,
                IdSuplemento = planejamentoAtual.SuplementoMineral.Id,
                IdLote = loteEntrada.Id,
                Suplemento = planejamentoAtual.SuplementoMineral.Nome,
                PesoProjetado = pesoMedio,
                UltimoFornecimento = ultimoFornecimento?.DataRealizado ?? null
            };

            return model;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public Task<List<FornecimentoPastoDTO>> ObterPaginacao(int? id = null)
        {
            throw new NotImplementedException();
        }

        public async Task<FornecimentoPasto> ObterPorId(int id)
        {
            return await _repositorio.ObterPorId(id);
        }

        public async Task Remover(List<FornecimentoPastoDTO> models)
        {
            foreach (var item in models)
            {
                await Remover(item.Id);

                if (TemNotificacao()) break;
            }

            if (TemNotificacao()) return;

            await _repositorio.UnitOfWork.Commit();
        }

        public async Task Remover(int id)
        {
            var model = await _repositorio.ObterPorId(id);
            if (model is null)
            {
                Notificar("Fornecimento não encontrado");
                return;
            }

            await _repositorio.Remover(model);
        }
    }
}
