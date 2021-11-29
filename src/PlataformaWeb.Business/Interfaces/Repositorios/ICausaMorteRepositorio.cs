using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface ICausaMorteRepositorio : IRepositorio<CausaMorte>
    {
        Task<List<CausaMorteDTO>> ObterPaginacao();
        Task<IEnumerable<CausaMorteDTO>> BuscarQuery(Expression<Func<CausaMorte, bool>> predicate);
    }

}
