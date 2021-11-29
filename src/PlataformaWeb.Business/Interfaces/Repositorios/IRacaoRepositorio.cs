using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IRacaoRepositorio : IRepositorio<Racao>
    {
        Task<List<RacaoDTO>> ObterPaginacao();
        Task<RacaoInsumo> ObterRacaoInsumo(int idRacaoInsumo);
        Task RemoverInsumo(RacaoInsumo racaoInsumo);
        Task<List<RacaoDTO>> BuscarQuery(Expression<Func<Racao, bool>> predicate);
        Task<List<Racao>> ObterRacoesContemInsumo(int idInsumo);
    }
}
