using Microsoft.AspNetCore.Http;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Extensions
{
    public class AppUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AppUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Nome => _accessor.HttpContext.User.Identity.Name;

        public bool EhAdmin()
        {
            return TemRole(TipoPessoa.Adm);
        }

        public bool EhCliente()
        {
            return TemRole(TipoPessoa.Cliente);
        }

        public bool EhTecnico()
        {
            return TemRole(TipoPessoa.Tecnico);
        }

        public bool EstaAutenticado()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public IEnumerable<Claim> ObterClaims()
        {
            return _accessor.HttpContext.User.Claims;
        }

        public string ObterUsuario()
        {
            return EstaAutenticado() ? _accessor.HttpContext.User.GetUserNameId() : String.Empty;
        }

        public string ObterEmail()
        {
            return EstaAutenticado() ? _accessor.HttpContext.User.GetUserEmail() : String.Empty;
        }

        public int ObterId()
        {
            return EstaAutenticado() ? int.Parse(_accessor.HttpContext.User.GetUserId()) : 0;
        }

        public bool EhUsuarioDoCliente()
        {
            return TemRole(TipoPessoa.UsuarioCliente);
        }

        public int ObterIdCliente()
        {
            if (EhUsuarioDoCliente() || EhCliente())
                return int.Parse(_accessor.HttpContext.User.GetUserClienteId());
            else if (EhTecnico() || EhAdmin())
            {
                int idCliente = 0;
                int.TryParse(_accessor.HttpContext.User.GetUserClienteId(), out idCliente);
                return idCliente;
            }

            return 0;
        }

        private bool TemRole(TipoPessoa tipoPessoa)
        {
            if (EstaAutenticado())
            {
                var role = _accessor.HttpContext.User.GetUserRole();

                if (role == tipoPessoa.ObterDescricao()) return true;
            }

            return false;
        }

        public string ObterNomePropriedade()
        {
            //if (!EhAdmin())
            return _accessor.HttpContext.User.GetNomePropriedade();

            //return String.Empty;
        }

        public string ObterNomeProprietario()
        {
            return _accessor.HttpContext.User.GetNomeProprietario();

            //return String.Empty;
        }

        public void SelecionarCliente(ClienteDTO cliente)
        {
            if (EhTecnico())
            {
                //_accessor.HttpContext.User.Add
            }
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.GetClaimValue(ClaimTypes.NameIdentifier);

            //if (principal == null)
            //{
            //    throw new ArgumentException(nameof(principal));
            //}

            //var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            //return claim?.Value;
        }

        public static string GetUserNameId(this ClaimsPrincipal principal)
        {
            return principal.GetClaimValue(ClaimTypes.Surname);

            //if (principal == null)
            //{
            //    throw new ArgumentException(nameof(principal));
            //}

            //var claim = principal.FindFirst(ClaimTypes.Surname);
            //return claim?.Value;
        }

        public static string GetUserRole(this ClaimsPrincipal principal)
        {
            return principal.GetClaimValue("role");

            //if (principal == null)
            //{
            //    throw new ArgumentException(nameof(principal));
            //}

            //var claim = principal.FindFirst("role");
            //return claim?.Value;
        }

        public static string GetNomePropriedade(this ClaimsPrincipal principal)
        {

            return principal.GetClaimValue(CustomClaims.NomePropriedade);

            //if (principal == null)
            //{
            //    throw new ArgumentException(nameof(principal));
            //}

            //var claim = principal.FindFirst(CustomClaims.NomePropriedade);
            //return claim?.Value;
        }

        public static string GetNomeProprietario(this ClaimsPrincipal principal)
        {
            return principal.GetClaimValue(CustomClaims.NomeProprietario);

            //if (principal == null)
            //{
            //    throw new ArgumentException(nameof(principal));
            //}

            //var claim = principal.FindFirst(CustomClaims.NomeProprietario);
            //return claim?.Value;
        }


        public static string GetUserClienteId(this ClaimsPrincipal principal)
        {
            return principal.GetClaimValue(CustomClaims.UsuarioCliente);

            //if (principal == null)
            //{
            //    throw new ArgumentException(nameof(principal));
            //}

            //var claim = principal.FindFirst(CustomClaims.UsuarioCliente);
            //return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            return principal.GetClaimValue(ClaimTypes.Email);
            //if (principal == null)
            //{
            //    throw new ArgumentException(nameof(principal));
            //}

            //var claim = principal.FindFirst(ClaimTypes.Email);
            //return claim?.Value;
        }

        private static string GetClaimValue(this ClaimsPrincipal principal, string claimType)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(claimType);
            return claim?.Value;
        }

    }
}
