using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Notificacoes;
using PlataformaWeb.WebApp.Extensions;
using PlataformaWeb.WebApp.Models;

namespace PlataformaWeb.WebApp.Controllers
{
    public class AutenticacaoController : MainController
    {
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly IClienteService _clienteService;
        private readonly IMapper _mapper;

        public AutenticacaoController(INotificador notificador,
                                      IUser appUser,
                                      IAutenticacaoService autenticacaoService,
                                      IMapper mapper, 
                                      IClienteService clienteService) : base(notificador, appUser)
        {
            _autenticacaoService = autenticacaoService;
            _mapper = mapper;
            _clienteService = clienteService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [Route("login")]
        public async Task<IActionResult> Login(UsuarioLoginViewModel usuarioLogin, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(usuarioLogin);

            var loginResponse = await _autenticacaoService.Login(_mapper.Map<UsuarioLoginDTO>(usuarioLogin));

            if (OperacaoInvalida()) return View(usuarioLogin);

            await RealizarLogin(loginResponse);

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

            return LocalRedirect(returnUrl);
        }

        [Authorize]
        [HttpGet]
        [Route("selecionar-cliente/{idCliente:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Adm)]
        public async Task<IActionResult> SelecionarCliente(int idCliente)
        {
            var cliente = await ValidaSelecaoCliente(idCliente);

            if (cliente is null)
            {
                AdicionarNotificacao("Nenhum cliente encontrado");
                return CustomJsonResponse();
            }                       

            await SelecionarCliente(cliente);

            return CustomJsonResponse(cliente);
        }

        [Authorize]
        [HttpGet]
        [Route("selecionar-cliente-view/{idCliente:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Adm)]
        public async Task<IActionResult> SelecionarClienteView(int idCliente)
        {
            var cliente = await ValidaSelecaoCliente(idCliente);

            if (cliente is null) return NotFound();

            await SelecionarCliente(cliente);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("sair")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login", "Autenticacao");
        }

        private async Task<ClienteDTO> ValidaSelecaoCliente(int idCliente)
        {
            if (idCliente <= 0) return null;

            var cliente = await _clienteService.ObterPorId(idCliente);

            if (cliente is null) return null;

            return new ClienteDTO(cliente);
        }

        private async Task SelecionarCliente(ClienteDTO cliente)
        {
            var novasClaims = await _autenticacaoService.SelecionarCliente(cliente);

            await CriarAutenticacao(novasClaims);
        }

        private async Task RealizarLogin(UsuarioResponseDTO resposta)
        {
            var claims = await _autenticacaoService.ObterClaims(resposta);

            await CriarAutenticacao(claims);
        }

        private async Task CriarAutenticacao(List<Claim> claims)
        {
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

    }
}
