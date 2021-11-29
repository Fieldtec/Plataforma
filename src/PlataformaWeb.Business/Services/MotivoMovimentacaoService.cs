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
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class MotivoMovimentacaoService : Service, IMotivoMovimentacaoService
    {
        private readonly IMotivoMovimentacaoRepositorio _motivoMovimentacaoRepositorio;

        public MotivoMovimentacaoService(INotificador notificador,
                                IUser appUser,
                                IMotivoMovimentacaoRepositorio motivoMovimentacaoRepositorio) : base(notificador, appUser)
        {
            _motivoMovimentacaoRepositorio = motivoMovimentacaoRepositorio;
        }

        public async Task Adicionar(MotivoMovimentacao entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new MotivoMovimentacaoValidation(TipoOperacao.Inclusao), entity)) return;

            await _motivoMovimentacaoRepositorio.Adicionar(entity);

            await _motivoMovimentacaoRepositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(MotivoMovimentacao entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new MotivoMovimentacaoValidation(TipoOperacao.Atualizacao), entity)) return;

            await _motivoMovimentacaoRepositorio.Atualizar(entity);

            await _motivoMovimentacaoRepositorio.UnitOfWork.Commit();

        }

        public async Task<IEnumerable<MotivoMovimentacaoDTO>> Buscar(Expression<Func<MotivoMovimentacao, bool>> predicate)
        {
            return await _motivoMovimentacaoRepositorio.BuscarQuery(predicate);
        }

        public async Task<MotivoMovimentacao> ObterPorId(int id)
        {
            return await _motivoMovimentacaoRepositorio.ObterPorId(id);
        }

        public async Task<List<MotivoMovimentacaoDTO>> ObterPaginacao(int? id = null)
        {
            return await _motivoMovimentacaoRepositorio.ObterPaginacao();
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id do Motivo da Movimentação é inválido");
                return;
            }

            var model = await _motivoMovimentacaoRepositorio.ObterPorId(id);

            if (model is null)
            {
                Notificar("Motivo da Movimentação não existe");
                return;
            }

            await _motivoMovimentacaoRepositorio.Remover(model);

            await _motivoMovimentacaoRepositorio.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }

}
