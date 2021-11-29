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
    public class FornecedorInsumoService : Service, IFornecedorInsumoService
    {
        private readonly IFornecedorInsumoRepositorio _fornecedorInsumoRepositorio;
        public FornecedorInsumoService(INotificador notificador, 
                                       IUser appUser, 
                                       IFornecedorInsumoRepositorio fornecedorInsumoRepositorio) : base(notificador, appUser)
        {
            _fornecedorInsumoRepositorio = fornecedorInsumoRepositorio;
        }

        public async Task Adicionar(FornecedorInsumo entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new FornecedorInsumoValidation(TipoOperacao.Inclusao), entity)) return;

            await _fornecedorInsumoRepositorio.Adicionar(entity);

            await _fornecedorInsumoRepositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(FornecedorInsumo entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new FornecedorInsumoValidation(TipoOperacao.Atualizacao), entity)) return;

            await _fornecedorInsumoRepositorio.Atualizar(entity);

            await _fornecedorInsumoRepositorio.UnitOfWork.Commit();

        }

        public async Task<IEnumerable<FornecedorInsumo>> Buscar(Expression<Func<FornecedorInsumo, bool>> predicate)
        {
            return await _fornecedorInsumoRepositorio.Buscar(predicate);
        }

        public async Task<FornecedorInsumo> ObterPorId(int id)
        {
            return await _fornecedorInsumoRepositorio.ObterPorId(id);
        }

        public async Task<List<FornecedorInsumoDTO>> ObterPaginacao(int? id = null)
        {
            return await _fornecedorInsumoRepositorio.ObterPaginacao();
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id do Fornecedor de Insumo é inválido");
                return;
            }

            var fornecedorInsumo = await _fornecedorInsumoRepositorio.ObterPorId(id);

            if (fornecedorInsumo is null)
            {
                Notificar("Fornecedor de Insumo não existe");
                return;
            }

            await _fornecedorInsumoRepositorio.Remover(fornecedorInsumo);

            await _fornecedorInsumoRepositorio.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
