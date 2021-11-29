using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Models.Cadastro;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface ILoteEntradaRepositorio : IRepositorio<LoteEntrada>
    {
        Task<LoteEntrada> ObterPorIdSimplicado(int id);
        Task<List<LoteAnimalDTO>> ObterPaginacao();
        Task<List<LoteAnimalDTO>> BuscarQuery(Expression<Func<LoteEntrada, bool>> predicate);
        Task<LoteAnimalCadastro> ObterLoteCadastroPorId(int id);
        Task RealizarTransferencia(LoteEntrada lote, List<Animal> animais);
        Task AtualizarAnimais(List<Animal> animais);
        Task<List<MorteAnimalDTO>> ObterMortesPaginacao();
        Task<MorteAnimalDTO> ObterMorte(int idLote, DateTime dataMorte);
        Task<List<Animal>> ObterAnimaisMortos(int idLote, DateTime dataMorte);
        Task<LoteEntrada> ObterLotePorLocal(int idLocal);
        Task<int> ObterQuantidadeNoLoteNaData(int id, DateTime data);
        Task<decimal> ObterPesoMedio(int idLote);
        //Task<LoteEntrada> BuscarPorPlanejamentos(List<int> idsPlanejamento);
    }
}
