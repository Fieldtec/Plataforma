using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IRacaRepositorio : IRepositorio<Raca>
    {
        Task<List<RacaDTO>> ObterPaginacao(int? idCliente = null);
        Task<List<RacaDTO>> BuscarQuery(Expression<Func<Raca, bool>> predicate);
    }
}
