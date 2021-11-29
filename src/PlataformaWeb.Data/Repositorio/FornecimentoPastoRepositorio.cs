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
    public class FornecimentoPastoRepositorio : Repositorio<FornecimentoPasto>, IFornecimentoPastoRepositorio
    {
        public FornecimentoPastoRepositorio(PlataformaFieldContext context, IUser appUser) 
            : base(context, appUser)
        {
        }

        public override Expression<Func<FornecimentoPasto, bool>> ObterWhere()
        {
            var where = base.ObterWhere()
                .And(x => x.IdCliente == AppUser.ObterIdCliente());

            return where;
        }

        public async override Task<FornecimentoPasto> ObterPorId(int id)
        {
            return await DbSet.AsNoTracking()
                            .SingleOrDefaultAsync(ObterWhere().And(x => x.Id == id));
        }

        public async Task<List<FornecimentoPastoDTO>> BuscarQuery(FiltroFornecimentoPastoDTO filtro)
        {
            var where = ObterWhere().And(x => x.DataRealizado.CompareTo(filtro.DataInicio.Date) >= 0)
                                    .And(x => x.DataRealizado.CompareTo(filtro.DataFinal.Date) <= 0);

            if (filtro.IdPasto.HasValue)
            {
                where = where.And(x => x.IdPasto == filtro.IdPasto.Value);
            }

            return await DbSet.AsNoTracking()
                            .Include(x => x.Suplemento)
                            .Include(x => x.Pasto)
                            .Where(where)
                            .Select(x => new FornecimentoPastoDTO
                            {
                                DataRealizado = x.DataRealizado,
                                Destino = x.Destino,
                                Origem = x.Origem,
                                Id = x.Id,
                                Pasto = x.Pasto.Nome,
                                PrevisaoKg = x.PrevisaoKg,
                                PrevisaoSaco = x.PrevisaoSaco,
                                QuantidadeAnimais = x.QuantidadeAnimais,
                                RealizadoKg = x.RealizadoKg,
                                RealizadoSaco = x.RealizadoSaco,
                                Suplemento = x.Suplemento.Nome
                            })
                            .OrderBy(x => x.DataRealizado)
                            .ToListAsync();
        }

        public async Task<FornecimentoPasto> BuscarUltimoFornecimento(int idLote, int idPasto, int idSuplemento)
        {
            return await DbSet.AsNoTracking()
                            .Where(ObterWhere().And(x => x.IdLote == idLote && x.IdPasto == idPasto && x.IdSuplemento == idSuplemento))                            
                            .OrderByDescending(x => x.DataRealizado)
                            .FirstOrDefaultAsync();
        }
    }
}
