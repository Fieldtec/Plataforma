using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Services
{
    public interface IMovimentacaoEntreLoteService : IBaseService<MovimentacaoEntreLote, MovimentacaoEntreLoteDTO>
    {
        Task<IEnumerable<MovimentacaoEntreLoteDTO>> Buscar(Expression<Func<MovimentacaoEntreLote, bool>> predicate);
    }
}
