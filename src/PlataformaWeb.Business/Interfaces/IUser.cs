using PlataformaWeb.Business.DTO;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace PlataformaWeb.Business.Interfaces
{
    public interface IUser
    {
        string Nome { get; }
        int ObterId();
        String ObterEmail();
        String ObterUsuario();
        IEnumerable<Claim> ObterClaims();
        bool EhAdmin();
        bool EhTecnico();
        bool EhCliente();
        bool EhUsuarioDoCliente();
        bool EstaAutenticado();
        int ObterIdCliente();
        string ObterNomePropriedade();
        string ObterNomeProprietario();
        void SelecionarCliente(ClienteDTO cliente);
    }
}
