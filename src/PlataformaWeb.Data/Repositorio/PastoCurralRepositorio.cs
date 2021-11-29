using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Dapper;

namespace PlataformaWeb.Data.Repositorio
{
    public class PastoCurralRepositorio : Repositorio<PastoCurral>, IPastoCurralRepositorio
    {
        public PastoCurralRepositorio(PlataformaFieldContext context, IUser appUser)
            : base(context, appUser)
        {
        }


        public override Expression<Func<PastoCurral, bool>> ObterWhere()
        {
            var where = base.ObterWhere();

            //if (!AppUser.EhAdmin())
                where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());

            return where;
        }

        public async override Task<PastoCurral> ObterPorId(int id)
        {
            return await DbSet.AsNoTracking()
                              .Where(ObterWhere().And(x => x.Id == id))
                              .FirstOrDefaultAsync();
        }

        public async Task<List<PastoCurralDTO>> ObterPaginacao(int? idCliente = null)
        {
            return await DbSet.AsNoTracking()
                               .Include(x => x.Cliente)
                                    .ThenInclude(c => c.Tecnico)
                              .Where(ObterWhere())
                              .Select(x => new PastoCurralDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  Tipo = x.Tipo,
                                  Capacidade = x.Capacidade,
                                  Lotacao = x.Lotacao,
                                  Proprietario = x.Cliente.Nome,
                                  NomePriedade = x.Cliente.NomePropriedade,
                                  Tecnico = x.Cliente.Tecnico.Nome,
                              })
                              .OrderBy(x => x.Nome)
                              .ToListAsync();                              
        }

        public async Task<List<PastoCurralDTO>> BuscarQuery(Expression<Func<PastoCurral, bool>> predicate)
        {
            return await DbSet.AsNoTracking()
                              .Where(ObterWhere().And(predicate))
                              .Select(x => new PastoCurralDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  Tipo = x.Tipo,
                                  Capacidade = x.Capacidade,
                                  Lotacao = x.Lotacao ?? 0
                              })
                              .OrderBy(x => x.Nome)
                              .ToListAsync();
        }

        public async Task<bool> ExisteLoteAtivo(int id)
        {
            var count = await Context.LotesEntradas
                .Where(x => x.IdLocal == id && x.Status == Business.Enums.Status.Ativado)
                .CountAsync();

            return count > 0;
        }

        public async Task<List<LocalLoteDTO>> BuscarLocaisAtivos()
        {
            var resultado = await DbConnection.QueryAsync<LocalLoteDTO>(
                "select p.Id, l.id IdLote, p.Nome, count(a2.idlote) Lotacao " +
                "FROM pastocurral p " +
                "INNER JOIN loteentrada l ON p.id = l.idlocal and l.status = 1 " +
                "INNER JOIN animal a2 on a2.idlote = l.id and a2.status = 1 " +
                "WHERE p.status = 1 AND p.idcliente = @idCliente " +
                "GROUP BY 1,2 " +
                "HAVING lotacao > 0 " +
                "ORDER BY p.nome", new { idCliente = AppUser.ObterIdCliente() });

            return resultado.ToList();
        }
    }
}
