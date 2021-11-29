using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IClienteRepositorio : IRepositorio<Cliente>
    {
        Task<List<ClienteDTO>> ObterPaginacao();
        Task<DateTime?> ObterDataValidadeLicenca(int id);
    }
}
