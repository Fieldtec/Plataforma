using Microsoft.EntityFrameworkCore;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

namespace PlataformaWeb.Data.Repositorio
{
    public class MovimentacaoEntreLoteRepositorio : Repositorio<MovimentacaoEntreLote>, IMovimentacaoEntreLoteRepositorio
    {
        public MovimentacaoEntreLoteRepositorio(PlataformaFieldContext context,
                                                IUser appUser) : base(context, appUser)
        {
        }

        public override Expression<Func<MovimentacaoEntreLote, bool>> ObterWhere()
        {
            var where = base.ObterWhere();
            where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());

            return where;
        }

        public async override Task<MovimentacaoEntreLote> ObterPorId(int id)
        {
            return await DbSet.AsNoTracking()
                              .Include(x => x.LocalDestino)
                              .Include(x => x.LocalOrigem)
                              .Include(x => x.Motivo)
                              .Include(x => x.LoteEntrada)
                              .Where(ObterWhere().And(x => x.Id == id))
                              .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MovimentacaoEntreLoteDTO>> BuscarQuery(Expression<Func<MovimentacaoEntreLote, bool>> predicate)
        {
            return await DbSet.AsNoTracking()
                              .Include(x => x.LocalDestino)
                              .Include(x => x.LocalOrigem)
                              .Include(x => x.Motivo)
                              .Include(x => x.LoteEntrada)
                              .Include(x => x.Cliente)
                                .ThenInclude(x => x.Tecnico)
                              .Where(ObterWhere().And(predicate))
                              .Select(x => new MovimentacaoEntreLoteDTO
                              {
                                  Id = x.Id,
                                  LocalDestino = x.LocalDestino.Nome,
                                  LocalOrigem = x.LocalOrigem.Nome,
                                  Motivo = x.Motivo.Nome,
                                  DataLote = x.LoteEntrada.DataEntrada,
                                  QuantidadeAnimais = x.QuantidadeAnimais,
                                  DataMovimentacao = x.DataMovimentacao,
                                  NomePropriedade = x.Cliente.NomePropriedade,
                                  Proprietario = x.Cliente.Nome,
                                  Tecnico = x.Cliente.Tecnico.Nome
                              })
                              .OrderByDescending(x => x.DataMovimentacao)
                              .ToListAsync();
        }

        public async Task<List<MovimentacaoEntreLoteDTO>> ObterPaginacao()
        {
            return await DbSet.AsNoTracking()
                              .Include(x => x.LocalDestino)
                              .Include(x => x.LocalOrigem)
                              .Include(x => x.Motivo)
                              .Include(x => x.LoteEntrada)
                              .Include(x => x.Cliente)
                                .ThenInclude(x => x.Tecnico)
                              .Where(ObterWhere())
                              .Select(x => new MovimentacaoEntreLoteDTO
                              {
                                  Id = x.Id,
                                  LocalDestino = x.LocalDestino.Nome,
                                  LocalOrigem = x.LocalOrigem.Nome,
                                  Motivo = x.Motivo.Nome,
                                  DataLote = x.LoteEntrada.DataEntrada,
                                  QuantidadeAnimais = x.QuantidadeAnimais,
                                  DataMovimentacao = x.DataMovimentacao,
                                  NomePropriedade = x.Cliente.NomePropriedade,
                                  Proprietario = x.Cliente.Nome,
                                  Tecnico = x.Cliente.Tecnico.Nome
                              })
                              .OrderByDescending(x => x.DataMovimentacao)
                              .ToListAsync();
        }
    }

}
