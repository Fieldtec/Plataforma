using Microsoft.EntityFrameworkCore;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlataformaWeb.Business.Enums;
using Dapper;
using System.Dynamic;

namespace PlataformaWeb.Data.Repositorio
{
    public class LeituraCochoRepositorio : Repositorio<LeituraCocho>, ILeituraCochoRepositorio
    {
        public LeituraCochoRepositorio(PlataformaFieldContext context,
            IUser appUser) : base(context, appUser)
        {
        }

        public async Task<List<LeituraCochoInsercaoDTO>> ObterLeiturasInsercao(DateTime dataLeitura)
        {
            var resultado = await DbConnection.QueryAsync<LeituraCochoInsercaoDTO>("SELECT " +
                "    l.Id, l3.id IdLote, l.dataleitura DataLeitura, p.id IdCurral, p.nome Curral, l.qtdanimais QuantidadeAnimais, n2.nome Nota, l.ajustegramas Ajuste, " +
                //"      f.kgrealizado RealizadoMateriaNatural, (f.kgrealizado * (r2.materiaseca/100)) RealizadoMateriaSeca, " +
                "    CASE WHEN f.kgrealizado = 0 THEN f.kgprevisto ELSE f.kgrealizado END AS RealizadoMateriaNatural, " +
                "   (CASE WHEN f.kgrealizado = 0 THEN f.kgprevisto ELSE f.kgrealizado END * (r2.materiaseca/100)) RealizadoMateriaSeca, " +
                "    CASE WHEN (SELECT id FROM fornecimentoconfinamento f2 WHERE f2.status = 1 AND f2.datafornecimento = @dataLeitura and f2.idcurral = p.id) > 0 THEN true ELSE false END AS TemLancamentoInsumo " +
                " FROM pastocurral p  " +
                "     INNER join loteentrada l3 on l3.idlocal = p.id and l3.status = 1 " +
                "     LEFT JOIN leituracocho l on p.id = l.idlocal AND l.dataleitura = @dataLeitura  and l.status = 1 " +
                "     LEFT JOIN notasleituracocho n2 on n2.ajusteporcentagem = l.ajustegramas and n2.idcliente = p.idcliente and n2.status = 1 " +
                "     LEFT JOIN fornecimentoconfinamento f on f.datafornecimento = @dataAnterior and f.idcurral = p.id and f.status = 1 and f.idlote = l3.id " +
                "     LEFT JOIN racao r2 on r2.id = f.idracao " +
                " WHERE p.status = 1 AND p.tipo = 1 AND p.lotacao > 0 AND p.idcliente = @idCliente " +
                "    and @dataLeitura > (select min(l2.dataentrada) from animal l2 join loteentrada l on l.id=l2.idlote where l2.status = 1 and l.idlocal =p.id)" +
                " ORDER BY p.ordemfornecimento, p.nome ", 
                new { dataLeitura = dataLeitura.Date, dataAnterior = dataLeitura.AddDays(-1).Date, idCliente = AppUser.ObterIdCliente() });

            return resultado.ToList();
        }

        public async Task<List<LeituraCochoDTO>> ObterTodosFiltro(FiltroLeituraCochoDTO filtro)
        {

            if (filtro == null)
                filtro = new FiltroLeituraCochoDTO { DataInicio = DateTime.Now, DataFinal = DateTime.Now, IdCurral = null };

            dynamic where = new ExpandoObject();
            StringBuilder sql = new StringBuilder();

            where.dataInicio = filtro.DataInicio.Date;
            where.dataFinal = filtro.DataFinal.Date;

            sql.AppendLine("SELECT " +
                " l.id, l.dataleitura DataLeitura, p.nome Curral, l.ajustegramas Ajuste, n2.nome Nota " +
                "   FROM leituracocho l " +
                " INNER JOIN pastocurral p on p.id = l.idlocal and p.status = 1 and p.tipo = 1 " +
                " LEFT JOIN notasleituracocho n2 on n2.ajusteporcentagem = l.ajustegramas and n2.idcliente = l.idcliente " +
                " WHERE l.status = 1 AND l.idcliente = @idCliente AND l.dataleitura between @dataInicio and @dataFinal ");

            if (filtro.IdCurral.HasValue)
            {
                where.idCurral = filtro.IdCurral.Value;
                sql.AppendLine(" AND p.id = @idCurral ");
            }

            where.idCliente = AppUser.ObterIdCliente();

            sql.AppendLine(" ORDER BY l.dataleitura");

            var resultado = await DbConnection.QueryAsync<LeituraCochoDTO>(sql.ToString(), (object)where);

            return resultado.ToList();
        }
    }
}
