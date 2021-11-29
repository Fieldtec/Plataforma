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
    public class SuplementoMineralService : Service, ISuplementoMineralService
    {
        private readonly ISuplementoMineralRepositorio _suplementoMineralRepositorio;
        private readonly IFornecedorInsumoRepositorio _fornecedorInsumoRepositorio;

        public SuplementoMineralService(INotificador notificador,
                                        IUser appUser,
                                        ISuplementoMineralRepositorio suplementoMineralRepositorio, 
                                        IFornecedorInsumoRepositorio fornecedorInsumoRepositorio) : base(notificador, appUser)
        {
            _suplementoMineralRepositorio = suplementoMineralRepositorio;
            _fornecedorInsumoRepositorio = fornecedorInsumoRepositorio;
        }

        public async Task Adicionar(SuplementoMineral entity)
        {

            if (!ExecutarValidacao(new SuplementoMineralValidation(TipoOperacao.Inclusao), entity)) return;

            if (!await ValidaInsercaoSuplemento(entity)) return;

            await _suplementoMineralRepositorio.Adicionar(entity);

            await _suplementoMineralRepositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(SuplementoMineral entity)
        {

            if (!ExecutarValidacao(new SuplementoMineralValidation(TipoOperacao.Atualizacao), entity)) return;

            if (!await ValidaInsercaoSuplemento(entity)) return;

            await _suplementoMineralRepositorio.Atualizar(entity);

            await _suplementoMineralRepositorio.UnitOfWork.Commit();

        }

        public async Task<IEnumerable<SuplementoMineralDTO>> Buscar(Expression<Func<SuplementoMineral, bool>> predicate)
        {
            return await _suplementoMineralRepositorio.BuscarQuery(predicate);
        }

        public async Task<SuplementoMineral> ObterPorId(int id)
        {
            return await _suplementoMineralRepositorio.ObterPorId(id);
        }

        public async Task<List<SuplementoMineralDTO>> ObterPaginacao(int? id = null)
        {
            return await _suplementoMineralRepositorio.ObterPaginacao();
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id do Suplemento é inválido");
                return;
            }

            var suplementoMineral = await _suplementoMineralRepositorio.ObterPorId(id);

            if (suplementoMineral is null)
            {
                Notificar("Suplemento não existe");
                return;
            }

            await _suplementoMineralRepositorio.Remover(suplementoMineral);

            await _suplementoMineralRepositorio.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        private async Task<bool> ValidaInsercaoSuplemento(SuplementoMineral entity)
        {
            var fornecedorInsumo = await _fornecedorInsumoRepositorio.ObterPorId(entity.IdFornecedor);

            return ValidaInsercaoAtualizacaoCliente(fornecedorInsumo);

            //if (fornecedorInsumo is null || fornecedorInsumo.IdCliente != AppUser.ObterIdCliente())
            //{
            //    Notificar("Fornecedor de Insumo não pertence ao Cliente");
            //    return false;
            //}

            //return true;
        }

    }
}
