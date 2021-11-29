using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Models.Cadastro;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface ILoteSaidaRepositorio : IRepositorio<LoteSaida>
    {
        Task<List<LoteSaidaDTO>> ObterPaginacao();
        Task<List<LoteSaidaDTO>> BuscarQuery(Expression<Func<LoteSaida, bool>> predicate);
        Task<int> ObterProximoNumeroLote();
        Task<int> ObterQuantidadeAnimaisNoLoteEntradaSaida(int idLote, int idLoteSaida);
        Task<List<Animal>> ObterAnimaisNoLote(int idLoteSaida);
        Task<SaidaAnimalCadastro> ObterSaidaAnimal(int id);
    }
}
