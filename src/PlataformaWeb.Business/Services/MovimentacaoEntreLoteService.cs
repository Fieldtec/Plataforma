using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Models.Validations;
using PlataformaWeb.Business.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class MovimentacaoEntreLoteService : Service, IMovimentacaoEntreLoteService
    {
        private readonly IMovimentacaoEntreLoteRepositorio _movimentacaoEntreLoteRepositorio;
        private readonly ILoteEntradaRepositorio _loteEntradaRepositorio;
        private readonly IPastoCurralRepositorio _pastoCurralRepositorio;

        public MovimentacaoEntreLoteService(INotificador notificador,
                                            IUser appUser,
                                            IMovimentacaoEntreLoteRepositorio movimentacaoEntreLoteRepositorio,
                                            ILoteEntradaRepositorio loteEntradaRepositorio, 
                                            IPastoCurralRepositorio pastoCurralRepositorio) : base(notificador, appUser)
        {
            _movimentacaoEntreLoteRepositorio = movimentacaoEntreLoteRepositorio;
            _loteEntradaRepositorio = loteEntradaRepositorio;
            _pastoCurralRepositorio = pastoCurralRepositorio;
        }

        public async Task Adicionar(MovimentacaoEntreLote entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new MovimentacaoEntreLoteValidation(TipoOperacao.Inclusao), entity)) return;

            if (!await AtualizaDadosDoLote(entity)) return;

            if (!await AtualizaDadosLocalOrigemAndDestino(entity)) return;

            await _movimentacaoEntreLoteRepositorio.Adicionar(new MovimentacaoEntreLote(entity));

            await _movimentacaoEntreLoteRepositorio.UnitOfWork.Commit();
        }

        public Task Atualizar(MovimentacaoEntreLote entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MovimentacaoEntreLoteDTO>> Buscar(Expression<Func<MovimentacaoEntreLote, bool>> predicate)
        {
            return await _movimentacaoEntreLoteRepositorio.BuscarQuery(predicate);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public async Task<List<MovimentacaoEntreLoteDTO>> ObterPaginacao(int? id = null)
        {
            return await _movimentacaoEntreLoteRepositorio.ObterPaginacao();
        }

        public async Task<MovimentacaoEntreLote> ObterPorId(int id)
        {
            return await _movimentacaoEntreLoteRepositorio.ObterPorId(id);
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id do Movimentação entre Lote é inválido");
                return;
            }

            var models = await _movimentacaoEntreLoteRepositorio.Buscar(x => x.Id == id);
            var model = models.FirstOrDefault();

            if (model is null)
            {
                Notificar("Movimentação entre Lote não existe");
                return;
            }

            if (!await ReverteDadosLocalOrigemAndDestino(model)) return;

            if (!await ReverteDadosDoLote(model)) return;

            await _movimentacaoEntreLoteRepositorio.Remover(model);

            await _movimentacaoEntreLoteRepositorio.UnitOfWork.Commit();

        }

        private async Task<bool> AtualizaDadosDoLote(MovimentacaoEntreLote entity)
        {
            var lote = await _loteEntradaRepositorio.ObterPorId(entity.IdLoteEntrada);

            if (lote is null)
            {
                Notificar("Lote de Entrada não existe no banco de dados");
                return false;
            }

            var menorDataDeEntrada = lote.AnimaisLote.Min(x => x.DataEntrada);

            if (entity.DataMovimentacao.CompareTo(menorDataDeEntrada) < 0)
            {
                Notificar($"Data da Movimentação({entity.DataMovimentacao.ToShortDateString()}) é menor que a Data da inserção do animal no Lote({menorDataDeEntrada.ToShortDateString()})");
                return false;
            }

            if (entity.QuantidadeAnimais != lote.AnimaisLote.Count)
            {
                Notificar($"Quantidade de animais divergem entre os dados da Movimentação({entity.QuantidadeAnimais}) e os dados do Lote({lote.AnimaisLote.Count})");
                return false;
            }

            if (entity.IdLocalOrigem != lote.IdLocal)
            {
                Notificar($"Local de Origem({entity.LocalOrigem.Nome}) não o mesmo Local registrado no Lote.");
                return false;
            }

            lote.IdLocal = entity.IdLocalDestino;

            //zera lista de animais para não atualiza-los no banco de dados sem precisar
            lote.AnimaisLote = new List<Animal>();

            await _loteEntradaRepositorio.Atualizar(lote);

            return true;
        }

        private async Task<bool> AtualizaDadosLocalOrigemAndDestino(MovimentacaoEntreLote entity)
        {
            var localDestino = await _pastoCurralRepositorio.ObterPorId(entity.IdLocalDestino);

            if (localDestino is null)
            {
                Notificar("Local Destino não existe no banco de dados");
                return false;
            }

            if (localDestino.Lotacao.HasValue && localDestino.Lotacao.Value > 0)
            {
                if (await _pastoCurralRepositorio.ExisteLoteAtivo(localDestino.Id))
                {
                    Notificar($"Local Destino(${localDestino.Nome}) possui lote ativo. Não é possível efetuar o lançamento.");
                    return false;
                }
            }

            var localOrigem = await _pastoCurralRepositorio.ObterPorId(entity.IdLocalOrigem);

            if (localOrigem is null)
            {
                Notificar("Local Origem não existe no banco de dados");
                return false;
            }

            localDestino.IncluirAnimais(entity.QuantidadeAnimais);
            localOrigem.RetirarAnimais(entity.QuantidadeAnimais);

            await _pastoCurralRepositorio.Atualizar(localDestino);

            await _pastoCurralRepositorio.Atualizar(localOrigem);

            return true;
        }

        private async Task<bool> ReverteDadosDoLote(MovimentacaoEntreLote entity)
        {
            var lote = await _loteEntradaRepositorio.ObterPorId(entity.IdLoteEntrada);

            if (lote is null)
            {
                Notificar("Lote de Entrada não existe no banco de dados");
                return false;
            }

            lote.IdLocal = entity.IdLocalOrigem;
            lote.AnimaisLote = new List<Animal>();

            await _loteEntradaRepositorio.Atualizar(lote);

            return true;
        }

        private async Task<bool> ReverteDadosLocalOrigemAndDestino(MovimentacaoEntreLote entity)
        {
            var localDestino = await _pastoCurralRepositorio.ObterPorId(entity.IdLocalDestino);

            if (localDestino is null)
            {
                Notificar($"Lote de Destino({entity.LocalDestino.Nome}) foi excluído. Não é possível fazer a exclusão da Movimentação");
                return false;
            }

            var localOrigem = await _pastoCurralRepositorio.ObterPorId(entity.IdLocalOrigem);

            if (localOrigem is null)
            {
                Notificar($"Lote de Origem({entity.LocalOrigem.Nome}) foi excluído. Não é possível fazer a exclusão da Movimentação");
                return false;
            }

            if (await _pastoCurralRepositorio.ExisteLoteAtivo(localOrigem.Id))
            {
                Notificar($"Lote de Origem({entity.LocalOrigem.Nome}) possui um lote Ativo. Não é possível fazer a exclusão da Movimentação.");
                return false;
            }

            localDestino.Lotacao -= entity.QuantidadeAnimais;
            if (localDestino.Lotacao < 0) localDestino.Lotacao = 0;

            if (!localOrigem.Lotacao.HasValue) localOrigem.Lotacao = 0;
            localOrigem.Lotacao += entity.QuantidadeAnimais;
            
            await _pastoCurralRepositorio.Atualizar(localOrigem);

            await _pastoCurralRepositorio.Atualizar(localDestino);            

            return true;
        }

    }

}
