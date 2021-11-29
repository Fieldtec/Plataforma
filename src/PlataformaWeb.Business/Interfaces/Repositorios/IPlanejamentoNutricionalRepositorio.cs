using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IPlanejamentoNutricionalRepositorio : IRepositorio<PlanejamentoNutricional>
    {
        Task<List<PlanejamentoNutricionalDTO>> ObterPaginacao();
        Task<List<PlanejamentoNutricionalDTO>> BuscarQuery(Expression<Func<PlanejamentoNutricional, bool>> predicate);
        Task<List<GerenciarPlanejamentoLoteDTO>> BuscarLotesGerenciamento(int idPlanejamento);
        Task<List<PlanejamentoValoresPasto>> BuscarPlanejamentoContemSuplemento(int idSuplemento);
    }
}
