using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class LeituraCochoService : Service, ILeituraCochoService
    {
        private readonly ILeituraCochoRepositorio _repositorio;
        private readonly IPastoCurralRepositorio _pastoCurralRepositorio;
        private readonly ILoteEntradaRepositorio _loteEntradaRepositorio;
        private readonly IFornecimentoConfinamentoRepositorio _fornecimentoRepositorio;
        private readonly IFaseDoAnoRepositorio _faseDoAnoRepositorio;
        private readonly IPlanejamentoNutricionalRepositorio _planejamentoNutricionalRepositorio;
        private readonly IRacaoRepositorio _racaoRepositorio;
        private readonly IFuncoesRepositorio _funcoesRepositorio;

        public LeituraCochoService(INotificador notificador,
            IUser appUser,
            ILeituraCochoRepositorio repositorio,
            IPastoCurralRepositorio pastoCurralRepositorio,
            ILoteEntradaRepositorio loteEntradaRepositorio,
            IFornecimentoConfinamentoRepositorio fornecimentoRepositorio,
            IRacaoRepositorio racaoRepositorio,
            IPlanejamentoNutricionalRepositorio planejamentoNutricionalRepositorio,
            IFaseDoAnoRepositorio faseDoAnoRepositorio, IFuncoesRepositorio funcoesRepositorio) : base(notificador, appUser)
        {
            _repositorio = repositorio;
            _pastoCurralRepositorio = pastoCurralRepositorio;
            _loteEntradaRepositorio = loteEntradaRepositorio;
            _fornecimentoRepositorio = fornecimentoRepositorio;
            _racaoRepositorio = racaoRepositorio;
            _planejamentoNutricionalRepositorio = planejamentoNutricionalRepositorio;
            _faseDoAnoRepositorio = faseDoAnoRepositorio;
            _funcoesRepositorio = funcoesRepositorio;
        }

        public async Task InserirLeitura(List<LeituraCochoInsercaoDTO> models)
        {
            var entites = new List<LeituraCocho>();
            var fornecimentos = new List<FornecimentoConfinamento>();

            foreach (var item in models)
            {
                //não salvar os locais que não houve lançamento
                if (!item.Ajuste.HasValue) continue;

                //busca o curral no banco de dados.
                var curral = await _pastoCurralRepositorio.ObterPorId(item.IdCurral);

                if (curral is null)
                {
                    Notificar($"Curral {item.Curral} não encontrado no banco de dados");
                    break;
                }

                //verifica se curral possui lote ativo
                if (!await _pastoCurralRepositorio.ExisteLoteAtivo(curral.Id))
                {
                    Notificar($"Não existe Lote ativo para o Curral {curral.Nome}");
                    break;
                }

                //validar se existe fornecimento
                if (await _fornecimentoRepositorio.ExisteFornecimento(curral.Id, item.DataLeitura.Value))
                    continue;

                //busca e valida o lote de entrada
                var lote = await _loteEntradaRepositorio.ObterLotePorLocal(curral.Id);
                if (lote is null)
                {
                    Notificar($"Lote de Entrada do Curral {curral.Nome} não encontrado");
                    break;
                }

                if (!lote.IdRacaoAtual.HasValue)
                {
                    Notificar($"Ração Atual do Lote de Entrada do Curral {curral.Nome} não está definida");
                    break;
                }

                if (!lote.IdFasePlanejamento.HasValue)
                {
                    Notificar($"Fase do Planejamento Lote de Entrada do Curral {curral.Nome} não está definida");
                    break;
                }

                if (!lote.PesoMedioProjetado.HasValue)
                {
                    Notificar($"Peso Projetado do Lote de Entrada do Curral {curral.Nome} não está definido");
                    break;
                }

                if (!lote.DiaAtual.HasValue)
                {
                    Notificar($"Dia Atual do Confinamento do Lote de Entrada do Curral {curral.Nome} não está definido");
                    break;
                }

                if (lote.DiaAtual.Value == 1)
                {
                    Notificar($"Não é permitido leitura de cocho no primeiro dia do lote(Curral: {curral.Nome}). ");
                    break;
                }

                var racao = await _racaoRepositorio.ObterPorId(lote.IdRacaoAtual.Value);

                if (racao is null)
                {
                    Notificar($"Ração Atual do Lote de Entrada do Curral {curral.Nome} não foi encontrado no banco de dados");
                    break;
                }

                var planejamentoNutricional = await _planejamentoNutricionalRepositorio.ObterPorId(lote.IdPlanejamento);

                if (planejamentoNutricional is null)
                {
                    Notificar($"Planejamento Nutricional do Curral {curral.Nome} não encontrado no banco de dados");
                    break;
                }

                //var faseAtual = await _faseDoAnoRepositorio.ObterPorId(lote.IdFasePlanejamento.Value);
                //if (faseAtual is null)
                //{
                //    Notificar($"Fase do Ano Atual do Lote de Entrada do Curral {curral.Nome} não encontrado no banco de dados");
                //    break;
                //}

                var fornecimentoDiaAnterior = await _fornecimentoRepositorio.Buscar(x => x.DataFornecimento.Date
                    .CompareTo(item.DataLeitura.Value.AddDays(-1).Date) == 0 && x.IdCurral == curral.Id && x.KgRealizado.HasValue);

                if (fornecimentoDiaAnterior.Count == 0 && lote.DiaAtual.Value > 1)
                {
                    Notificar($"Lote do local {curral.Nome} sem Fornecimento no dia anterior");
                    break;
                }

                var quantidadeAnimais = await _loteEntradaRepositorio.ObterQuantidadeNoLoteNaData(lote.Id, item.DataLeitura.Value);

                if (quantidadeAnimais == 0)
                {
                    Notificar($"Lote do local {curral.Nome} não possui animal na data {item.DataLeitura.Value.ToShortDateString()}");
                    break;
                }

                var model = new LeituraCocho
                {
                    Id = item.Id.HasValue ? item.Id.Value : 0,
                    AjusteGramas = item.Ajuste.Value,
                    DataLeitura = item.DataLeitura.Value,
                    IdCliente = AppUser.ObterIdCliente(),
                    IdLocal = curral.Id,
                    IdLote = lote.Id,
                    IdPlanejamento = lote.IdPlanejamento,
                    QuantidadeAnimais = quantidadeAnimais,
                    Status = Enums.Status.Ativado
                };

                entites.Add(model);

                var kgPrevisto = CalcularKgPrevistoFornecimento(planejamentoNutricional, racao, fornecimentoDiaAnterior.FirstOrDefault(),
                    item.Ajuste.Value, lote.PesoMedioProjetado.Value, lote.DiaAtual.Value, lote.IdFasePlanejamento.Value, quantidadeAnimais);

                if (TemNotificacao()) break;

                var fornecimento = new FornecimentoConfinamento
                {
                    IdCliente = AppUser.ObterIdCliente(),
                    IdCurral = curral.Id,
                    Ajuste = item.Ajuste.Value,
                    DataFornecimento = model.DataLeitura,
                    IdRacao = racao.Id,
                    QuantidadeAnimais = quantidadeAnimais,
                    MateriaSecaRacao = racao.MateriaSeca,
                    KgPrevisto = kgPrevisto,
                    IdLote = lote.Id
                };

                fornecimentos.Add(fornecimento);

            }

            if (TemNotificacao()) return;

            foreach (var item in entites)
            {
                if (item.Id == 0)
                    await _repositorio.Adicionar(item);
                else
                    await _repositorio.Atualizar(item);
            }

            foreach (var item in fornecimentos) await _fornecimentoRepositorio.Adicionar(item);

            await _repositorio.UnitOfWork.Commit();

        }

        public decimal CalcularKgPrevistoFornecimento(PlanejamentoNutricional planejamento,
            Racao racao,
            FornecimentoConfinamento fornecimentoDiaAnterior,
            decimal ajuste,
            decimal pesoMedioProjeto,
            int diaAtualDieta,
            int idFasePlanejamento,
            int quantidadeAnimais)
        {
            decimal valorKgPrevisto = 0;
            decimal consumoPrevistoMS;

            //if (diaAtualDieta == 1)
            //{
            //    var valorConfinamento = planejamento.PlanejamentoValoresConfinamento
            //                                .FirstOrDefault(x => x.Id == idFasePlanejamento);
            //    if (valorConfinamento is null)
            //    {
            //        Notificar("Valores de Confinamento não encontrado");
            //        return 0;
            //    }
            //    consumoPrevistoMS = (valorConfinamento.ImspvEsperado * pesoMedioProjeto) / 100;                
            //} 
            //else
            //{
            //Caso o KG realizado é igual a 0, usar o KG Previsto
            var kgRealizado = fornecimentoDiaAnterior.KgRealizado.Value == 0 ? fornecimentoDiaAnterior.KgPrevisto
                                          : fornecimentoDiaAnterior.KgRealizado.Value;

            consumoPrevistoMS = (kgRealizado * (racao.MateriaSeca / 100)) * (1 + (ajuste / 100));

            //}

            valorKgPrevisto = ((consumoPrevistoMS / racao.MateriaSeca) * 100);

            return valorKgPrevisto;
        }

        public async Task<List<LeituraCochoInsercaoDTO>> ObterLeiturasInsercao(DateTime dataLeitura)
        {
            var leituras = await _repositorio.ObterLeiturasInsercao(dataLeitura);

            foreach (var leitura in leituras)
            {
                await _funcoesRepositorio.AtualizaLote(AppUser.ObterIdCliente(), leitura.IdLote, dataLeitura);
            }

            return leituras;
        }

        public async Task<List<LeituraCochoDTO>> ObterTodosFiltro(FiltroLeituraCochoDTO filtro)
        {
            return await _repositorio.ObterTodosFiltro(filtro);
        }



    }
}
