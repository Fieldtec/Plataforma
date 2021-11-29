using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IFornecimentoConfinamentoRepositorio : IRepositorio<FornecimentoConfinamento>
    {
        Task<bool> ExisteFornecimento(int idLocal, DateTime dataFornecimento);
        Task<List<FornecimentoConfinamentoDTO>> ObterTodosFiltro(FiltroFornecimentoConfinamentoDTO filtro);
        Task<List<FornecimentoConfinamentoDTO>> ObterPorData(DateTime dataFornecimento);
        Task<bool> EhPrimeiroFornecimentoDoLote(int id, DateTime dataFornecimento);
    }
}
