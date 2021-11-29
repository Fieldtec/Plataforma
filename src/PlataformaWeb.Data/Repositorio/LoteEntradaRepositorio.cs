using Microsoft.EntityFrameworkCore;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Models.Cadastro;
using PlataformaWeb.Business.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PlataformaWeb.Business.Enums;

namespace PlataformaWeb.Data.Repositorio
{
    public class LoteEntradaRepositorio : Repositorio<LoteEntrada>, ILoteEntradaRepositorio
    {
        public LoteEntradaRepositorio(PlataformaFieldContext context, IUser appUser)
            : base(context, appUser)
        {
        }

        public override Expression<Func<LoteEntrada, bool>> ObterWhere()
        {
            var where = base.ObterWhere();
            where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());

            return where;
        }

        public async override Task Atualizar(LoteEntrada entity)
        {
            await base.Atualizar(entity);

            foreach (var animal in entity.AnimaisLote)
            {
                if (animal.Id == 0)
                    Context.Entry(animal).State = EntityState.Added;
            }
        }

        public async Task<List<LoteAnimalDTO>> BuscarQuery(Expression<Func<LoteEntrada, bool>> predicate)
        {
            var lotes = await DbSet.AsNoTracking()
                               .Include(x => x.Local)
                               .Include(x => x.Planejamento)
                               .Where(ObterWhere().And(predicate))
                               .Select(x => new LoteAnimalDTO
                               {
                                   Id = x.Id,
                                   Planejamento = x.Planejamento.Nome,
                                   Local = x.Local.Nome,
                                   DataEntrada = x.DataEntrada,
                               })
                               .OrderByDescending(x => x.DataEntrada)
                               .ToListAsync();

            foreach (var lote in lotes)
            {
                lote.QuantidadeAnimais = await Context.Animais
                                                    .Where(x => x.IdLote == lote.Id && x.Status == Business.Enums.Status.Ativado)
                                                    .CountAsync();
            }

            return lotes;
        }

        public async override Task<LoteEntrada> ObterPorId(int id)
        {
            var lote = await DbSet.AsNoTracking()
                                .Where(ObterWhere().And(x => x.Id == id))
                                .FirstOrDefaultAsync();

            if (lote != null)
            {
                lote.AnimaisLote = await BuscarAnimais(lote.Id);
            }


            return lote;
        }


        //public async Task<LoteEntrada> ObterLotePorId(int id, Status statusAnimais)
        //{
        //    var lote = await DbSet.AsNoTracking()
        //                        .Where(ObterWhere().And(x => x.Id == id))
        //                        .FirstOrDefaultAsync();

        //    if (lote != null)
        //    {
        //        lote.AnimaisLote = await BuscarAnimais(lote.Id, statusAnimais);
        //    }


        //    return lote;
        //}

        public async Task<LoteAnimalCadastro> ObterLoteCadastroPorId(int id)
        {
            var lote = await DbSet.AsNoTracking()
                               .Include(x => x.Local)
                               .Include(x => x.Planejamento)
                               .Where(ObterWhere().And(x => x.Id == id))
                               .Select(x => new LoteAnimalCadastro
                               {
                                   Id = x.Id,
                                   Planejamento = x.Planejamento,
                                   Local = x.Local,
                                   DataEntrada = x.DataEntrada,
                                   Status = x.Status
                               })
                               .OrderByDescending(x => x.DataEntrada)
                               .FirstOrDefaultAsync();

            if (lote != null)
            {
                lote.QuantidadeAnimais = await BuscarQuantidadeAnimaisPorLocal(lote.Id);
                await BuscarInformacoesAnimaisLote(lote);


                lote.MovimentacoesNoLote = await ObterMovimentacoesNoLote(lote.Id);
            }


            return lote;
        }

