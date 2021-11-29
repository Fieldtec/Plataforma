using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface ILeituraCochoRepositorio : IRepositorio<LeituraCocho>
    {
        Task<List<LeituraCochoDTO>> ObterTodosFiltro(FiltroLeituraCochoDTO filtro);
        Task<List<LeituraCochoInsercaoDTO>> ObterLeiturasInsercao(DateTime dataLeitura);
    }
}
