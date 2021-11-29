using Microsoft.EntityFrameworkCore;
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
using System.Linq;
using PlataformaWeb.Business.Utils;

namespace PlataformaWeb.Data.Repositorio
{
    public class FaseDoAnoRepositorio : Repositorio<FaseDoAno>, IFaseDoAnoRepositorio
    {
        public FaseDoAnoRepositorio(PlataformaFieldContext context, 
                                    IUser appUser) : base(context, appUser)
        {
        }

        public override Expression<Func<FaseDoAno, bool>> ObterWhere()
        {
            var where = base.ObterWhere();

            //if (!AppUser.EhAdmin())
                where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());

            return where;
        }

        public async override Task<FaseDoAno> ObterPorId(int id)
        {
            return await DbSet.AsNoTracking()
                              .Where(ObterWhere().And(x => x.Id == id))
                              .FirstOrDefaultAsync();
        }


        public async Task<List<FaseDoAnoDTO>> ObterPaginacao()
        {
            return await DbSet.AsNoTracking()
                               .Include(x => x.Cliente)
                                    .ThenInclude(c => c.Tecnico)
                              .Where(ObterWhere())
                              .Select(x => new FaseDoAnoDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  DataInicio = x.DataInicio,
                                  DataFim = x.DataFim,
                                  Proprietario = x.Cliente.Nome,
                                  NomePropriedade = x.Cliente.NomePropriedade,
                                  Tecnico = x.Cliente.Tecnico.Nome,
                              }).ToListAsync();
        }

        public async Task<List<FaseDoAno>> ObterFaseNoPeriodo(FaseDoAno fase)
        {
            var where = ObterWhere();
            if (fase.Id > 0)
                where = where.And(x => x.Id != fase.Id);

            var whereData = PredicateBuilder.True<FaseDoAno>();
            whereData = whereData.And(x => x.DataInicio.Value.CompareTo(fase.DataInicio.Value) <= 0 && x.DataFim.Value.CompareTo(fase.DataInicio.Value) >= 0);
            whereData = whereData.Or(x => x.DataInicio.Value.CompareTo(fase.DataFim.Value) <= 0 && x.DataFim.Value.CompareTo(fase.DataFim.Value) >= 0);
            whereData = whereData.Or(x => x.DataInicio.Value.CompareTo(fase.DataInicio.Value) >= 0 && x.DataInicio.Value.CompareTo(fase.DataFim.Value) <= 0);

            return await DbSet.AsNoTracking()
                              .Where(where.And(whereData))
                              .ToListAsync();
        }

        public async Task<IEnumerable<FaseDoAnoDTO>> BuscarQuery(Expression<Func<FaseDoAno, bool>> predicate)
        {
            return await DbSet.AsNoTracking()                               
                              .Where(ObterWhere().And(predicate))
                              .Select(x => new FaseDoAnoDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  DataInicio = x.DataInicio,
                                  DataFim = x.DataFim
                              }).ToListAsync();
        }
    }
}
