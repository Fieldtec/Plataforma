using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Models.Cadastro;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IMovimentacaoAnimalRepositorio : IRepositorio<MovimentacaoAnimal>
    {
        Task<List<MovimentacaoAnimalDTO>> ObterPaginacao();
        Task<MovimentacaoAnimalDTO> ObterMovimentacao(int idLocalOrigem, int idLocalDestino, int idLoteOrigem, int idLoteDestino, DateTime dataMovimentacao);
        Task<List<MovimentacaoAnimal>> ObterAnimaisMovimentacao(int idLocalOrigem, int idLocalDestino, int idLoteOrigem, int idLoteDestino, DateTime dataMovimentacao);

    }
}
