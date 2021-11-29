using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Services
{
    public interface ILeituraCochoService
    {
        Task<List<LeituraCochoDTO>> ObterTodosFiltro(FiltroLeituraCochoDTO filtro);
        Task<List<LeituraCochoInsercaoDTO>> ObterLeiturasInsercao(DateTime dataLeitura);
        Task InserirLeitura(List<LeituraCochoInsercaoDTO> models);
    }
}