        public async Task<List<LoteAnimalDTO>> ObterPaginacao()
        {
            var lotes = await DbSet.AsNoTracking()
                               .Include(x => x.Cliente)
                                    .ThenInclude(c => c.Tecnico)
                               .Include(x => x.Local)
                               .Include(x => x.Planejamento)
                               .Where(ObterWhere())
                               .Select(x => new LoteAnimalDTO
                               {
                                   Id = x.Id,
                                   Planejamento = x.Planejamento.Nome,
                                   Local = x.Local.Nome,
                                   DataEntrada = x.DataEntrada,
                                   Proprietario = x.Cliente.Nome,
                                   NomePropriedade = x.Cliente.NomePropriedade,
                                   Tecnico = x.Cliente.Tecnico.Nome,
                               }).ToListAsync();

            foreach (var lote in lotes)
            {
                lote.QuantidadeAnimais = await BuscarQuantidadeAnimaisPorLocal(lote.Id);
            }

            return lotes;
        }

        private async Task BuscarInformacoesAnimaisLote(LoteAnimalCadastro lote)
        {
            var animalNoLote = await Context.Animais
                                            .Include(x => x.ProdutorPaceiro)
                                            .Include(x => x.Raca)
                                            .Include(x => x.Categoria)
                                            .Where(x => x.IdLote == lote.Id && x.Status == Business.Enums.Status.Ativado)
                                            .Select(x => new LoteAnimalCadastro
                                            {
                                                Categoria = x.Categoria,
                                                Raca = x.Raca,
                                                PesoEntrada = x.PesoEntrada,
                                                IdadeEntrada = x.IdadeEntrada,
                                                ProdutorParceiro = x.ProdutorPaceiro,
                                                TipoEntrada = x.TipoEntrada,
                                                ValorCompra = x.ValorCompra
                                            }).FirstOrDefaultAsync();

            if (animalNoLote != null)
            {
                lote.Categoria = animalNoLote.Categoria;
                lote.Raca = animalNoLote.Raca;
                lote.PesoEntrada = animalNoLote.PesoEntrada;
                lote.IdadeEntrada = animalNoLote.IdadeEntrada;
                lote.ProdutorParceiro = animalNoLote.ProdutorParceiro;
                lote.TipoEntrada = animalNoLote.TipoEntrada;
                lote.ValorCompra = animalNoLote.ValorCompra;
            }
        }

        private async Task<List<Animal>> BuscarAnimais(int idLote, Status status = Status.Ativado)
        {
            //if (status == Status.Desativado)
            //    status = Status.Ativado;

            return await Context.Animais
                                .Where(x => x.IdLote == idLote && x.Status == status)
                                .ToListAsync();
        }

        private async Task<int> BuscarQuantidadeAnimaisPorLocal(int idLote)
        {
            return await Context.Animais
                                .Where(x => x.IdLote == idLote && x.Status == Business.Enums.Status.Ativado)
                                .CountAsync();
        }


        private async Task<List<MovimentacaoLoteEntrada>> ObterMovimentacoesNoLote(int idLote)
        {
            return await Context.Animais
                    .Include(x => x.ProdutorPaceiro)
                    .Where(X => X.IdLote == idLote)
                    .GroupBy(x => new { x.IdLote, x.DataEntrada, x.ProdutorPaceiro.Nome })
                    .Select(x => new MovimentacaoLoteEntrada
                    {
                        IdLote = x.Key.IdLote,
                        DataEntrada = x.Key.DataEntrada,
                        ProdutorOrigem = x.Key.Nome,
                        QuantidadeAnimais = x.Count(),
                        PesoMedio = x.Average(x => x.PesoEntrada)
                    })
                    .ToListAsync();
        }

        public async Task RealizarTransferencia(LoteEntrada lote, List<Animal> animais)
        {
            foreach (var animal in animais)
            {
                animal.IdLote = lote.Id;
                await Task.FromResult(Context.Animais.Update(animal));
            }
        }

        public async Task AtualizarAnimais(List<Animal> animais)
        {
            foreach (var animal in animais)
                await Task.FromResult(Context.Animais.Update(animal));
        }

