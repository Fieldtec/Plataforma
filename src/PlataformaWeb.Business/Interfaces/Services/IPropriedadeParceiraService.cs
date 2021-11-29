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
    public interface IPropriedadeParceiraService : IBaseService<PropriedadeParceira, PropriedadeParceiraDTO>
    {
        Task<IEnumerable<PropriedadeParceira>> Buscar(Expression<Func<PropriedadeParceira, bool>> predicate);
    }
}
