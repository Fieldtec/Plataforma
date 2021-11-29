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
    public interface IRacaoService : IBaseService<Racao, RacaoDTO>
    {
        bool CalcularValores(Racao model);
        Task RemoverInsumo(Racao model, int idRacaoInsumo);
        Task<List<RacaoDTO>> Buscar(Expression<Func<Racao, bool>> predicate);
    }

}
