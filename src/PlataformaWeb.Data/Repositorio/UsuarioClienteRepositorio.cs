using Microsoft.EntityFrameworkCore;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Data.Repositorio
{
    public class UsuarioClienteRepositorio : Repositorio<UsuarioCliente>, IUsuarioClienteRepositorio
    {
        public UsuarioClienteRepositorio(PlataformaFieldContext context, IUser appUser)
            : base(context, appUser)
        {
        }

        private Expression<Func<UsuarioCliente, bool>> ObterWhere(int? idCliente)
        {
            var where = ObterWhere();

            if (idCliente.HasValue)
                where = where.And(x => x.IdCliente == idCliente.Value);

            return where;
        }

        public async override Task<UsuarioCliente> ObterPorId(int id)
        {
            return await DbSet.AsNoTracking()                              
                              .Where(ObterWhere().And(x => x.Id == id))
                              .FirstOrDefaultAsync();
        }

        public override Expression<Func<UsuarioCliente, bool>> ObterWhere()
        {
            //var where = base.ObterWhere();
            var where = base.ObterWhere()
                                .And(x => x.IdCliente == AppUser.ObterIdCliente());

            //if (!AppUser.EhAdmin())
            //    where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());
                       

            return where;
        }
               
        public async Task<List<UsuarioClienteDTO>> ObterPaginacao(int? idCliente = null)
        {
            return await DbSet.AsNoTracking()
                               .Include(x => x.Cliente)
                                .ThenInclude(c => c.Tecnico)
                              .Where(ObterWhere(idCliente))
                              .Select(model => new UsuarioClienteDTO
                              {
                                  Id = model.Id,
                                  Nome = model.Nome,
                                  Email = model.Email,
                                  Usuario = model.Usuario,
                                  Propriedade = model.Cliente.NomePropriedade,
                                  Tecnico = model.Cliente.Tecnico.Nome
                              })
                              .ToListAsync();
        }

        public async Task<int> ObterIdCliente(int idUsuarioCliente)
        {
            return await DbSet.Where(base.ObterWhere().And(x => x.Id == idUsuarioCliente)).Select(x => x.IdCliente).SingleOrDefaultAsync() ?? 0;
        }
    }
}
