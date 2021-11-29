using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface ISuplementoMineralRepositorio : IRepositorio<SuplementoMineral>
    {
        Task<List<SuplementoMineralDTO>> ObterPaginacao();
        Task<List<SuplementoMineralDTO>> BuscarQuery(Expression<Func<SuplementoMineral, bool>> predicate);
    }
}
