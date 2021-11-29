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
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        public CategoriaRepositorio(PlataformaFieldContext context, IUser appUser)
            : base(context, appUser)
        {
        }

        public override Expression<Func<Categoria, bool>> ObterWhere()
        {
            var where = base.ObterWhere();

            //if (!AppUser.EhAdmin())
                where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());

            return where;
        }

        public async override Task<Categoria> ObterPorId(int id)
        {
            return await DbSet.AsNoTracking()
                              .Where(ObterWhere().And(x => x.Id == id))
                              .FirstOrDefaultAsync();
        }

        public async Task<List<CategoriaDTO>> ObterPaginacao(int? idCliente = null)
        {
            return await DbSet.AsNoTracking()
                               .Include(x => x.Cliente)
                                    .ThenInclude(c => c.Tecnico)
                              .Where(ObterWhere())
                              .Select(x => new CategoriaDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  IdadeMaxima = x.IdadeMaxima,
                                  IdadeMinima = x.IdadeMinima,
                                  Sexo = x.Sexo,
                                  Proprietario = x.Cliente.Nome,
                                  NomePropriedade = x.Cliente.NomePropriedade,
                                  Tecnico = x.Cliente.Tecnico.Nome,
                              }).ToListAsync();
        }

        public async Task<IEnumerable<CategoriaDTO>> BuscarQuery(Expression<Func<Categoria, bool>> predicate)
        {
            return await DbSet.AsNoTracking()                               
                              .Where(ObterWhere().And(predicate))
                              .Select(x => new CategoriaDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  IdadeMaxima = x.IdadeMaxima,
                                  IdadeMinima = x.IdadeMinima,
                                  Sexo = x.Sexo
                              }).ToListAsync();
        }
    }
}
