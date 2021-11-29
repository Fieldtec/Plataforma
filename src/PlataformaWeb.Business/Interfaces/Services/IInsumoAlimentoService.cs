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
    public interface IInsumoAlimentoService : IBaseService<InsumoAlimento, InsumoAlimentoDTO>
    {
        Task<List<LogAtualizacaoMaterialSecaDTO>> ObterLogsAlteracao(int id);
        Task<IEnumerable<InsumoAlimentoDTO>> Buscar(Expression<Func<InsumoAlimento, bool>> predicate);
        Task AtualizarMateriaSeca(InsumoAlimento insumoAlimento);
    }
}
