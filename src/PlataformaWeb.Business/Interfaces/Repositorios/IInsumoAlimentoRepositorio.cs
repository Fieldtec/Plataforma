using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IInsumoAlimentoRepositorio : IRepositorio<InsumoAlimento>
    {
        Task<List<InsumoAlimentoDTO>> ObterPaginacao();
        Task<IEnumerable<InsumoAlimentoDTO>> BuscarQuery(Expression<Func<InsumoAlimento, bool>> predicate);
        Task AtualizarMateriaSeca(InsumoAlimento insumo, decimal materiaSecaAnterio);
        Task<List<LogAtualizacaoMaterialSecaDTO>> ObterLogsAlteracao(int id);
    }
}
