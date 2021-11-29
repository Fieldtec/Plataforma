using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IUsuarioClienteRepositorio : IRepositorio<UsuarioCliente>
    {
        Task<List<UsuarioClienteDTO>> ObterPaginacao(int? idCliente = null);
        Task<int> ObterIdCliente(int idUsuarioCliente);
    }
}
