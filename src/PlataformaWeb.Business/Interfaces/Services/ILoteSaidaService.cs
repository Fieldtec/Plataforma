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
    public interface ILoteSaidaService : IBaseService<LoteSaida, LoteSaidaDTO>
    {
        Task<List<LoteSaidaDTO>> Buscar(Expression<Func<LoteSaida, bool>> predicate);
        Task LancarSaida(SaidaAnimalCadastro saida);
        Task AtualizarSaida(SaidaAnimalCadastro saida);
        Task ExcluirSaida(SaidaAnimalCadastro saida);
        Task<SaidaAnimalCadastro> ObterSaidaAnimal(int id);
    }
}
