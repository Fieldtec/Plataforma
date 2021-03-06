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
    public interface ICategoriaService : IBaseService<Categoria, CategoriaDTO>
    {
        Task<IEnumerable<CategoriaDTO>> Buscar(Expression<Func<Categoria, bool>> predicate);
    }
}
