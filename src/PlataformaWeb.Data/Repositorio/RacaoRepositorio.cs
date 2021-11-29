using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PlataformaWeb.Data.Repositorio
{
    public class RacaoRepositorio : Repositorio<Racao>, IRacaoRepositorio
    {
        public RacaoRepositorio(PlataformaFieldContext context, 
                                IUser appUser) : base(context, appUser)
        {

        }

        public override Expression<Func<Racao, bool>> ObterWhere()
        {
            var where = base.ObterWhere();

            //if (!AppUser.EhAdmin())
                where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());

            return where;
        }

        public async override Task<Racao> ObterPorId(int id)
        {
            var racao = await DbSet.AsNoTracking()                              
                              .Where(ObterWhere().And(x => x.Id == id))
                              .FirstOrDefaultAsync();

            //necessário desmebrar a busca dos insumos para filtrar somente os insumos ativados.
            if (racao != null)
                racao.InsumosRacao = await ObterInsumos(racao.Id);

            return racao;
        }

        private async Task<List<RacaoInsumo>> ObterInsumos(int idRacao)
        {
            return await Context.RacaoInsumo.AsNoTracking()
                                .Include(x => x.InsumoAlimento)
                                .Where(x => x.Status == Business.Enums.Status.Ativado && x.IdRacao == idRacao)
                                .ToListAsync();
        }


        public async Task<List<RacaoDTO>> ObterPaginacao()
        {
            return await DbSet.AsNoTracking()
                               .Include(x => x.Cliente)
                                    .ThenInclude(c => c.Tecnico)
                              .Where(ObterWhere())
                              .Select(x => new RacaoDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  Tipo = x.Tipo,
                                  Gmd = x.Gmd,
                                  MateriaSeca = x.MateriaSeca,
                                  ValorKg = x.ValorKg,
                                  DataFormulacao = x.DataFormulacao,
                                  Proprietario = x.Cliente.Nome,
                                  NomePropriedade = x.Cliente.NomePropriedade,
                                  Tecnico = x.Cliente.Tecnico.Nome,
                              }).ToListAsync();
        }

        public async Task<RacaoInsumo> ObterRacaoInsumo(int idRacaoInsumo)
        {
            var where = PredicateBuilder.True<RacaoInsumo>()
                                .And(x => x.Status == Business.Enums.Status.Ativado)
                                .And(x => x.Id == idRacaoInsumo)
                                .And(x => x.IdCliente == AppUser.ObterIdCliente());

            return await Context.RacaoInsumo.Where(where).FirstOrDefaultAsync();
        }

        public async override Task Atualizar(Racao entity)
        {
            await Task.FromResult(Context.Racao.Update(entity));

            foreach (var insumo in entity.InsumosRacao)
            {
                if (insumo.Id == 0)
                    Context.Entry(insumo).State = EntityState.Added;

                if (insumo.InsumoAlimento != null)
                    Context.Entry(insumo.InsumoAlimento).State = EntityState.Unchanged;

                if (insumo?.InsumoAlimento?.FornecedorInsumo != null)
                    Context.Entry(insumo.InsumoAlimento.FornecedorInsumo).State = EntityState.Unchanged;
            }

            await RemoverInsumosForaDaLista(entity.InsumosRacao.Where(x => x.Id > 0).Select(x => x.Id).ToArray(), entity.Id);

        }

        public async Task RemoverInsumosForaDaLista(int[] ids, int idRacao)
        {
            var where = PredicateBuilder.True<RacaoInsumo>()
                                .And(x => x.IdCliente == AppUser.ObterIdCliente())
                                .And(x => x.Status == Business.Enums.Status.Ativado)
                                .And(x => x.IdRacao == idRacao);

            if (ids.Length > 0)
                where = where.And(x => !ids.Contains(x.Id));

            var insumosParaRemover = await Context.RacaoInsumo.Where(where).ToListAsync();

            foreach (var insumo in insumosParaRemover)
            {
                insumo.Status = Business.Enums.Status.Desativado;
                Context.RacaoInsumo.Update(insumo);
            }

        }

        public async override Task Adicionar(Racao entity)
        {
            foreach (var insumo in entity.InsumosRacao)
            {
                Context.Entry(insumo.InsumoAlimento).State = EntityState.Unchanged;
                if (insumo.InsumoAlimento.FornecedorInsumo != null)
                    Context.Entry(insumo.InsumoAlimento.FornecedorInsumo).State = EntityState.Unchanged;
            }

            await Task.FromResult(Context.Racao.Add(entity));
        }

        public async Task RemoverInsumo(RacaoInsumo racaoInsumo)
        {
            racaoInsumo.Status = Business.Enums.Status.Desativado;
            await Task.FromResult(Context.RacaoInsumo.Update(racaoInsumo));
        }

        public async Task<List<RacaoDTO>> BuscarQuery(Expression<Func<Racao, bool>> predicate)
        {
            return await DbSet.AsNoTracking()                               
                              .Where(ObterWhere().And(predicate))
                              .Select(x => new RacaoDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  Tipo = x.Tipo,
                                  Gmd = x.Gmd,
                                  MateriaSeca = x.MateriaSeca,
                                  ValorKg = x.ValorKg,
                                  DataFormulacao = x.DataFormulacao                                  
                              }).ToListAsync();
        }

        public async Task<List<Racao>> ObterRacoesContemInsumo(int idInsumo)
        {
            //obtém ids de rações que contém insumo
            var idsRacao = await Context.RacaoInsumo.AsNoTracking()
                                    .Where(x => x.IdInsumoAlimento == idInsumo && x.IdCliente == AppUser.ObterIdCliente()
                                                && x.Status == Business.Enums.Status.Ativado)
                                    .Select(x => x.IdRacao)
                                    .ToListAsync();

            //otém list de objetos de ração
            var racoes = await DbSet.AsNoTracking()
                              .Where(ObterWhere().And(x => idsRacao.Contains(x.Id)))
                              .ToListAsync();

            //busca os insumos da ração
            foreach (var racao in racoes)
            {
                racao.InsumosRacao = await Context.RacaoInsumo.AsNoTracking()
                                                .Where(x => x.IdRacao == racao.Id && x.Status == Business.Enums.Status.Ativado)
                                                .ToListAsync();
            }

            return racoes;
        }
    }
}
