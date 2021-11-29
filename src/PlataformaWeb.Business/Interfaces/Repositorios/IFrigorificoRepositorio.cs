using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IFrigorificoRepositorio : IRepositorio<Frigorifico>
    {
        Task<List<FrigorificoDTO>> ObterPaginacao();
        Task<IEnumerable<FrigorificoDTO>> BuscarQuery(Expression<Func<Frigorifico, bool>> predicate);
    }

}
