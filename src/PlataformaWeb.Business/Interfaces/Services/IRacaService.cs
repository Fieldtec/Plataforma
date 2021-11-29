using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Services
{
    public interface IRacaService : IBaseService<Raca, RacaDTO>
    {
        Task<List<RacaDTO>> Buscar(Expression<Func<Raca, bool>> predicate);
    }
}
