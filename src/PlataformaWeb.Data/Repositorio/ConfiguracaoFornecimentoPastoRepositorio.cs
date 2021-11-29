using Microsoft.EntityFrameworkCore;
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
    public class ConfiguracaoFornecimentoPastoRepositorio : Repositorio<ConfiguracaoFornecimentoPasto>, IConfiguracaoFornecimentoPastoRepositorio
    {
        public ConfiguracaoFornecimentoPastoRepositorio(PlataformaFieldContext context, IUser appUser) : base(context, appUser)
        {
        }

        public override Expression<Func<ConfiguracaoFornecimentoPasto, bool>> ObterWhere()
        {
            var where = base.ObterWhere();            
            where = where.And(x => x.IdCliente == AppUser.ObterIdCliente());
            return where;
        }

        public async override Task<List<ConfiguracaoFornecimentoPasto>> ObterTodos()
        {
            return await DbSet.Where(ObterWhere()).ToListAsync();
        }

    }

}
