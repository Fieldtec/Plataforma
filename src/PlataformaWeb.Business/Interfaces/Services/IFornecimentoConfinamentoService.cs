using PlataformaWeb.Business.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Services
{
    public interface IFornecimentoConfinamentoService
    {
        Task<List<FornecimentoConfinamentoDTO>> ObterTodosFiltro(FiltroFornecimentoConfinamentoDTO filtro);
        Task<List<FornecimentoConfinamentoDTO>> ObterPorData(DateTime dataFornecimento);
        Task InserirKgRealizado(List<FornecimentoConfinamentoDTO> models);
        Task RemoverForncimentos(List<FornecimentoConfinamentoDTO> models);
        Task<FornecimentoConfinamentoDTO> RecalcularPrevisto(int id);
    }
}
