using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface ICategoriaRepositorio : IRepositorio<Categoria>
    {
        Task<List<CategoriaDTO>> ObterPaginacao(int? idCliente = null);
        Task<IEnumerable<CategoriaDTO>> BuscarQuery(Expression<Func<Categoria, bool>> predicate);
    }
}
