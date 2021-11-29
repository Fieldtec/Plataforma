using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IProdutorParceiroRepositorio : IRepositorio<ProdutorParceiro>
    {
        Task<List<ProdutorParceiroDTO>> ObterPaginacao(int? idCliente = null);
        Task<List<ProdutorParceiroDTO>> BuscarQuery(Expression<Func<ProdutorParceiro, bool>> predicate);
    }
}
