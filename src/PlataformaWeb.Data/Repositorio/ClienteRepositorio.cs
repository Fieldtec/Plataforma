using Microsoft.EntityFrameworkCore;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Enums;
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
    public class ClienteRepositorio : Repositorio<Cliente>, IClienteRepositorio
    {

        public ClienteRepositorio(PlataformaFieldContext context, IUser appUser)
            : base(context, appUser)
        {
        }

        public override Expression<Func<Cliente, bool>> ObterWhere()
        {
            var where = base.ObterWhere();

            if (AppUser.EhTecnico())
                where = where.And(x => x.IdTecnico == AppUser.ObterId());

            return where;
        }


        public async override Task<Cliente> ObterPorId(int id)
        {
            return await DbSet.Where(ObterWhere().And(x => x.Id == id))
                              .SingleOrDefaultAsync();
        }

        public async Task<List<ClienteDTO>> ObterPaginacao()
        {
            return await DbSet.AsNoTracking().Include(x => x.Tecnico).Where(ObterWhere())
                .Select(model => new ClienteDTO
                {
                    Id = model.Id,
                    Propriedade = model.NomePropriedade,
                    Nome = model.Nome,
                    Municipio = model.Cidade,
                    Uf = model.Uf,
                    Status = model.Status,
                    Tecnico = model.Tecnico.Nome
                })
                .ToListAsync();
        }

        public async Task<DateTime?> ObterDataValidadeLicenca(int id)
        {
            return await DbSet.Where(x => x.Id == id).Select(x => x.DataValidadeLicenca).SingleOrDefaultAsync();
        }
    }
}
