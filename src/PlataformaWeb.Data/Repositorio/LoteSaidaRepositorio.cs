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
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlataformaWeb.Business.Enums;
using Dapper;
using PlataformaWeb.Business.Models.Cadastro;

namespace PlataformaWeb.Data.Repositorio
{
    public class LoteSaidaRepositorio : Repositorio<LoteSaida>, ILoteSaidaRepositorio
    {
        public LoteSaidaRepositorio(PlataformaFieldContext context, IUser appUser) : base(context, appUser)
        {
        }

        public override Expression<Func<LoteSaida, bool>> ObterWhere()
        {
            return base.ObterWhere()
                .And(x => x.IdCliente == AppUser.ObterIdCliente());            
        }

        public async override Task<LoteSaida> ObterPorId(int id)
        {
            return await DbSet.AsNoTracking()
                            .Include(x => x.ProdutorDestino)
                            .Include(x => x.FrigorificoDestino)
                            .Where(ObterWhere().And(x => x.Id == id))
                            .FirstOrDefaultAsync();
        }

        public async Task<List<LoteSaidaDTO>> BuscarQuery(Expression<Func<LoteSaida, bool>> predicate)
        {
            return await DbSet.AsNoTracking()
                                .Include(x => x.Cliente)
                                    .ThenInclude(x => x.Tecnico)
                                .Include(x => x.FrigorificoDestino)
                                .Include(x => x.ProdutorDestino)
                            .Where(ObterWhere().And(predicate))
                            .Select(x => new LoteSaidaDTO
                            {
                                DataEmbarque = x.DataEmbarque,
                                NumeroLote = x.NumeroLote,
                                QuantidadeAnimalEmbarcado = x.QuantidadeAnimaEmbarcado,
                                QuantidadeAnimalPrevista = x.QuantidadeAnimalPrevisto,
                                Id = x.Id,
                                TipoSaida = x.TipoSaida,
                                ProdutorFrigorificoDestino = x.TipoSaida == TipoSaida.Abate ? x.FrigorificoDestino.Nome : x.ProdutorDestino.Nome,
                                Tecnico = x.Cliente.Tecnico.Nome,
                                NomePropriedade = x.Cliente.NomePropriedade,
                                Proprietario = x.Cliente.Nome
                            })
                            .ToListAsync();
        }

        public async Task<List<LoteSaidaDTO>> ObterPaginacao()
        {
            return await DbSet.AsNoTracking()
                                .Include(x => x.Cliente)
                                    .ThenInclude(x => x.Tecnico)
                                .Include(x => x.ProdutorDestino)
                                .Include(x => x.FrigorificoDestino)
                            .Where(ObterWhere())
                            .Select(x => new LoteSaidaDTO
                            { 
                                DataEmbarque = x.DataEmbarque,
                                NumeroLote = x.NumeroLote,
                                QuantidadeAnimalEmbarcado = x.QuantidadeAnimaEmbarcado,
                                QuantidadeAnimalPrevista = x.QuantidadeAnimalPrevisto,
                                Id = x.Id,
                                TipoSaida = x.TipoSaida,
                                ProdutorFrigorificoDestino = x.TipoSaida == TipoSaida.Abate ? x.FrigorificoDestino.Nome : x.ProdutorDestino.Nome,
                                Tecnico = x.Cliente.Tecnico.Nome,
                                NomePropriedade = x.Cliente.NomePropriedade,
                                Proprietario = x.Cliente.Nome
                            })
                            .ToListAsync();
        }

        public async Task<int> ObterProximoNumeroLote()
        {
            var count = await DbConnection.ExecuteScalarAsync<int?>("" +
                "SELECT max(numerolote) FROM lotesaida WHERE idcliente = @idCliente",
                new { idCliente = AppUser.ObterIdCliente() });

            if (!count.HasValue)
                return 1;

            return count.Value + 1;
        }

        public async Task<int> ObterQuantidadeAnimaisNoLoteEntradaSaida(int idLote, int idLoteSaida)
        {
            return await Context.Animais.AsNoTracking()
                                .Where(x => x.IdLote == idLote && x.IdLoteSaida == idLoteSaida && x.Status == Status.Fechado)
                                .CountAsync();
        }

        public async Task<List<Animal>> ObterAnimaisNoLote(int idLoteSaida)
        {
            return await Context.Animais.AsNoTracking()
                                        .Where(x => x.IdLoteSaida == idLoteSaida)
                                        .ToListAsync();
        }

        public async Task<SaidaAnimalCadastro> ObterSaidaAnimal(int id)
        {
            var resultado = await DbSet.AsNoTracking()
                                    .Include(x => x.FrigorificoDestino)
                                    .Include(x => x.ProdutorDestino)
                                    .Where(ObterWhere().And(x => x.Id == id))
                                    .Select(x => new SaidaAnimalCadastro
                                    {
                                        DataEmbarque = x.DataEmbarque,
                                        NumeroLote = x.NumeroLote,
                                        QuantidadeAnimalPrevisto = x.QuantidadeAnimalPrevisto,
                                        QuantidadeAnimalEmbarcado = x.QuantidadeAnimaEmbarcado,
                                        TipoSaida = x.TipoSaida,
                                        Destino = x.TipoSaida == TipoSaida.Abate ? x.FrigorificoDestino.Nome : x.ProdutorDestino.Nome,
                                        Id = x.Id
                                    }).FirstOrDefaultAsync();


            if (resultado != null)
            {
                var lotesQuantidades = await Context.Animais.AsNoTracking()
                                                .Where(x => x.IdLoteSaida == resultado.Id)
                                                .GroupBy(x => x.IdLote)
                                                .Select(x => new
                                                {
                                                    Lote = x.Key,
                                                    QuantidadeAnimais = x.Count(),
                                                    PesoSaida = x.Average(x => x.PesoSaida)
                                                })
                                                .ToListAsync();

                foreach (var item in lotesQuantidades)
                {
                    var loteEntrada = await Context.LotesEntradas.AsNoTracking().Where(x => x.Id == item.Lote).FirstOrDefaultAsync();
                    var local = await Context.Pastocurral.AsNoTracking().Where(x => x.Id == loteEntrada.IdLocal).FirstOrDefaultAsync();

                    resultado.Lotes.Add(new SaidaAnimalLote
                    {
                        Local = local,
                        Lote = loteEntrada,
                        PesoMedio = item.PesoSaida.Value,
                        QuantidadeEmbarcado = item.QuantidadeAnimais
                    });
                }
            }

            return resultado;
        }
    }
}
