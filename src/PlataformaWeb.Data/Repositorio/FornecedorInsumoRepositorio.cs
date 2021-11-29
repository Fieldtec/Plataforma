using Microsoft.EntityFrameworkCore;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Data.Repositorio
{
    public class FornecedorInsumoRepositorio : Repositorio<FornecedorInsumo>, IFornecedorInsumoRepositorio
    {
        public FornecedorInsumoRepositorio(PlataformaFieldContext context, 
                                           IUser appUser) : base(context, appUser)
        {
        }

        public override Task<List<FornecedorInsumo>> Buscar(Expression<Func<FornecedorInsumo, bool>> predicate)
        {
            return base.Buscar(ObterWhere().And(predicate));
        }

        public override Expression<Func<FornecedorInsumo, bool>> ObterWhere()
        {
            var where = base.ObterWhere();

            //if (!AppUser.EhAdmin())
                where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());

            return where;
        }

        public async override Task<FornecedorInsumo> ObterPorId(int id)
        {
            return await DbSet.AsNoTracking()
                              .Where(ObterWhere().And(x => x.Id == id))
                              .FirstOrDefaultAsync();
        }

        public async Task<List<FornecedorInsumoDTO>> ObterPaginacao()
        {
            return await DbSet.AsNoTracking()
                               .Include(x => x.Cliente)
                                    .ThenInclude(c => c.Tecnico)
                              .Where(ObterWhere())
                              .Select(x => new FornecedorInsumoDTO
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
