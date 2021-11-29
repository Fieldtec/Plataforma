using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Extensions;
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
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class PrevisaoFornecimentoPastoService : Service, IPrevisaoFornecimentoPastoService
    {
        private readonly IPrevisaoFornecimentoPastoRepositorio _repositorio;
        private readonly ISuplementoMineralRepositorio _suplementoRepositorio;
        private readonly IPlanejamentoNutricionalRepositorio _planejamentoNutricionalRepositorio;
        private readonly ILoteEntradaRepositorio _loteEntradaRepositorio;
        private readonly IFuncoesRepositorio _funcoesRepositorio;
        private readonly IConfiguracaoFornecimentoPastoRepositorio _configuracaoFornecimentoRepositorio;

        public PrevisaoFornecimentoPastoService(INotificador notificador,
            IUser appUser,
            IPrevisaoFornecimentoPastoRepositorio repositorio,
            IConfiguracaoFornecimentoPastoRepositorio configuracaoFornecimentoRepositorio,
            ISuplementoMineralRepositorio suplementoRepositorio,
            IPlanejamentoNutricionalRepositorio planejamentoNutricional,
            ILoteEntradaRepositorio loteEntradaRepositorio,
            IFuncoesRepositorio funcoesRepositorio) : base(notificador, appUser)
        {
            _repositorio = repositorio;
            _configuracaoFornecimentoRepositorio = configuracaoFornecimentoRepositorio;
            _suplementoRepositorio = suplementoRepositorio;
            _planejamentoNutricionalRepositorio = planejamentoNutricional;
            _loteEntradaRepositorio = loteEntradaRepositorio;
            _funcoesRepositorio = funcoesRepositorio;
        }

        public async Task Adicionar(PrevisaoFornecimentoPasto entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new PrevisaoFornecimentoPastoValidation(), entity)) return;

            await _repositorio.Adicionar(entity);

            await _repositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(PrevisaoFornecimentoPasto entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new PrevisaoFornecimentoPastoValidation(), entity)) return;

            await _repositorio.Atualizar(entity);

            await _repositorio.UnitOfWork.Commit();
        }

        public async Task<List<PrevisaoFornecimentoPastoDTO>> Buscar(FiltroPrevisaoFornecimentoPastoDTO filtro)
        {
            if (filtro.DataInicio.Date.CompareTo(filtro.DataFinal.Date) > 0)
            {
                Notificar("Data Inicial deverá ser menor que a Data Final");
                return null;
            }

            //var predicate = PredicateBuilder.True<PrevisaoFornecimentoPasto>()
            //                    .And(x => x.DataPrevisao.CompareTo(filtro.DataInicio) >= 0 && x.DataPrevisao.CompareTo(filtro.DataFinal) <= 0);

            //if (filtro.IdPasto.HasValue)
            //{
            //    predicate = predicate.And(x => x.IdPasto == filtro.IdPasto.Value);
            //}

            return await _repositorio.BuscarQuery(filtro);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        private bool ValidaDadosGeracao(GeracaoFornecimentoPastoDTO model)
        {
            if (model.DataInicial.Date.CompareTo(DateTime.Now.Date) < 0)
            {
                Notificar("Não é permitido a geração de previsão de fornecimento para datas retroativas");
                return false;
            }

            if (model.QuantidadeSemanas == 0)
            {
                Notificar("Quantidade de Semanas precisa ser maior ou igual a 1");
                return false;
            }

            if (model.IdSuplemento <= 0)
            {
                Notificar("Suplemento não informado");
                return false;
            }

            return true;
        }

        public async Task GerarPrevisoes(GeracaoFornecimentoPastoDTO model)
        {

            if (!ValidaDadosGeracao(model)) return;

            var suplementoMineral = await _suplementoRepositorio.ObterPorId(model.IdSuplemento);

            if (suplementoMineral is null)
            {
                Notificar("Suplemento não existe no banco de dados");
                return;
            }

            //verificar se existe em algum plano nutricional e busca-los
            var planejamentosValoresPasto = await _planejamentoNutricionalRepositorio.BuscarPlanejamentoContemSuplemento(suplementoMineral.Id);
            if (planejamentosValoresPasto.Count == 0)
            {
                Notificar("Suplemento não possui vinculo em nenhum Planejamento Nutricional");
                return;
            }

            await _funcoesRepositorio.AtualizaGmd(AppUser.ObterIdCliente());
            await _funcoesRepositorio.AtualizaLote(AppUser.ObterIdCliente(), model.DataInicial);

            var lotesEntrada = await _loteEntradaRepositorio.Buscar(x => planejamentosValoresPasto.Select(x => x.IdPlanejamento).ToList().Contains(x.IdPlanejamento));

            if (lotesEntrada.Count == 0)
            {
                Notificar("Nenhum Lote foi encontrado");
                return;
            }

            var previsoes = await ObterPrevisoes(model, suplementoMineral, lotesEntrada, planejamentosValoresPasto);

            if (TemNotificacao()) return;

            if (previsoes.Count == 0)
            {
                Notificar("Nenhuma previsão foi gerada");
                return;
            }

            foreach (var previsao in previsoes)
            {
                if (!ExecutarValidacao(new PrevisaoFornecimentoPastoValidation(), previsao)) break;

                await _repositorio.Adicionar(previsao);
            }

            if (TemNotificacao()) return;

            await SalvarConfiguracaoFornecimentoPasto(model);

            await _repositorio.UnitOfWork.Commit();
        }

        private async Task<List<PrevisaoFornecimentoPasto>> ObterPrevisoes(
            GeracaoFornecimentoPastoDTO model,
            SuplementoMineral suplementoMineral,
            List<LoteEntrada> lotesEntrada,
            List<PlanejamentoValoresPasto> planejamentosValoresPasto)
        {
            bool erroSemPlanejamento = false;
            bool erroSemAnimal = false;
            List<PrevisaoFornecimentoPasto> previsoes = new List<PrevisaoFornecimentoPasto>();
            List<PrevisaoFornecimentoPasto> previsoesParaSalvar = new List<PrevisaoFornecimentoPasto>();

            foreach (var lote in lotesEntrada)
            {
                int quantidadeAnimais = await _loteEntradaRepositorio.ObterQuantidadeNoLoteNaData(lote.Id, model.DataInicial);
                decimal pesoMedio = await _loteEntradaRepositorio.ObterPesoMedio(lote.Id);

                if (quantidadeAnimais == 0)
                {
                    erroSemAnimal = true;
                    continue;
                };

                int quantidadeDeDias = model.QuantidadeSemanas * 7;

                //var valorPlanejamentoPasto = planejamentosValoresPasto.SingleOrDefault(x => x.IdPlanejamento == lote.IdPlanejamento);

                for (int i = 0; i < quantidadeDeDias; i++)
                {
                    var dataPrevisao = model.DataInicial.AddDays(i);
                    bool ignorarDia = false;

                    //caso usuário marcou para ignorar dia da semana
                    if ((dataPrevisao.DayOfWeek == DayOfWeek.Sunday && model.Domingo)
                        || (dataPrevisao.DayOfWeek == DayOfWeek.Saturday && model.Sabado)
                        || (dataPrevisao.DayOfWeek == DayOfWeek.Monday && model.Segunda)
                        || (dataPrevisao.DayOfWeek == DayOfWeek.Tuesday && model.Terca)
                        || (dataPrevisao.DayOfWeek == DayOfWeek.Wednesday && model.Quarta)
                        || (dataPrevisao.DayOfWeek == DayOfWeek.Thursday && model.Quinta)
                        || (dataPrevisao.DayOfWeek == DayOfWeek.Friday && model.Sexta))
                    {
                        ignorarDia = true;
                    }

                    PlanejamentoValoresPasto planejamentoAtual = null;

                    foreach (var valoresPasto in planejamentosValoresPasto.Where(x => x.IdPlanejamento == lote.IdPlanejamento))
                    {
                        var dtInicio = valoresPasto.FaseDoAno.DataInicio;
                        var dtFim = valoresPasto.FaseDoAno.DataFim;
                        if (dtInicio.HasValue && dtFim.HasValue && dataPrevisao.EstaEntre(dtInicio.Value, dtFim.Value))
                        {
                            planejamentoAtual = valoresPasto;
                            break;
                        }
                    }

                    if (planejamentoAtual is null)
                    {
                        erroSemPlanejamento = true;
                        continue;
                    };

                    var previsao = new PrevisaoFornecimentoPasto
                    {
                        IdCliente = AppUser.ObterIdCliente(),
                        IdLote = lote.Id,
                        IdPasto = lote.IdLocal,
                        IdSuplemento = model.IdSuplemento,
                        DataPrevisao = dataPrevisao,
                        QuantidadeAnimais = quantidadeAnimais
                    };

                    decimal gmd = (i + 1) * planejamentoAtual.GmdEsperado;
                    decimal pesoProjetado = gmd + pesoMedio;
                    decimal consumoPrevistoCabecaDia = pesoProjetado * (planejamentoAtual.ImspvEsperado / 100);
                    decimal consumoTotalKg = consumoPrevistoCabecaDia * quantidadeAnimais;
                    decimal consumoTotalSaco = 0;

                    if (suplementoMineral.PesoEmbalagem > 0)
                    {
                        consumoTotalSaco = consumoTotalKg / suplementoMineral.PesoEmbalagem;
                    }

                    previsao.PrevisaoKg = consumoTotalKg;
                    previsao.PrevisaoSaco = consumoTotalSaco;

                    await RemoverSeExistir(previsao);

                    previsoes.Add(previsao);

                    if (!ignorarDia)
                    {
                        previsoesParaSalvar.Add(previsao);
                    }

                }
            }

            if (previsoesParaSalvar.Count == 0)
            {
                if (erroSemPlanejamento)
                {
                    Notificar("Nenhuma fase do ano foi encontrada no(s) Planejamento(s) que atendessem a data informada");
                }

                if (erroSemAnimal)
                {
                    Notificar("Nenhum Animal encontrado no(s) lote(s) para a Data Informada");
                }
            }

            return previsoesParaSalvar;
        }

        private async Task RemoverSeExistir(PrevisaoFornecimentoPasto previsao)
        {
            var existePrevisao = await _repositorio.Buscar(x => x.IdLote == previsao.IdLote && x.IdPasto == previsao.IdPasto
                            && x.IdSuplemento == previsao.IdSuplemento && x.DataPrevisao.CompareTo(previsao.DataPrevisao.Date) == 0);

            if (existePrevisao.Count > 0)
            {
                foreach (var item in existePrevisao)
                {
                    await _repositorio.Remover(item);
                }
            }
        }

        private async Task SalvarConfiguracaoFornecimentoPasto(GeracaoFornecimentoPastoDTO model)
        {
            var configuracaoPrevisao = await _configuracaoFornecimentoRepositorio.ObterTodos();
            if (configuracaoPrevisao.Count == 0)
            {
                ConfiguracaoFornecimentoPasto confg = new ConfiguracaoFornecimentoPasto
                {
                    IdCliente = AppUser.ObterIdCliente(),
                    Domingo = !model.Domingo ? Status.Desativado : Status.Ativado,
                    Segunda = !model.Segunda ? Status.Desativado : Status.Ativado,
                    Terca = !model.Terca ? Status.Desativado : Status.Ativado,
                    Quarta = !model.Quarta ? Status.Desativado : Status.Ativado,
                    Quinta = !model.Quinta ? Status.Desativado : Status.Ativado,
                    Sexta = !model.Sexta ? Status.Desativado : Status.Ativado,
                    Sabado = !model.Sabado ? Status.Desativado : Status.Ativado,
                };

                await _configuracaoFornecimentoRepositorio.Adicionar(confg);
            }
            else
            {
                var configuracao = configuracaoPrevisao.FirstOrDefault();
                configuracao.Domingo = !model.Domingo ? Status.Desativado : Status.Ativado;
                configuracao.Segunda = !model.Segunda ? Status.Desativado : Status.Ativado;
                configuracao.Terca = !model.Terca ? Status.Desativado : Status.Ativado;
                configuracao.Quarta = !model.Quarta ? Status.Desativado : Status.Ativado;
                configuracao.Quinta = !model.Quinta ? Status.Desativado : Status.Ativado;
                configuracao.Sexta = !model.Sexta ? Status.Desativado : Status.Ativado;
                configuracao.Sabado = !model.Sabado ? Status.Desativado : Status.Ativado;

                await _configuracaoFornecimentoRepositorio.Atualizar(configuracao);

            }
        }

        public async Task<List<PrevisaoFornecimentoPastoDTO>> ObterPaginacao(int? id = null)
        {
            return await _repositorio.ObterPaginacao();
        }

        public async Task<PrevisaoFornecimentoPasto> ObterPorId(int id)
        {
            return await _repositorio.ObterPorId(id);
        }

        public async Task Remover(int id)
        {
            var model = await _repositorio.ObterPorId(id);
            if (model is null)
            {
                Notificar("Previsão do Fornecimento não encontrado");
                return;
            }

            await _repositorio.Remover(model);

            await _repositorio.UnitOfWork.Commit();

        }

        public async Task RemoverPrevisoes(List<PrevisaoFornecimentoPastoDTO> models)
        {
            foreach (var model in models)
            {
                var registro = await _repositorio.ObterPorId(model.Id);
                if (registro is null)
                {
                    Notificar("Previsão do Fornecimento não encontrado");
                    return;
                }

                await _repositorio.Remover(registro);

            }

            await _repositorio.UnitOfWork.Commit();
        }

        public async Task<ConfiguracaoFornecimentoPasto> ObterConfiguracaoFornecimento()
        {
            var models = await _configuracaoFornecimentoRepositorio.ObterTodos();
            return models.FirstOrDefault();
        }
    }
}
