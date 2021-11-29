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
    public class CausaMorteService : Service, ICausaMorteService
    {
        private readonly ICausaMorteRepositorio _causaMorteRepositorio;

        public CausaMorteService(INotificador notificador,
                                IUser appUser,
                                ICausaMorteRepositorio causaMorteRepositorio) : base(notificador, appUser)
        {
            _causaMorteRepositorio = causaMorteRepositorio;
        }

        public async Task Adicionar(CausaMorte entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new CausaMorteValidation(TipoOperacao.Inclusao), entity)) return;

            await _causaMorteRepositorio.Adicionar(entity);

            await _causaMorteRepositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(CausaMorte entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new CausaMorteValidation(TipoOperacao.Atualizacao), entity)) return;

            await _causaMorteRepositorio.Atualizar(entity);

            await _causaMorteRepositorio.UnitOfWork.Commit();

        }

        public async Task<IEnumerable<CausaMorteDTO>> Buscar(Expression<Func<CausaMorte, bool>> predicate)
        {
            return await _causaMorteRepositorio.BuscarQuery(predicate);
        }

        public async Task<CausaMorte> ObterPorId(int id)
        {
            return await _causaMorteRepositorio.ObterPorId(id);
        }

        public async Task<List<CausaMorteDTO>> ObterPaginacao(int? id = null)
        {
            return await _causaMorteRepositorio.ObterPaginacao();
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id da Causa da Morte é inválida");
                return;
            }

            var model = await _causaMorteRepositorio.ObterPorId(id);

            if (model is null)
            {
                Notificar("Causa da Morte não existe");
                return;
            }

            await _causaMorteRepositorio.Remover(model);

            await _causaMorteRepositorio.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }

}
