using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface ITecnicoRepositorio : IRepositorio<Tecnico>
    {
        Task<int> BuscaLicencasDisponiveis(int tecnicoId);
        Task<int> BuscaQtdLicencas(int tecnicoId);
    }
}
