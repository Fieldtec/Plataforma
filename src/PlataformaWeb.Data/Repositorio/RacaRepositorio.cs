using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PlataformaWeb.Data.Repositorio
{
    public class RacaRepositorio : Repositorio<Raca>, IRacaRepositorio
    {
        public RacaRepositorio(PlataformaFieldContext context, IUser appUser) : base(context, appUser)
        {
        }

        public override Expression<Func<Raca, bool>> ObterWhere()
        {
            var where = base.ObterWhere();

            //if (!AppUser.EhAdmin())
                where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());

            return where;
        }

        public async override Task<Raca> ObterPorId(int id)
        {
            return await DbSet.AsNoTracking()                              
                              .Where(ObterWhere().And(x => x.Id == id))
                              .FirstOrDefaultAsync();
        }

        public async Task<List<RacaDTO>> ObterPaginacao(int? idCliente = null)
        {
            return await DbSet.AsNoTracking()
                               .Include(x => x.Cliente)
                                    .ThenInclude(c => c.Tecnico)
                              .Where(ObterWhere())
                              .Select(x => new RacaDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  CodigoBnd = x.CodigoBnd,
                                  Proprietario = x.Cliente.Nome,
                                  NomePropriedade = x.Cliente.NomePropriedade,
                                  Tecnico = x.Cliente.Tecnico.Nome,
                              }).ToListAsync();
        }

        public async Task<List<RacaDTO>> BuscarQuery(Expression<Func<Raca, bool>> predicate)
        {
            return await DbSet.AsNoTracking()
                              .Where(ObterWhere().And(predicate))
                              .Select(x => new RacaDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  CodigoBnd = x.CodigoBnd,
                              }).ToListAsync();
        }
    }
}
