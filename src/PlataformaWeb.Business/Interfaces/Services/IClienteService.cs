using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Services
{
    public interface IClienteService : IBaseService<Cliente, ClienteDTO>
    {
        Task<int> ObterLicencasDisponiveis();
    }
}
