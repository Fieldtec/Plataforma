using Microsoft.EntityFrameworkCore;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Models.Cadastro;
using PlataformaWeb.Business.Extensions;
using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Data.Repositorio
{
    public class MovimentacaoAnimalRepositorio : Repositorio<MovimentacaoAnimal>, IMovimentacaoAnimalRepositorio
    {
        public MovimentacaoAnimalRepositorio(PlataformaFieldContext context, 
                                                IUser appUser) : base(context, appUser)
        {
        }

        public override Expression<Func<MovimentacaoAnimal, bool>> ObterWhere()
        {
            var where = base.ObterWhere();
            where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());

            return where;
        }


        public async Task<List<MovimentacaoAnimal>> ObterAnimaisMovimentacao(int idLocalOrigem, int idLocalDestino, int idLoteOrigem, int idLoteDestino, DateTime dataMovimentacao)
        {
            return await DbSet.AsNoTracking()
                            .Where(ObterWhere().And(x => x.IdLocalOrigem == idLocalOrigem && x.IdLocalDestino == idLocalDestino &&
                                        x.IdLoteOrigem == idLoteOrigem && x.IdLoteDestino == idLoteDestino && x.DataMovimentacao.Date.CompareTo(dataMovimentacao.Date) == 0))
                            .ToListAsync();
        }

        public async Task<MovimentacaoAnimalDTO> ObterMovimentacao(int idLocalOrigem, int idLocalDestino, int idLoteOrigem, int idLoteDestino, DateTime dataMovimentacao)
        {
            var result = await DbConnection.QueryAsync<MovimentacaoAnimalDTO>(
                "SELECT ma.IdLocalDestino, ma.IdLocalOrigem, ma.IdLoteDestino, ma.IdLoteOrigem, ma.datamov DataMovimentacao, min(ld.tipo) Tipo, " +
                        "COUNT(ma.IdLocalDestino) QuantidadeAnimais, min(ld.nome) LocalDestino, min(lo.nome) LocalOrigem, array_to_string(array_agg(distinct mot.nome), ',') Motivo " +
                "FROM movimentacaoanimal ma " +
                "INNER JOIN pastocurral lo ON lo.id = ma.idlocalorigem " +
                "INNER JOIN pastocurral ld ON ld.id = ma.idlocaldestino " +
                "INNER JOIN auxmotivomovimentacao mot on mot.id = ma.idmotivo " +
                "WHERE ma.status = 1 AND ma.idcliente = @idCliente AND idlocalorigem = @idLocalOrigem AND " +
                    " idlocaldestino = @idLocalDestino AND idloteorigem = @idLoteOrigem AND idlotedestino = @idLoteDestino AND " +
                    " datamov = @dataMovimentacao " +
                "GROUP BY 1,2,3,4,5 " +
                "ORDER BY ma.datamov DESC", new { idCliente = AppUser.ObterIdCliente(), idLocalOrigem, idLocalDestino, idLoteOrigem, idLoteDestino, dataMovimentacao });

            return result.FirstOrDefault();

            //return await DbSet.AsNoTracking()
            //                        .Include(x => x.LocalDestino)
            //                        .Include(x => x.LocalOrigem)
            //                        .Include(x => x.LoteDestino)
            //                        .Include(x => x.LoteOrigem)
            //                        .Include(x => x.Motivo)
            //                        .Where(ObterWhere().And(x => x.IdLocalOrigem == idLocalOrigem && x.IdLocalDestino == idLocalDestino &&
            //                                    x.IdLoteOrigem == idLoteOrigem && x.IdLoteDestino == idLoteDestino && x.DataMovimentacao.Date.CompareTo(dataMovimentacao.Date) == 0))
            //                        .GroupBy(x => new { x.IdLocalOrigem, x.IdLocalDestino, x.IdLoteOrigem, x.IdLoteDestino, x.DataMovimentacao })
            //                        .Select(x => new MovimentacaoAnimalCadastro
            //                        {
            //                            LocalDestino = x.Min(x => x.LocalDestino),
            //                            LocalOrigem = x.Min(x => x.LocalOrigem),
            //                            LoteOrigem = x.Min(x => x.LoteOrigem),
            //                            LoteDestino = x.Min(x => x.LoteDestino),
            //                            Motivo = x.Min(x => x.Motivo),
            //                            DataMovimentacao = x.Key.DataMovimentacao,
            //                            QuantidadeAnimais = x.Count()                                        
            //                        })
            //                        .FirstOrDefaultAsync();
        }

        //public async Task<MovimentacaoAnimal> ObterMovimentacaoComInclude(int idLocalOrigem, int idLocalDestino, int idLoteOrigem, int idLoteDestino, DateTime dataMovimentacao)
        //{
        //    return await DbSet.AsNoTracking()
        //                    .Include(x => x.LocalDestino)
        //                    .Include(x => x.LocalOrigem)
        //                    .Include(x => x.LoteDestino)
        //                    .Include(x => x.LoteOrigem)
        //                    .Include(x => x.Motivo)
        //                    .Where(ObterWhere().And(x => x.IdLocalOrigem == idLocalOrigem && x.IdLocalDestino == idLocalDestino &&
        //                                        x.IdLoteOrigem == idLoteOrigem && x.IdLoteDestino == idLoteDestino && x.DataMovimentacao.Date.CompareTo(dataMovimentacao.Date) == 0))
        //                    .FirstOrDefaultAsync();
        //}

        public async Task<List<MovimentacaoAnimalDTO>> ObterPaginacao()
        {
            var result = await DbConnection.QueryAsync<MovimentacaoAnimalDTO>(
                "SELECT ma.IdLocalDestino, ma.IdLocalOrigem, ma.IdLoteDestino, ma.IdLoteOrigem, ma.datamov DataMovimentacao, min(ld.tipo) Tipo, " +
                        "COUNT(ma.IdLocalDestino) QuantidadeAnimais, min(ld.nome) LocalDestino, min(lo.nome) LocalOrigem, array_to_string(array_agg(distinct mot.nome), ',') Motivo " +
                "FROM movimentacaoanimal ma " +
                "INNER JOIN pastocurral lo ON lo.id = ma.idlocalorigem " +
                "INNER JOIN pastocurral ld ON ld.id = ma.idlocaldestino " +
                "INNER JOIN auxmotivomovimentacao mot on mot.id = ma.idmotivo " +
                "WHERE ma.status = 1 AND ma.idcliente = @idCliente " +
                "GROUP BY 1,2,3,4,5 " +
                "ORDER BY ma.datamov DESC", new { idCliente = AppUser.ObterIdCliente() });

            return result.ToList();
        }
    }
}
