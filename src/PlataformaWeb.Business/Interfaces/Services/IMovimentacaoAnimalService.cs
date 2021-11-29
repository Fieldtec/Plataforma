using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Models.Cadastro;
using PlataformaWeb.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Services
{
    public interface IMovimentacaoAnimalService 
    {
        Task<List<MovimentacaoAnimalDTO>> ObterPaginacao();
        Task Adicionar(MovimentacaoAnimalCadastro entity);
        Task Remover(int idLocalOrigem, int idLocalDestino, int idLoteOrigem, int idLoteDestino, DateTime dataMovimentacao);
        Task<MovimentacaoAnimalDTO> ObterMovimentacao(int idLocalOrigem, int idLocalDestino, int idLoteOrigem, int idLoteDestino, DateTime dataMovimentacao);
    }

}
