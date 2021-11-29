using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Services
{
    public interface IFrigorificoService : IBaseService<Frigorifico, FrigorificoDTO>
    {
        Task<IEnumerable<FrigorificoDTO>> Buscar(Expression<Func<Frigorifico, bool>> predicate);
    }
}
