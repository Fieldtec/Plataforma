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
    public class MovimentacaoAnimalService : Service, IMovimentacaoAnimalService
    {

        private readonly IMovimentacaoAnimalRepositorio _movimentacaoAnimalRepositorio;
        private readonly ILoteEntradaRepositorio _loteEntradaRepositorio;
        private readonly IPastoCurralRepositorio _pastoCurralRepositorio;

        private LoteEntrada LoteDestinoCriado { get; set; } 

        public MovimentacaoAnimalService(INotificador notificador,
                        IUser appUser,
                        IMovimentacaoAnimalRepositorio movimentacaoAnimalRepositorio,
                        ILoteEntradaRepositorio loteEntradaRepositorio,
                        IPastoCurralRepositorio pastoCurralRepositorio) : base(notificador, appUser)
        {
            _movimentacaoAnimalRepositorio = movimentacaoAnimalRepositorio;
            _loteEntradaRepositorio = loteEntradaRepositorio;
            _pastoCurralRepositorio = pastoCurralRepositorio;

            LoteDestinoCriado = null;
        }

        private bool ValidarDadosDoLoteOrigem(MovimentacaoAnimalCadastro movimentacao, LoteEntrada loteOrigem)
        {
            if (movimentacao.QuantidadeAnimais == 0)
            {
                Notificar("Quantidade Animais da movimentação não pode ser 0");
                return false;
            }

            if (movimentacao.QuantidadeAnimais > loteOrigem.AnimaisLote.Count)
            {
                Notificar($"Quantidade de Animais na movimentação({movimentacao.QuantidadeAnimais}) é maior do que a quantidade no Lote de Origem({loteOrigem.AnimaisLote.Count}).");
                return false;
            }

            return true;
        }

        private bool ValidarDataMovimentacao(MovimentacaoAnimalCadastro movimentacao, LoteEntrada loteOrigem)
        {
            if (movimentacao.LocalOrigem.Id != loteOrigem.IdLocal)
            {
                Notificar("O Local no Lote de Origem diverge das informações enviadas");
                return false;
            }

            if (movimentacao.DataMovimentacao.Date.CompareTo(DateTime.Now.Date) > 0)
            {
                Notificar("Data da Movimentação não pode ser maior que a Data de Hoje");
                return false;
            }

            var dataMinimaOrigem = loteOrigem.AnimaisLote.Min(x => x.DataEntrada);

            if (movimentacao.DataMovimentacao.Date.CompareTo(dataMinimaOrigem.Date) < 0)
            {
                Notificar($"Data da Movimentação({movimentacao.DataMovimentacao.ToShortDateString()}) não pode ser inferior a Data da Movimentação de Animais({dataMinimaOrigem.ToShortDateString()}) no Lote de Origem");
                return false;
            }

            return true;
        }

        private async Task<LoteEntrada> CriaOuObtemLoteDestino(MovimentacaoAnimalCadastro entity)
        {
            LoteEntrada loteDestino = new LoteEntrada();

            if (entity.LoteDestino.Id != 0)
            {
                loteDestino = await _loteEntradaRepositorio.ObterPorId(entity.LoteDestino.Id);

                if (loteDestino is null)
                {
                    Notificar("Lote de Destino não encontrado");
                    return null;
                }
            }
            else
            {
                loteDestino.IdLocal = entity.LocalDestino.Id;

                if (entity.LoteDestino.Planejamento == null || entity.LoteDestino.Planejamento.Id <= 0)
                {
                    Notificar("Planejamento não foi informado");
                    return null;
                }

                loteDestino.IdPlanejamento = entity.LoteDestino.Planejamento.Id;
                loteDestino.DataEntrada = entity.DataMovimentacao;

                ValidaInsercaoAtualizacaoCliente(loteDestino);

                await _loteEntradaRepositorio.Adicionar(loteDestino);

                await _loteEntradaRepositorio.UnitOfWork.Commit();

                LoteDestinoCriado = loteDestino;

            }


            return loteDestino;
        }

        private List<MovimentacaoAnimal> CriaObjetoMovimentacaoAnimal(MovimentacaoAnimalCadastro entity,
                                List<Animal> animaisTransferidos,
                                LoteEntrada loteDestino)
        {
            List<MovimentacaoAnimal> movimentacoes = new List<MovimentacaoAnimal>();

            foreach (var animal in animaisTransferidos)
            {
                movimentacoes.Add(new MovimentacaoAnimal
                {
                    DataMovimentacao = entity.DataMovimentacao,
                    IdAnimal = animal.Id,
                    IdLocalDestino = entity.LocalDestino.Id,
                    IdLocalOrigem = entity.LocalOrigem.Id,
                    IdLoteOrigem = entity.LoteOrigem.Id,
                    IdLoteDestino = loteDestino.Id,
                    IdMotivo = entity.Motivo.Id,
                    Status = Status.Ativado
                });
            }

            if (movimentacoes.Count == 0)
                Notificar("Nenhum Animal foi registrado na movimentação");

            return movimentacoes;
        }

        public async Task Adicionar(MovimentacaoAnimalCadastro entity)
        {

            //Obtem Lote Origem
            var loteOrigem = await _loteEntradaRepositorio.ObterPorId(entity.LoteOrigem.Id);
            if (loteOrigem is null)
            {
                Notificar("Lote de Origem não existe");
                return;
            }

            //Valida os dados do Lote de Origem
            if (!ValidarDadosDoLoteOrigem(entity, loteOrigem)) return;

            //Valida Dados da Movimentação
            if (!ValidarDataMovimentacao(entity, loteOrigem)) return;

            //Cria ou Obtém Objeto do Lote de Entrada Destino
            LoteEntrada loteDestino = await CriaOuObtemLoteDestino(entity);
            if (loteDestino is null) return;

            //Transfere os animais do lote origem para o lote destino e retorna esses animais que foram transferidos
            var animaisTransferidos = loteOrigem.TransferirAnimais(loteDestino, entity.QuantidadeAnimais);

            //Grava as informações do Lote Destino no banco de dados
            await _loteEntradaRepositorio.RealizarTransferencia(loteDestino, animaisTransferidos);

            //Atualizando dados dos locais de origem e destino
            if (!await AtualizaDadosLocalOrigemAndDestino(entity))
            {
                await RollbackLoteDestino();
                return;
            }

            //Criar Objeto com os animais
            var movimentacoes = CriaObjetoMovimentacaoAnimal(entity, animaisTransferidos, loteDestino);
            if (movimentacoes.Count == 0)
            {
                await RollbackLoteDestino();
                return;
            }

            //Validar cliente no loop e Validar Campos no loop
            foreach (var movimentacao in movimentacoes)
            {
                ValidaInsercaoAtualizacaoCliente(movimentacao);
                if (!ExecutarValidacao(new MovimentacaoAnimalValidation(TipoOperacao.Inclusao,
                    loteDestino.Id == 0 ? TipoOperacao.Inclusao : TipoOperacao.Atualizacao), movimentacao)) break;

                await _movimentacaoAnimalRepositorio.Adicionar(movimentacao);
            }

            if (!TemNotificacao())
                await _movimentacaoAnimalRepositorio.UnitOfWork.Commit();
            else
                await RollbackLoteDestino();
        }

        public async Task RollbackLoteDestino()
        {
            if (LoteDestinoCriado != null && LoteDestinoCriado.Id > 0)
            {
                await _loteEntradaRepositorio.Remover(LoteDestinoCriado);
                await _loteEntradaRepositorio.UnitOfWork.Commit();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public async Task<MovimentacaoAnimalDTO> ObterMovimentacao(int idLocalOrigem,
                                        int idLocalDestino,
                                        int idLoteOrigem,
                                        int idLoteDestino,
                                        DateTime dataMovimentacao)
        {
            return await _movimentacaoAnimalRepositorio.ObterMovimentacao(idLocalOrigem,
                                                                          idLocalDestino,
                                                                          idLoteOrigem,
                                                                          idLoteDestino,
                                                                          dataMovimentacao);
        }

        public async Task<List<MovimentacaoAnimalDTO>> ObterPaginacao()
        {
            return await _movimentacaoAnimalRepositorio.ObterPaginacao();
        }

        public async Task Remover(int idLocalOrigem, int idLocalDestino, int idLoteOrigem, int idLoteDestino, DateTime dataMovimentacao)
        {

            var model = await _movimentacaoAnimalRepositorio.ObterMovimentacao(idLocalOrigem,
                                                                          idLocalDestino,
                                                                          idLoteOrigem,
                                                                          idLoteDestino,
                                                                          dataMovimentacao);

            if (model is null)
            {
                Notificar("Movimentação de Animal não existe");
                return;
            }

            var animaisMovimentacao = await _movimentacaoAnimalRepositorio.ObterAnimaisMovimentacao(idLocalOrigem,
                                                                          idLocalDestino,
                                                                          idLoteOrigem,
                                                                          idLoteDestino,
                                                                          dataMovimentacao);

            var loteOrigem = await _loteEntradaRepositorio.ObterPorId(model.IdLoteOrigem);
            if (loteOrigem is null)
            {
                Notificar("Lote de Origem não foi encontrado no banco de dados");
                return;
            }

            var loteDestino = await _loteEntradaRepositorio.ObterPorId(model.IdLoteDestino);
            if (loteDestino is null)
            {
                Notificar("Lote de Destino não foi encontrado no banco de dados");
                return;
            }

            var animaisTransferidos = loteDestino.TransferirAnimais(loteOrigem, model.QuantidadeAnimais);

            await _loteEntradaRepositorio.RealizarTransferencia(loteOrigem, animaisTransferidos);

            if (!await ReverteDadosLocalOrigemAndDestino(model)) return;

            foreach (var animal in animaisMovimentacao)            
                await _movimentacaoAnimalRepositorio.Remover(animal);

            await _movimentacaoAnimalRepositorio.UnitOfWork.Commit();

        }

        private async Task<bool> AtualizaDadosLocalOrigemAndDestino(MovimentacaoAnimalCadastro entity)
        {
            var localDestino = await _pastoCurralRepositorio.ObterPorId(entity.LocalDestino.Id);

            if (localDestino is null)
            {
                Notificar("Local Destino não existe no banco de dados");
                return false;
            }

            //if (entity.LoteDestino.Id > 0)
            //{
            if (!await _pastoCurralRepositorio.ExisteLoteAtivo(localDestino.Id))
            {
                Notificar($"Local Destino({localDestino.Nome}) não possui lote ativo. Não é possível efetuar o lançamento.");
                return false;
            }
            //}

            var localOrigem = await _pastoCurralRepositorio.ObterPorId(entity.LocalOrigem.Id);

            if (localOrigem is null)
            {
                Notificar("Local Origem não existe no banco de dados");
                return false;
            }

            if (!await _pastoCurralRepositorio.ExisteLoteAtivo(localOrigem.Id))
            {
                Notificar($"Local de Origem({localOrigem.Nome}) não possui lote ativo. Não é possível efetuar o lançamento.");
                return false;
            }

            localDestino.IncluirAnimais(entity.QuantidadeAnimais);
            localOrigem.RetirarAnimais(entity.QuantidadeAnimais);

            await _pastoCurralRepositorio.Atualizar(localDestino);

            await _pastoCurralRepositorio.Atualizar(localOrigem);

            return true;
        }

        private async Task<bool> ReverteDadosLocalOrigemAndDestino(MovimentacaoAnimalDTO entity)
        {
            var localDestino = await _pastoCurralRepositorio.ObterPorId(entity.IdLocalDestino);

            if (localDestino is null)
            {
                Notificar($"Lote de Destino({entity.LocalDestino}) foi excluído. Não é possível fazer a exclusão da Movimentação");
                return false;
            }

            var localOrigem = await _pastoCurralRepositorio.ObterPorId(entity.IdLocalOrigem);

            if (localOrigem is null)
            {
                Notificar($"Lote de Origem({entity.LocalOrigem}) foi excluído. Não é possível fazer a exclusão da Movimentação");
                return false;
            }

            localOrigem.IncluirAnimais(entity.QuantidadeAnimais);
            localDestino.RetirarAnimais(entity.QuantidadeAnimais);

            await _pastoCurralRepositorio.Atualizar(localOrigem);

            await _pastoCurralRepositorio.Atualizar(localDestino);

            return true;
        }
    }
}
