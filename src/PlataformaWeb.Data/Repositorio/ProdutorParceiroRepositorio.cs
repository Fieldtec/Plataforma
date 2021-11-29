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
    public class ProdutorParceiroRepositorio : Repositorio<ProdutorParceiro>, IProdutorParceiroRepositorio
    {
        public ProdutorParceiroRepositorio(PlataformaFieldContext context, IUser appUser) : base(context, appUser)
        {
        }

        public override Expression<Func<ProdutorParceiro, bool>> ObterWhere()
        {
            var where = base.ObterWhere();

            //if (!AppUser.EhAdmin())
                where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());

            return where;
        }

        public async override Task<ProdutorParceiro> ObterPorId(int id)
        {
            return await DbSet.AsNoTracking()
                              .Include(x => x.PropriedadeParceira)
                              .Where(ObterWhere().And(x => x.Id == id))
                              .FirstOrDefaultAsync();
        }

        public async Task<List<ProdutorParceiroDTO>> ObterPaginacao(int? idCliente = null)
        {
            return await DbSet.AsNoTracking()
                              .Include(x => x.PropriedadeParceira)
                              .Include(x => x.Cliente)
                                   .ThenInclude(c => c.Tecnico)
                             .Where(ObterWhere())
                             .Select(x => new ProdutorParceiroDTO
                             {
                                 Id = x.Id,
                                 Nome = x.Nome,
                                 CpfCnpj = x.CpfCnpj,                             
                                 Proprietario = x.Cliente.Nome,
                                 NomePropriedadeParceira = x.PropriedadeParceira.Nome,
                                 NomePropriedade = x.Cliente.NomePropriedade,
                                 Tecnico = x.Cliente.Tecnico.Nome,
                             }).ToListAsync();
        }

        public async Task<List<ProdutorParceiroDTO>> BuscarQuery(Expression<Func<ProdutorParceiro, bool>> predicate)
        {
            return await DbSet.AsNoTracking()
                              .Include(x => x.PropriedadeParceira)
                             .Where(ObterWhere().And(predicate))
                             .Select(x => new ProdutorParceiroDTO
                             {
                                 Id = x.Id,
                                 Nome = x.Nome,
                                 CpfCnpj = x.CpfCnpj,
                                 NomePropriedadeParceira = x.PropriedadeParceira.Nome,
                             }).ToListAsync();
        }
    }
}
