using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Services
{
    public interface IPlanejamentoNutricionalService : IBaseService<PlanejamentoNutricional, PlanejamentoNutricionalDTO>
    {
        Task<List<PlanejamentoNutricionalDTO>> Buscar(Expression<Func<PlanejamentoNutricional, bool>> predicate);
        Task<List<GerenciarPlanejamentoLoteDTO>> BuscarLotesGerenciamento(int idPlanejamento);
        Task AlterarPlanejamentosNosLotes(List<GerenciarPlanejamentoDTO> planejamentosGerenciamento);
    }

}
