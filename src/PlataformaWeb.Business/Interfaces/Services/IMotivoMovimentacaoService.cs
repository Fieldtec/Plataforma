using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Services
{
    public interface IMotivoMovimentacaoService : IBaseService<MotivoMovimentacao, MotivoMovimentacaoDTO>
    {
        Task<IEnumerable<MotivoMovimentacaoDTO>> Buscar(Expression<Func<MotivoMovimentacao, bool>> predicate);
    }
}
