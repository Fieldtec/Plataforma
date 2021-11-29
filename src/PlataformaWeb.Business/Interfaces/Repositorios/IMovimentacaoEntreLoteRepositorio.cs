using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IMovimentacaoEntreLoteRepositorio : IRepositorio<MovimentacaoEntreLote>
    {
        Task<List<MovimentacaoEntreLoteDTO>> ObterPaginacao();
        Task<IEnumerable<MovimentacaoEntreLoteDTO>> BuscarQuery(Expression<Func<MovimentacaoEntreLote, bool>> predicate);
    }

}
