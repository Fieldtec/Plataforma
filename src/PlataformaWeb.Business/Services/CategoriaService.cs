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
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class CategoriaService : Service, ICategoriaService
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        public CategoriaService(INotificador notificador,
                                IUser appUser,
                                ICategoriaRepositorio categoriaRepositorio) : base(notificador, appUser)
        {
            _categoriaRepositorio = categoriaRepositorio;
        }

        public async Task Adicionar(Categoria entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new CategoriaValidation(TipoOperacao.Inclusao), entity)) return;

            await _categoriaRepositorio.Adicionar(entity);

            await _categoriaRepositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(Categoria entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new CategoriaValidation(TipoOperacao.Atualizacao), entity)) return;

            await _categoriaRepositorio.Atualizar(entity);

            await _categoriaRepositorio.UnitOfWork.Commit();

        }

        public async Task<IEnumerable<CategoriaDTO>> Buscar(Expression<Func<Categoria, bool>> predicate)
        {
            return await _categoriaRepositorio.BuscarQuery(predicate);
        }

        public async Task<Categoria> ObterPorId(int id)
        {
            return await _categoriaRepositorio.ObterPorId(id);
        }

        public async Task<List<CategoriaDTO>> ObterPaginacao(int? id = null)
        {
            return await _categoriaRepositorio.ObterPaginacao(id);
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id da Categoria é inválida");
                return;
            }

            var categoria = await _categoriaRepositorio.ObterPorId(id);

            if (categoria is null)
            {
                Notificar("Categoria não existe");
                return;
            }

            await _categoriaRepositorio.Remover(categoria);

            await _categoriaRepositorio.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
