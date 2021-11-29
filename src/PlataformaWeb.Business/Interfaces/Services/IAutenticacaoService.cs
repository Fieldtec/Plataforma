using PlataformaWeb.Business.DTO;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Services
{
    public interface IAutenticacaoService 
    {
        Task<UsuarioResponseDTO> Login(UsuarioLoginDTO login);
        Task<List<Claim>> ObterClaims(UsuarioResponseDTO usuarioLogado);
        Task<List<Claim>> SelecionarCliente(ClienteDTO cliente);
    }
}
