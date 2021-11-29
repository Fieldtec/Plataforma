using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class ConfiguracaoFornecimentoPastoService : Service, IConfiguracaoFornecimentoPastoService
    {
        public ConfiguracaoFornecimentoPastoService(INotificador notificador, IUser appUser) : base(notificador, appUser)
        {
        }

        public Task Adicionar(ConfiguracaoFornecimentoPasto entity)
        {
            throw new NotImplementedException();
        }

        public Task Atualizar(ConfiguracaoFornecimentoPasto entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConfiguracaoFornecimentoPasto>> Buscar(Expression<Func<ConfiguracaoFornecimentoPasto, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<ConfiguracaoFornecimentoPasto> ObterPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ConfiguracaoFornecimentoPasto>> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public Task Remover(int id)
        {
            throw new NotImplementedException();
        }
    }
}
