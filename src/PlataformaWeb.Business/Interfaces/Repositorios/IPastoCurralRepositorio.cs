using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IPastoCurralRepositorio : IRepositorio<PastoCurral>
    {
        Task<List<PastoCurralDTO>> ObterPaginacao(int? idCliente = null);
        Task<List<PastoCurralDTO>> BuscarQuery(Expression<Func<PastoCurral, bool>> predicate);
        Task<bool> ExisteLoteAtivo(int id);
        Task<List<LocalLoteDTO>> BuscarLocaisAtivos();
    }
}
