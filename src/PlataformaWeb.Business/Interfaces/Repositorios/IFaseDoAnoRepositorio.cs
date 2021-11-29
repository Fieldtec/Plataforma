using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IFaseDoAnoRepositorio : IRepositorio<FaseDoAno>
    {
        Task<List<FaseDoAnoDTO>> ObterPaginacao();
        Task<List<FaseDoAno>> ObterFaseNoPeriodo(FaseDoAno fase);
        Task<IEnumerable<FaseDoAnoDTO>> BuscarQuery(Expression<Func<FaseDoAno, bool>> predicate);
    }
}
