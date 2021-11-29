using Microsoft.EntityFrameworkCore;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using PlataformaWeb.Business.DTO;
using System.Dynamic;
using Dapper;

namespace PlataformaWeb.Data.Repositorio
{
    public class FornecimentoConfinamentoRepositorio : Repositorio<FornecimentoConfinamento>, IFornecimentoConfinamentoRepositorio
    {
        public FornecimentoConfinamentoRepositorio(PlataformaFieldContext context, IUser appUser) : base(context, appUser)
        {
        }

        public async Task<bool> EhPrimeiroFornecimentoDoLote(int idCurral, DateTime dataFornecimento)
        {
            var resultado = await DbSet.AsNoTracking()
                                    .Where(ObterWhere().And(x => x.IdCliente == AppUser.ObterIdCliente() && x.IdCurral == idCurral))
                                    .OrderBy(x => x.DataFornecimento)
                                    .FirstOrDefaultAsync();

            return dataFornecimento.Date.CompareTo(resultado.DataFornecimento.Date) == 0;
        }

        public async Task<bool> ExisteFornecimento(int idLocal, DateTime dataFornecimento)
        {
            return await DbSet.AsNoTracking()
                        .Where(ObterWhere().And(x => x.IdCurral == idLocal && x.DataFornecimento.Date.CompareTo(dataFornecimento.Date) == 0))
                        .AnyAsync();
        }

        public async Task<List<FornecimentoConfinamentoDTO>> ObterPorData(DateTime dataFornecimento)
        {
            var resultado = await DbConnection.QueryAsync<FornecimentoConfinamentoDTO>("SELECT " +
               " f.Id, f.DataFornecimento, f.qtdeanimais QuantidadeAnimais, p.nome Curral, r.nome NomeRacao, r.materiaseca MateriaSecaRacao, f.KgPrevisto, f.KgRealizado, f.Ajuste, " +
               " CASE WHEN (select min(f2.datafornecimento) from fornecimentoconfinamento f2 where f2.status = 1 and f2.idcliente = @idCliente and f2.idlote = f.idlote) = @dataFornecimento then 1 else 0 end EhPrimeiroDia " +
               "   FROM fornecimentoconfinamento f " +
               " INNER JOIN pastocurral p on p.id = f.idcurral and p.status = 1 " +
               " INNER JOIN racao r on r.id = f.idracao and r.status = 1 " +
               " INNER JOIN loteentrada l2 on l2.id = f.idlote and l2.status = 1 " +
               " WHERE f.status = 1 AND f.idcliente = @idCliente AND  f.datafornecimento = @dataFornecimento " +
               " ORDER BY p.ordemfornecimento, p.nome",
                    new { dataFornecimento = dataFornecimento.Date, idCliente = AppUser.ObterIdCliente() });

            return resultado.ToList();
        }

        public async Task<List<FornecimentoConfinamentoDTO>> ObterTodosFiltro(FiltroFornecimentoConfinamentoDTO filtro)
        {
            if (filtro == null)
                filtro = new FiltroFornecimentoConfinamentoDTO { DataInicio = DateTime.Now, DataFinal = DateTime.Now, IdCurral = null, IdRacao = null };

            dynamic where = new ExpandoObject();
            StringBuilder sql = new StringBuilder();

            where.dataInicio = filtro.DataInicio.Date;
            where.dataFinal = filtro.DataFinal.Date;

            sql.AppendLine("SELECT " +
                " f.Id, f.DataFornecimento, f.qtdeanimais QuantidadeAnimais, p.nome Curral, r.nome NomeRacao, r.materiaseca MateriaSecaRacao, f.KgPrevisto, f.KgRealizado, f.Ajuste " +
                "   FROM fornecimentoconfinamento f " +
                " INNER JOIN pastocurral p on p.id = f.idcurral and p.status = 1 " +
                " INNER JOIN racao r on r.id = f.idracao and r.status = 1 " +
                " INNER JOIN loteentrada l2 on l2.id = f.idlote and l2.status = 1 " +
                " WHERE f.status = 1 AND f.idcliente = @idCliente AND f.datafornecimento between @dataInicio and @dataFinal ");

            if (filtro.IdCurral.HasValue)
            {
                where.idCurral = filtro.IdCurral.Value;
                sql.AppendLine(" AND p.id = @idCurral ");
            }

            if (filtro.IdRacao.HasValue)
            {
                where.idRacao = filtro.IdRacao.Value;
                sql.AppendLine(" AND r.id = @idRacao ");
            }

            where.idCliente = AppUser.ObterIdCliente();

            sql.AppendLine(" ORDER BY f.datafornecimento");

            var resultado = await DbConnection.QueryAsync<FornecimentoConfinamentoDTO>(sql.ToString(), (object)where);

            return resultado.ToList();
        }
    }
}
