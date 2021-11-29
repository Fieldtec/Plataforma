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
using PlataformaWeb.Business.Enums;
using FluentValidation.Results;
using Dapper;
using System.Security.Cryptography.X509Certificates;

namespace PlataformaWeb.Data.Repositorio
{
    public class PlanejamentoNutricionalRepositorio : Repositorio<PlanejamentoNutricional>, IPlanejamentoNutricionalRepositorio
    {
        public PlanejamentoNutricionalRepositorio(PlataformaFieldContext context,
                                                 IUser appUser) : base(context, appUser)
        {

        }

        private void RetirarRelatedItemsDoContexto(PlanejamentoNutricional entity, TipoOperacao operacao)
        {
            foreach (var valor in entity.PlanejamentoValoresConfinamento)
            {
                if (valor.Id == 0 && operacao == TipoOperacao.Atualizacao)
                    Context.Entry(valor).State = EntityState.Added;

                Context.Entry(valor.Racao).State = EntityState.Unchanged;
            }

            foreach (var valor in entity.PlanejamentoValoresPasto)
            {
                if (valor.Id == 0 && operacao == TipoOperacao.Atualizacao)
                    Context.Entry(valor).State = EntityState.Added;

                if (!base.ExisteNoChangeTracker(valor.SuplementoMineral))
                    Context.Entry(valor.SuplementoMineral).State = EntityState.Unchanged;

                if (!base.ExisteNoChangeTracker(valor.FaseDoAno))
                    Context.Entry(valor.FaseDoAno).State = EntityState.Unchanged;

                if (!base.ExisteNoChangeTracker(valor.Categoria))
                    Context.Entry(valor.Categoria).State = EntityState.Unchanged;

            }
        }

        private async Task RemoverValoresConfinamentoForaDaLista(int idPlanejamento, int[] ids)
        {
            var where = PredicateBuilder.True<PlanejamentoValoresConfinamento>()
                               .And(x => x.Status == Business.Enums.Status.Ativado)
                               .And(x => x.IdPlanejamento == idPlanejamento);

            if (ids.Length > 0)
                where = where.And(x => !ids.Contains(x.Id));

            var valoresConfinamento = await Context.PlanejamentosValoresConfinamento.Where(where).ToListAsync();

            foreach (var valor in valoresConfinamento)
            {
                valor.Status = Business.Enums.Status.Desativado;
                Context.PlanejamentosValoresConfinamento.Update(valor);
            }
        }

        private async Task RemoverValoresPastoForaDaLista(int idPlanejamento, int[] ids)
        {
            var where = PredicateBuilder.True<PlanejamentoValoresPasto>()
                               .And(x => x.Status == Business.Enums.Status.Ativado)
                               .And(x => x.IdPlanejamento == idPlanejamento);

            if (ids.Length > 0)
                where = where.And(x => !ids.Contains(x.Id));

            var valoresPasto = await Context.PlanejamentosValoresPasto.Where(where).ToListAsync();

            foreach (var valor in valoresPasto)
            {
                valor.Status = Business.Enums.Status.Desativado;
                Context.PlanejamentosValoresPasto.Update(valor);
            }
        }

        public async override Task Adicionar(PlanejamentoNutricional entity)
        {
            //RetirarRelatedItemsDoContexto(entity, TipoOperacao.Inclusao);

            await base.Adicionar(entity);

        }

        public async override Task Atualizar(PlanejamentoNutricional entity)
        {
            //RetirarRelatedItemsDoContexto(entity, TipoOperacao.Atualizacao);

            await base.Atualizar(entity);

            //if (entity.Tipo == TipoPlanejamentoNutricional.Confinamento)
            await RemoverValoresConfinamentoForaDaLista(entity.Id, entity.PlanejamentoValoresConfinamento
                .Where(x => x.Id > 0).Select(x => x.Id).ToArray());
            //else
            await RemoverValoresPastoForaDaLista(entity.Id, entity.PlanejamentoValoresPasto
                .Where(x => x.Id > 0).Select(x => x.Id).ToArray());
        }


        public override Expression<Func<PlanejamentoNutricional, bool>> ObterWhere()
        {
            var where = base.ObterWhere();

            //if (!AppUser.EhAdmin())
                where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());

            return where;
        }

