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
    public class FrigorificoService : Service, IFrigorificoService
    {
        private readonly IFrigorificoRepositorio _frigorificoRepositorio;

        public FrigorificoService(INotificador notificador,
                                IUser appUser,
                                IFrigorificoRepositorio frigorificoRepositorio) : base(notificador, appUser)
        {
            _frigorificoRepositorio = frigorificoRepositorio;
        }

        public async Task Adicionar(Frigorifico entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new FrigorificoValidation(TipoOperacao.Inclusao), entity)) return;

            await _frigorificoRepositorio.Adicionar(entity);

            await _frigorificoRepositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(Frigorifico entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new FrigorificoValidation(TipoOperacao.Atualizacao), entity)) return;

            await _frigorificoRepositorio.Atualizar(entity);

            await _frigorificoRepositorio.UnitOfWork.Commit();

        }

        public async Task<IEnumerable<FrigorificoDTO>> Buscar(Expression<Func<Frigorifico, bool>> predicate)
        {
            return await _frigorificoRepositorio.BuscarQuery(predicate);
        }

        public async Task<Frigorifico> ObterPorId(int id)
        {
            return await _frigorificoRepositorio.ObterPorId(id);
        }

        public async Task<List<FrigorificoDTO>> ObterPaginacao(int? id = null)
        {
            return await _frigorificoRepositorio.ObterPaginacao();
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id do Frigorífico é inválido");
                return;
            }

            var model = await _frigorificoRepositorio.ObterPorId(id);

            if (model is null)
            {
                Notificar("Frigorífico não existe");
                return;
            }

            await _frigorificoRepositorio.Remover(model);

            await _frigorificoRepositorio.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }

}
