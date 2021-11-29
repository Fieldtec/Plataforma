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
    public interface IProdutorParceiroService : IBaseService<ProdutorParceiro, ProdutorParceiroDTO>
    {
        Task<List<ProdutorParceiroDTO>> Buscar(Expression<Func<ProdutorParceiro, bool>> predicate);
    }
}
