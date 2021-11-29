using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IFornecimentoPastoRepositorio : IRepositorio<FornecimentoPasto>
    {
        Task<List<FornecimentoPastoDTO>> BuscarQuery(FiltroFornecimentoPastoDTO filtro);
        Task<FornecimentoPasto> BuscarUltimoFornecimento(int idLote, int idPasto, int idSuplemento);
    }
}
