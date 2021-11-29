using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Services
{
    public interface IPrevisaoFornecimentoPastoService : IBaseService<PrevisaoFornecimentoPasto, PrevisaoFornecimentoPastoDTO>
    {
        Task<List<PrevisaoFornecimentoPastoDTO>> Buscar(FiltroPrevisaoFornecimentoPastoDTO filtro);
        Task GerarPrevisoes(GeracaoFornecimentoPastoDTO model);
        Task RemoverPrevisoes(List<PrevisaoFornecimentoPastoDTO> models);
        Task<ConfiguracaoFornecimentoPasto> ObterConfiguracaoFornecimento();
    }
}
