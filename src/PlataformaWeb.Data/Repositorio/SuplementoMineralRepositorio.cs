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
    public class SuplementoMineralRepositorio : Repositorio<SuplementoMineral>, ISuplementoMineralRepositorio
    {
        public SuplementoMineralRepositorio(PlataformaFieldContext context, 
                                            IUser appUser) : base(context, appUser)
        {
        }

        public override Expression<Func<SuplementoMineral, bool>> ObterWhere()
        {
            var where = base.ObterWhere();

            //if (!AppUser.EhAdmin())
                where = where.And(x => x.FornecedorInsumo.IdCliente == AppUser.ObterIdCliente());

            return where;
        }

        public async override Task<SuplementoMineral> ObterPorId(int id)
        {
            return await DbSet.AsNoTracking()
                              .Include(x => x.FornecedorInsumo)
                              .Where(ObterWhere().And(x => x.Id == id))
                              .FirstOrDefaultAsync();
        }

        public async Task<List<SuplementoMineralDTO>> ObterPaginacao()
        {
            return await DbSet.AsNoTracking()
                               .Include(x => x.FornecedorInsumo)
                                    .ThenInclude(c => c.Cliente)
                                    .ThenInclude(t => t.Tecnico)
                              .Where(ObterWhere())
                              .Select(x => new SuplementoMineralDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  Proprietario = x.FornecedorInsumo.Cliente.Nome,
                                  NomeFornecedorInsumo = x.FornecedorInsumo.Nome,
                                  ConsumoEsperado = x.ConsumoEsperado,
                                  Tecnico = x.FornecedorInsumo.Cliente.Tecnico.Nome,
                              }).ToListAsync();
        }

        public async Task<List<SuplementoMineralDTO>> BuscarQuery(Expression<Func<SuplementoMineral, bool>> predicate)
        {
            return await DbSet.AsNoTracking()
                                .Include(x => x.FornecedorInsumo)
                                .Where(ObterWhere().And(predicate))
                                .Select(x => new SuplementoMineralDTO
                                {
                                    Id = x.Id,
                                    Nome = x.Nome,
                                    ConsumoEsperado = x.ConsumoEsperado,
                                    NomeFornecedorInsumo = x.FornecedorInsumo.Nome
                                }).ToListAsync();
        }
    }
}
