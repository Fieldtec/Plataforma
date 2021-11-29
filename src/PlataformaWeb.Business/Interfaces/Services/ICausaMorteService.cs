using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Services
{
    public interface ICausaMorteService : IBaseService<CausaMorte, CausaMorteDTO>
    {
        Task<IEnumerable<CausaMorteDTO>> Buscar(Expression<Func<CausaMorte, bool>> predicate);
    }
}
