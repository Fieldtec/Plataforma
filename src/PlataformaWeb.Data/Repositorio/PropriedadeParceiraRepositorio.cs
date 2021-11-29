using Microsoft.EntityFrameworkCore;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Data.Repositorio
{
    public class PropriedadeParceiraRepositorio : Repositorio<PropriedadeParceira>, IPropriedadeParceiraRepositorio
    {
        public PropriedadeParceiraRepositorio(PlataformaFieldContext context, IUser appUser) 
            : base(context, appUser)
        {
        }

        public override Task<List<PropriedadeParceira>> Buscar(Expression<Func<PropriedadeParceira, bool>> predicate)
        {
            return base.Buscar(ObterWhere().And(predicate));
        }

        public async override Task<PropriedadeParceira> ObterPorId(int id)
        {
            return await DbSet.AsNoTracking()
                              .Where(ObterWhere().And(x => x.Id == id))
                              .FirstOrDefaultAsync();
        }

        public override Expression<Func<PropriedadeParceira, bool>> ObterWhere()
        {
            var where = base.ObterWhere();

            //if (!AppUser.EhAdmin())
                where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());

            return where;
        }

        public async Task<List<PropriedadeParceiraDTO>> ObterPaginacao(int? idCliente = null)
        {
            return await DbSet.AsNoTracking()
                              .Include(x => x.Cliente)
                                   .ThenInclude(c => c.Tecnico)
                             .Where(ObterWhere())
                             .Select(x => new PropriedadeParceiraDTO
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
    }
}
