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
using Dapper;
using System.Dynamic;
using PlataformaWeb.Business.Enums;

namespace PlataformaWeb.Data.Repositorio
{
    public class PrevisaoFornecimentoPastoRepositorio : Repositorio<PrevisaoFornecimentoPasto>, IPrevisaoFornecimentoPastoRepositorio
    {
        public PrevisaoFornecimentoPastoRepositorio(PlataformaFieldContext context, IUser appUser) 
            : base(context, appUser)
        {

        }

        public override Expression<Func<PrevisaoFornecimentoPasto, bool>> ObterWhere()
        {
            var where = base.ObterWhere();
            where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());
            return where;
        }

        public async Task<List<PrevisaoFornecimentoPastoDTO>> BuscarQuery(FiltroPrevisaoFornecimentoPastoDTO filtro)
        {
            dynamic where = new ExpandoObject();
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT " +
                " p.Id, p.dataprev DataPrevisao, p.qtdanimais QuantidadeAnimais, p.PrevisaoKg, p.PrevisaoSaco, p2.nome Pasto, s2.nome Suplemento " +
                "FROM previsaofornecimentopasto p " +
                "INNER JOIN pastocurral p2 on p2.id = p.idpasto " +
                "INNER JOIN suplementomineral s2 on s2.id  = p.idsuplemento " +
                "WHERE p.idcliente = @idCliente " +
                "  and p.status = 1 and p2.tipo = 2 and p.dataprev between @dtInicial and @dtFinal ");

            

            where.dtInicial = filtro.DataInicio.Date;
            where.dtFinal = filtro.DataFinal.Date;
            where.idCliente = AppUser.ObterIdCliente();

            if (filtro.IdPasto.HasValue)
            {
                where.idPasto = filtro.IdPasto.Value;
                sql.Append(" and p.idpasto = @idPasto ");
            }

            sql.Append(" ORDER BY p.dataprev");

            var resultado = await DbConnection.QueryAsync<PrevisaoFornecimentoPastoDTO>(sql.ToString(), (object)where);

            return resultado.ToList();
        }

        public async Task<List<PrevisaoFornecimentoPastoDTO>> ObterPaginacao()
        {
            return await DbSet.AsNoTracking()
                           .Where(ObterWhere())
                           .Select(x => new PrevisaoFornecimentoPastoDTO
                           {
                               DataPrevisao = x.DataPrevisao,
                               Id = x.Id,
                               PrevisaoKg = x.PrevisaoKg.Value,
                               PrevisaoSaco = x.PrevisaoSaco.Value,
                               QuantidadeAnimais = x.QuantidadeAnimais,
                               Suplemento = x.Suplemento.Nome,
                               Pasto = x.Pasto.Nome
                           }).ToListAsync();
        }
    }
}
