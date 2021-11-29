using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Models.Cadastro;
using PlataformaWeb.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Services
{
    public interface ILoteEntradaService : IBaseService<LoteAnimalCadastro, LoteAnimalDTO>
    {
        Task<List<LoteAnimalDTO>> Buscar(Expression<Func<LoteEntrada, bool>> predicate);

        Task<List<MorteAnimalDTO>> ObterMortesPaginacao();
        Task<MorteAnimalDTO> ObterMorte(int idLote, DateTime dataMorte);
        Task RegistrarMortes(MorteAnimalCadastro morte);
        Task ExcluirMorte(MorteAnimalDTO morte);
    }
}
