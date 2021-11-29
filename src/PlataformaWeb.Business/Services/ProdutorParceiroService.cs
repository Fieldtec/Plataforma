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
    public class ProdutorParceiroService : Service, IProdutorParceiroService
    {
        private readonly IProdutorParceiroRepositorio _produtorParceiroRepositorio;
        public ProdutorParceiroService(INotificador notificador, 
                                       IUser appUser, 
                                       IProdutorParceiroRepositorio produtorParceiroRepositorio) : base(notificador, appUser)
        {
            _produtorParceiroRepositorio = produtorParceiroRepositorio;
        }

        public async Task Adicionar(ProdutorParceiro entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new ProdutorParceiroValidation(TipoOperacao.Inclusao), entity)) return;

            await _produtorParceiroRepositorio.Adicionar(entity);

            await _produtorParceiroRepositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(ProdutorParceiro entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new ProdutorParceiroValidation(TipoOperacao.Atualizacao), entity)) return;

            await _produtorParceiroRepositorio.Atualizar(entity);

            await _produtorParceiroRepositorio.UnitOfWork.Commit();

        }

        public async Task<List<ProdutorParceiroDTO>> Buscar(Expression<Func<ProdutorParceiro, bool>> predicate)
        {
            return await _produtorParceiroRepositorio.BuscarQuery(predicate);
        }   

        public async Task<ProdutorParceiro> ObterPorId(int id)
        {
            return await _produtorParceiroRepositorio.ObterPorId(id);
        }

        public async Task<List<ProdutorParceiroDTO>> ObterPaginacao(int? id = null)
        {
            return await _produtorParceiroRepositorio.ObterPaginacao(id);
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id do Produtor Parceiro é inválido");
                return;
            }

            var produtorParceiro = await _produtorParceiroRepositorio.ObterPorId(id);

            if (produtorParceiro is null)
            {
                Notificar("Produtor Parceiro não existe");
                return;
            }

            await _produtorParceiroRepositorio.Remover(produtorParceiro);

            await _produtorParceiroRepositorio.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
