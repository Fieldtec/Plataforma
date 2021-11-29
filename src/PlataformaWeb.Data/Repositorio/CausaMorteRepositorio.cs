using Microsoft.EntityFrameworkCore;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

namespace PlataformaWeb.Data.Repositorio
{
    public class CausaMorteRepositorio : Repositorio<CausaMorte>, ICausaMorteRepositorio
    {
        public CausaMorteRepositorio(PlataformaFieldContext context,
                                    IUser appUser) : base(context, appUser)
        {
        }

        public override Expression<Func<CausaMorte, bool>> ObterWhere()
        {
            var where = base.ObterWhere();

            //if (!AppUser.EhAdmin())
            where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());

            return where;
        }

        public async override Task<CausaMorte> ObterPorId(int id)
        {
            return await DbSet.AsNoTracking()
                              .Where(ObterWhere().And(x => x.Id == id))
                              .FirstOrDefaultAsync();
        }


        public async Task<List<CausaMorteDTO>> ObterPaginacao()
        {
            return await DbSet.AsNoTracking()
                               .Include(x => x.Cliente)
                                    .ThenInclude(c => c.Tecnico)
                              .Where(ObterWhere())
                              .Select(x => new CausaMorteDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  Proprietario = x.Cliente.Nome,
                                  NomePropriedade = x.Cliente.NomePropriedade,
                                  Tecnico = x.Cliente.Tecnico.Nome,
                              }).ToListAsync();
        }

        public async Task<IEnumerable<CausaMorteDTO>> BuscarQuery(Expression<Func<CausaMorte, bool>> predicate)
        {
            return await DbSet.AsNoTracking()
                              .Where(ObterWhere().And(predicate))
                              .Select(x => new CausaMorteDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                              }).ToListAsync();
        }
    }

}
