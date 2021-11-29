using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IMotivoMovimentacaoRepositorio : IRepositorio<MotivoMovimentacao>
    {
        Task<List<MotivoMovimentacaoDTO>> ObterPaginacao();
        Task<IEnumerable<MotivoMovimentacaoDTO>> BuscarQuery(Expression<Func<MotivoMovimentacao, bool>> predicate);
    }

}
