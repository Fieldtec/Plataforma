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
    public class FrigorificoRepositorio : Repositorio<Frigorifico>, IFrigorificoRepositorio
    {
        public FrigorificoRepositorio(PlataformaFieldContext context,
                                    IUser appUser) : base(context, appUser)
        {
        }

        public override Expression<Func<Frigorifico, bool>> ObterWhere()
        {
            var where = base.ObterWhere();

            //if (!AppUser.EhAdmin())
            where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());

            return where;
        }

        public async override Task<Frigorifico> ObterPorId(int id)
        {
            return await DbSet.AsNoTracking()
                              .Where(ObterWhere().And(x => x.Id == id))
                              .FirstOrDefaultAsync();
        }


        public async Task<List<FrigorificoDTO>> ObterPaginacao()
        {
            return await DbSet.AsNoTracking()
                               .Include(x => x.Cliente)
                                    .ThenInclude(c => c.Tecnico)
                              .Where(ObterWhere())
                              .Select(x => new FrigorificoDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  Cidade = x.Cidade,
                                  Uf = x.Uf,
                                  Proprietario = x.Cliente.Nome,
                                  NomePropriedade = x.Cliente.NomePropriedade,
                                  Tecnico = x.Cliente.Tecnico.Nome,
                              }).ToListAsync();
        }

        public async Task<IEnumerable<FrigorificoDTO>> BuscarQuery(Expression<Func<Frigorifico, bool>> predicate)
        {
            return await DbSet.AsNoTracking()
                              .Where(ObterWhere().And(predicate))
                              .Select(x => new FrigorificoDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  Cidade = x.Cidade,
                                  Uf = x.Uf
                              }).ToListAsync();
        }
    }

}
