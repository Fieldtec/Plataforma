using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Services
{
    public interface IFornecimentoPastoService : IBaseService<FornecimentoPasto, FornecimentoPastoDTO>
    {
        Task<List<FornecimentoPastoDTO>> Buscar(FiltroFornecimentoPastoDTO filtro);
        Task<PreparaDadosFornecimentoPastoDTO> BuscarDadosLancamento(FiltroFornecimentoPastoDTO filtro);
        Task Remover(List<FornecimentoPastoDTO> models);
    }
}
