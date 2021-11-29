using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IPrevisaoFornecimentoPastoRepositorio : IRepositorio<PrevisaoFornecimentoPasto>
    {
        Task<List<PrevisaoFornecimentoPastoDTO>> ObterPaginacao();
        Task<List<PrevisaoFornecimentoPastoDTO>> BuscarQuery(FiltroPrevisaoFornecimentoPastoDTO filtro);
    }
}
