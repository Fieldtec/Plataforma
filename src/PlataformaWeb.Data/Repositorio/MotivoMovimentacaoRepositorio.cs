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
    public class MotivoMovimentacaoRepositorio : Repositorio<MotivoMovimentacao>, IMotivoMovimentacaoRepositorio
    {
        public MotivoMovimentacaoRepositorio(PlataformaFieldContext context,
                                    IUser appUser) : base(context, appUser)
        {
        }

        public override Expression<Func<MotivoMovimentacao, bool>> ObterWhere()
        {
            var where = base.ObterWhere();

            //if (!AppUser.EhAdmin())
            where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());

            return where;
        }


        public async override Task<MotivoMovimentacao> ObterPorId(int id)
        {
            return await DbSet.AsNoTracking()
                              .Where(ObterWhere().And(x => x.Id == id))
                              .FirstOrDefaultAsync();
        }


        public async Task<List<MotivoMovimentacaoDTO>> ObterPaginacao()
        {
            return await DbSet.AsNoTracking()
                               .Include(x => x.Cliente)
                                    .ThenInclude(c => c.Tecnico)
                              .Where(ObterWhere())
                              .Select(x => new MotivoMovimentacaoDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  Proprietario = x.Cliente.Nome,
                                  NomePropriedade = x.Cliente.NomePropriedade,
                                  Tecnico = x.Cliente.Tecnico.Nome,
                              }).ToListAsync();
        }

        public async Task<IEnumerable<MotivoMovimentacaoDTO>> BuscarQuery(Expression<Func<MotivoMovimentacao, bool>> predicate)
        {
            return await DbSet.AsNoTracking()
                              .Where(ObterWhere().And(predicate))
                              .Select(x => new MotivoMovimentacaoDTO
                              {
                                  Id = x.Id,
                                  Nome = x.Nome,
                              }).ToListAsync();
        }
    }

}