        public async Task<List<MorteAnimalDTO>> ObterMortesPaginacao()
        {
            var result = await DbConnection.QueryAsync<MorteAnimalDTO>("SELECT a.DataMorte, " +
                "a.IdLote, COUNT(a.id) QuantidadeAnimais, array_to_string(array_agg(distinct c.nome), ',') CausaMorte, min(lo.nome) LocalOrigem " +
                "FROM animal a " +
                "INNER JOIN loteentrada l ON l.id = a.idlote " +
                "INNER JOIN pastocurral lo ON lo.id = l.idlocal " +
                "INNER JOIN auxcausamorte c ON c.id = a.idcausamorte " +
                "WHERE a.status = 3 AND l.idcliente = @idCliente " +
                "GROUP BY 1, 2 " +
                "ORDER BY a.datamorte DESC ", new { idCliente = AppUser.ObterIdCliente() });


            return result.ToList();
        }

        public async Task<List<Animal>> ObterAnimaisMortos(int idLote, DateTime dataMorte)
        {
            var where = PredicateBuilder.True<Animal>()
                                .And(x => x.Status == Business.Enums.Status.Morte)
                                .And(x => x.IdLote == idLote)
                                .And(x => x.DataMorte == dataMorte)
                                .And(x => x.LoteEntrada.IdCliente == AppUser.ObterIdCliente());

            return await Context.Animais
                        .Include(x => x.LoteEntrada)
                        .Where(where)
                        .ToListAsync();
        }

        public async Task<MorteAnimalDTO> ObterMorte(int idLote, DateTime dataMorte)
        {
            var result = await DbConnection.QueryAsync<MorteAnimalDTO>("SELECT a.DataMorte, " +
               "a.IdLote, COUNT(a.id) QuantidadeAnimais, array_to_string(array_agg(distinct c.nome), ',') CausaMorte, min(lo.nome) LocalOrigem " +
               "FROM animal a " +
               "INNER JOIN loteentrada l ON l.id = a.idlote " +
               "INNER JOIN pastocurral lo ON lo.id = l.idlocal " +
               "INNER JOIN auxcausamorte c ON c.id = a.idcausamorte " +
               "WHERE a.status = 3 AND l.idcliente = @idCliente AND a.idlote = @idLote AND a.datamorte = @dataMorte " +
               "GROUP BY 1, 2 " +
               "ORDER BY a.datamorte DESC ", new { idCliente = AppUser.ObterIdCliente(), idLote, dataMorte = dataMorte.Date });

            return result.FirstOrDefault();
        }

        public async Task<LoteEntrada> ObterLotePorLocal(int idLocal)
        {
            return await DbSet.AsNoTracking()
                            .Where(ObterWhere().And(x => x.IdLocal == idLocal))
                            .OrderByDescending(x => x.DataEntrada)
                            .FirstOrDefaultAsync();
        }

        public async Task<LoteEntrada> ObterPorIdSimplicado(int id)
        {
            return await DbSet.AsNoTracking()
                            .Where(ObterWhere().And(x => x.Id == id))
                            .FirstOrDefaultAsync();
        }

        public async Task<int> ObterQuantidadeNoLoteNaData(int id, DateTime data)
        {
            var result = await DbConnection.ExecuteScalarAsync<int>("SELECT " +
                "   count(*) " +
                " FROM animal " +
                " WHERE idLote= @id AND status > 0 AND dataentrada <= @data " +
                "   and id not in (SELECT id FROM animal " +
                "       WHERE idLote = @id and status in (2,3) and datasaida< @data) ", new { id, data = data.Date });

            return result;
        }

        public async Task<decimal> ObterPesoMedio(int idLote)
        {
            var result = await DbConnection.ExecuteScalarAsync<decimal>("SELECT " +
                "   avg(pesoentrada) " +
                " FROM animal " +
                " WHERE idLote= @id AND status > 0 " +
                "   and id not in (SELECT id FROM animal " +
                "       WHERE idLote = @id and status in (2,3))", new { id = idLote });

            return result;
        }
    }
}