        public async override Task<PlanejamentoNutricional> ObterPorId(int id)
        {
            var planejamentoNutricional = await DbSet.AsNoTracking()
                              .Where(ObterWhere().And(x => x.Id == id))
                              .FirstOrDefaultAsync();

            if (planejamentoNutricional != null)
            {
                if (planejamentoNutricional.Tipo == Business.Enums.TipoPlanejamentoNutricional.Confinamento)
                    planejamentoNutricional.PlanejamentoValoresConfinamento = await ObterPlanejamentoValoresConfinamento(planejamentoNutricional.Id);
                else
                    planejamentoNutricional.PlanejamentoValoresPasto = await ObterPlanejamentoValoresPastos(planejamentoNutricional.Id);
            }

            return planejamentoNutricional;
        }

        private async Task<List<PlanejamentoValoresPasto>> ObterPlanejamentoValoresPastos(int idPlanejamento)
        {
            return await Context.PlanejamentosValoresPasto
                            .AsNoTracking()
                            .Include(x => x.FaseDoAno)
                            .Include(x => x.Categoria)
                            .Include(x => x.SuplementoMineral)
                            .Where(x => x.Status == Business.Enums.Status.Ativado && x.IdPlanejamento == idPlanejamento)
                            .ToListAsync();
        }

        private async Task<List<PlanejamentoValoresConfinamento>> ObterPlanejamentoValoresConfinamento(int idPlanejamento)
        {
            return await Context.PlanejamentosValoresConfinamento
                            .AsNoTracking()
                            .Include(x => x.Racao)
                            .Where(x => x.Status == Business.Enums.Status.Ativado && x.IdPlanejamento == idPlanejamento)
                            .ToListAsync();
        }

        public async Task<List<PlanejamentoNutricionalDTO>> ObterPaginacao()
        {
            return await DbSet.AsNoTracking()
                               .Include(x => x.Cliente)
                                    .ThenInclude(c => c.Tecnico)
                              .Where(ObterWhere())
                              .Select(x => new PlanejamentoNutricionalDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  Tipo = x.Tipo,
                                  Proprietario = x.Cliente.Nome,
                                  NomePropriedade = x.Cliente.NomePropriedade,
                                  Tecnico = x.Cliente.Tecnico.Nome,
                              }).ToListAsync();
        }

        public async Task<List<PlanejamentoNutricionalDTO>> BuscarQuery(Expression<Func<PlanejamentoNutricional, bool>> predicate)
        {
            return await DbSet.AsNoTracking()
                              .Where(ObterWhere().And(predicate))
                              .Select(x => new PlanejamentoNutricionalDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  Tipo = x.Tipo
                              }).ToListAsync();
        }

        public async Task<List<GerenciarPlanejamentoLoteDTO>> BuscarLotesGerenciamento(int idPlanejamento)
        {
            var where = PredicateBuilder.True<LoteEntrada>()
                    .And(x => x.IdCliente == AppUser.ObterIdCliente())
                    .And(x => x.Status == Status.Ativado)
                    .And(x => x.IdPlanejamento == idPlanejamento);

            return await Context.LotesEntradas.AsNoTracking()
                                .Include(x => x.Local)
                                .Include(x => x.Planejamento)
                                .Where(where)
                                .Select(x => new GerenciarPlanejamentoLoteDTO
                                {
                                    DataEntrada = x.DataEntrada,
                                    IdPlanejamento = x.IdPlanejamento,
                                    IdLote = x.Id,
                                    Local = x.Local.Nome,
                                    Tipo = x.Planejamento.Tipo,
                                    QuantidadeAnimais = x.Local.Lotacao.Value
                                })
                                .ToListAsync();
        }

        public async Task<List<PlanejamentoValoresPasto>> BuscarPlanejamentoContemSuplemento(int idSuplemento)
        {
            var resultado = await DbConnection.QueryAsync<PlanejamentoValoresPasto>("SELECT " +
                "   p3.Id, p3.IdPlanejamento, p3.IdCategoria, p3.IdSuplemento, p3.IdFase, " +
                "   p3.ImspvEsperado, p3.GmdEsperado " +
                " FROM planejamentonutricional p2 " +
                " INNER JOIN planejamentovalorespasto p3 ON p3.idplanejamento = p2.id " +
                " WHERE p3.idsuplemento = @idSuplemento AND p3.status = 1 AND p2.status = 1 ", new { idSuplemento });

            var list = resultado.ToList();

            foreach (var item in list)
            {
                item.FaseDoAno = await Context.FaseDoAno.AsNoTracking().SingleOrDefaultAsync(x => x.Id == item.IdFase);
            }

            return list;
        }
    }
}
