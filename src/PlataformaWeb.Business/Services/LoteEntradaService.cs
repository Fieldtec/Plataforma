using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Models.Cadastro;
using PlataformaWeb.Business.Models.Validations;
using PlataformaWeb.Business.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class LoteEntradaService : Service, ILoteEntradaService
    {
        private readonly ILoteEntradaRepositorio _loteEntradaRepositorio;
        private readonly IPastoCurralRepositorio _pastoCurralRepositorio;
        private readonly IPlanejamentoNutricionalRepositorio _planejamentoNutricionalRepositorio;
        private readonly IFornecimentoConfinamentoRepositorio _fornecimentoConfinamentoRepositorio;
        private readonly IRacaoRepositorio _racaoRepositorio;
        private readonly IFuncoesRepositorio _funcoesRepositorio;

        public LoteEntradaService(INotificador notificador,
                                  IUser appUser,
                                  ILoteEntradaRepositorio loteEntradaRepositorio,
                                  IPastoCurralRepositorio pastoCurralRepositorio,
                                  IPlanejamentoNutricionalRepositorio planejamentoNutricionalRepositorio,
                                  IFornecimentoConfinamentoRepositorio fornecimentoConfinamentoRepositorio, 
                                  IRacaoRepositorio racaoRepositorio, 
                                  IFuncoesRepositorio funcaoRepositorio) : base(notificador, appUser)
        {
            _loteEntradaRepositorio = loteEntradaRepositorio;
            _pastoCurralRepositorio = pastoCurralRepositorio;
            _planejamentoNutricionalRepositorio = planejamentoNutricionalRepositorio;
            _fornecimentoConfinamentoRepositorio = fornecimentoConfinamentoRepositorio;
            _racaoRepositorio = racaoRepositorio;
            _funcoesRepositorio = funcaoRepositorio;
        }

        private async Task<bool> LocalPossuiLoteAtivo(LoteEntrada loteEntrada)
        {
            var loteAtivo = await _loteEntradaRepositorio.BuscarQuery(x => x.IdLocal == loteEntrada.IdLocal);

            if (loteAtivo.Count > 0)
            {
                loteEntrada.Id = loteAtivo.FirstOrDefault().Id;
            }

            return loteAtivo.Count > 0;
        }

        public async Task Adicionar(LoteAnimalCadastro entity)
        {
            var loteEntrada = new LoteEntrada(entity);

            FornecimentoConfinamento fornecimentoPrimeiroDia = null;


            if (!ValidaInsercaoAtualizacaoCliente(loteEntrada)) return;

            if (!ExecutarValidacao(new LoteEntradaValidation(Enums.TipoOperacao.Inclusao), loteEntrada)) return;

            if (await LocalPossuiLoteAtivo(loteEntrada))
            {
                loteEntrada.AnimaisLote.ForEach(x => x.IdLote = loteEntrada.Id);
                await _loteEntradaRepositorio.Atualizar(loteEntrada);
            } 
            else
            {
                fornecimentoPrimeiroDia = await InserirFornecimentoPrimeiroDia(loteEntrada, entity.PesoEntrada, entity.QuantidadeAnimais);
                if (TemNotificacao()) return;

                if (fornecimentoPrimeiroDia != null)
                {
                    loteEntrada.FornecimentosConfinamento.Add(fornecimentoPrimeiroDia);
                }

                await _loteEntradaRepositorio.Adicionar(loteEntrada);
            }

            await AtualizaLotacaoLocal(loteEntrada.IdLocal, entity.QuantidadeAnimais);

            if (TemNotificacao()) return;

            await _loteEntradaRepositorio.UnitOfWork.Commit();

            await _funcoesRepositorio.AtualizaGmd(AppUser.ObterIdCliente());

            await _funcoesRepositorio.AtualizaLote(AppUser.ObterIdCliente(), loteEntrada.Id, loteEntrada.DataEntrada);
        }

        public async Task Atualizar(LoteAnimalCadastro entity)
        {
            var loteEdicao = await _loteEntradaRepositorio.ObterPorId(entity.Id);

            if (loteEdicao is null)
            {
                Notificar("Lote não foi encontrado");
                return;
            }

            if (loteEdicao.IdPlanejamento != entity.Planejamento.Id)
            {
                //lógica para gravar

                loteEdicao.IdPlanejamento = entity.Planejamento.Id;
            }

            loteEdicao.DataEntrada = entity.DataEntrada;            
            loteEdicao.AnimaisLote.ForEach(x =>
            {
                x.IdCategoria = entity.Categoria.Id;
                x.PesoEntrada = entity.PesoEntrada;
                x.IdRaca = entity.Raca.Id;
                x.DataEntrada = entity.DataEntrada;
            });

            if (!ValidaInsercaoAtualizacaoCliente(loteEdicao)) return;

            if (!ExecutarValidacao(new LoteEntradaValidation(Enums.TipoOperacao.Atualizacao), loteEdicao)) return;

            await _loteEntradaRepositorio.Atualizar(loteEdicao);            

            await _loteEntradaRepositorio.UnitOfWork.Commit();

            await _funcoesRepositorio.AtualizaGmd(AppUser.ObterIdCliente());

            await _funcoesRepositorio.AtualizaLote(AppUser.ObterIdCliente(), loteEdicao.Id, loteEdicao.DataEntrada);
        }

        public async Task<List<LoteAnimalDTO>> Buscar(Expression<Func<LoteEntrada, bool>> predicate)
        {
            return await _loteEntradaRepositorio.BuscarQuery(predicate);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public async Task<List<LoteAnimalDTO>> ObterPaginacao(int? id = null)
        {
            return await _loteEntradaRepositorio.ObterPaginacao();
        }

        public async Task<LoteAnimalCadastro> ObterPorId(int id)
        {
            return await _loteEntradaRepositorio.ObterLoteCadastroPorId(id);
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id do Lote é inválido");
                return;
            }

            var loteEntrada = await _loteEntradaRepositorio.ObterPorId(id);

            if (loteEntrada is null)
            {
                Notificar("Lote de entrada não existe");
                return;
            }

            loteEntrada.AnimaisLote.ForEach(x => x.Status = Status.Desativado);

            await _loteEntradaRepositorio.Remover(loteEntrada);

            await AtualizaLotacaoLocal(loteEntrada.IdLocal, loteEntrada.AnimaisLote.Count * -1);

            await _loteEntradaRepositorio.UnitOfWork.Commit();
        }

        private async Task AtualizaLotacaoLocal(int idLocal, int quantidadeAnimal)
        {
            var local = await _pastoCurralRepositorio.ObterPorId(idLocal);
            if (local is null)
            {
                Notificar("Local não existe no banco de dados");
                return;
            }

            local.IncluirAnimais(quantidadeAnimal);

            //if (!local.Lotacao.HasValue) local.Lotacao = 0;
            //local.Lotacao += quantidadeAnimal;

            await _pastoCurralRepositorio.Atualizar(local);
        }

        #region ...::: Morte de Animal :::...

        public async Task<List<MorteAnimalDTO>> ObterMortesPaginacao()
        {
            return await _loteEntradaRepositorio.ObterMortesPaginacao();
        }

        public async Task<MorteAnimalDTO> ObterMorte(int idLote, DateTime dataMorte)
        {
            return await _loteEntradaRepositorio.ObterMorte(idLote, dataMorte);
        }

        public async Task RegistrarMortes(MorteAnimalCadastro morte)
        {
            if (!ExecutarValidacao(new MorteAnimalCadastroValidation(), morte)) return;

            var loteEntrada = await _loteEntradaRepositorio.ObterPorId(morte.LoteEntrada.Id);

            if (loteEntrada is null)
            {
                Notificar("Lote de Entrada não encontrado");
                return;
            }

            if (!ValidarDataDaMorte(morte, loteEntrada)) return;

            if (morte.QuantidadeAnimais > loteEntrada.AnimaisLote.Count)
            {
                Notificar($"Quantidade de animais mortos informados({morte.QuantidadeAnimais}) é maior que a quantidade de animais do lote({loteEntrada.AnimaisLote.Count}).");
                return;
            }

            var animaisMortos = loteEntrada.RealizarMorteAnimais(morte.QuantidadeAnimais, morte.CausaMorte.Id, morte.DataMorte);

            await _loteEntradaRepositorio.AtualizarAnimais(animaisMortos);

            await AtualizaLotacaoLocal(loteEntrada.IdLocal, morte.QuantidadeAnimais * -1);

            await _loteEntradaRepositorio.UnitOfWork.Commit();

        }

        public async Task ExcluirMorte(MorteAnimalDTO morte)
        {
            var loteEntrada = await _loteEntradaRepositorio.ObterPorId(morte.IdLote);

            if (loteEntrada is null)
            {
                Notificar("Lote de Entrada não encontrado");
                return;
            }

            var animaisMortos = await _loteEntradaRepositorio.ObterAnimaisMortos(morte.IdLote, morte.DataMorte);

            if (animaisMortos.Count != morte.QuantidadeAnimais)
            {
                Notificar($"Quantidade de Animais Mortos para a Data data informada({animaisMortos.Count}) diverge com a Quantidade enviada({morte.QuantidadeAnimais}).");
                return;
            }

            foreach (var animal in animaisMortos)            
                animal.CancelarMorte();            

            await _loteEntradaRepositorio.AtualizarAnimais(animaisMortos);

            await AtualizaLotacaoLocal(loteEntrada.IdLocal, morte.QuantidadeAnimais);

            await _loteEntradaRepositorio.UnitOfWork.Commit();
            
        }
        
        private bool ValidarDataDaMorte(MorteAnimalCadastro morte, LoteEntrada lote)
        {
            if (morte.DataMorte.Date.CompareTo(DateTime.Now.Date) > 0)
            {
                Notificar("Data da Morte não pode ser maior do que a Data de Hoje");
                return false;
            }

            var dataMinimaEntradaAnimal = lote.AnimaisLote.Min(x => x.DataEntrada);

            if (morte.DataMorte.CompareTo(dataMinimaEntradaAnimal) < 0)
            {
                Notificar($"Data da Morte({morte.DataMorte.ToShortDateString()}) não pode ser menor do que a Data de Entrada do Animal no Lote({dataMinimaEntradaAnimal.ToShortDateString()}).");
                return false;
            }

            return true;
        }

        #endregion

        public async Task<FornecimentoConfinamento> InserirFornecimentoPrimeiroDia(LoteEntrada lote, decimal pesoMedio, int quantidadeAnimais)
        {
            var curral = await _pastoCurralRepositorio.ObterPorId(lote.IdLocal);
            if (curral.Tipo == TipoPastoCurral.Curral)
            {
                var planejamento = await _planejamentoNutricionalRepositorio.ObterPorId(lote.IdPlanejamento);

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

                lote.DiaAtual = 1;
                lote.IdFasePlanejamento = valorConfinamento.Id;
                lote.IdRacaoAtual = valorConfinamento.IdRacao;
                lote.PesoMedioProjetado = pesoMedio + valorConfinamento.GmdEsperado;                

                decimal consumoPrevistoMS = (valorConfinamento.ImspvEsperado * lote.PesoMedioProjetado.Value) / 100;
                decimal valorKgPrevisto = ((consumoPrevistoMS / racao.MateriaSeca) * 100) * quantidadeAnimais;

                FornecimentoConfinamento fornecimento = new FornecimentoConfinamento
                {
                    Ajuste = 0,
                    QuantidadeAnimais = quantidadeAnimais,
                    DataFornecimento = lote.DataEntrada,
                    IdCliente = AppUser.ObterIdCliente(),
                    IdCurral = curral.Id,
                    IdRacao = racao.Id,
                    KgPrevisto = valorKgPrevisto,
                    MateriaSecaRacao = racao.MateriaSeca,
                    Status = Status.Ativado
                };

                //await _fornecimentoConfinamentoRepositorio.Adicionar(fornecimento);

                return fornecimento;
            }

            return null;
        }

    }
}
