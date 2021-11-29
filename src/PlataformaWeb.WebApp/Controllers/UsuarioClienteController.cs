using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Notificacoes;
using PlataformaWeb.WebApp.Extensions;
using PlataformaWeb.WebApp.Models;

namespace PlataformaWeb.WebApp.Controllers
{

    [Authorize]
    [ClaimsAuthorize(Role.Adm, Role.Tecnico, Role.Cliente)]
    public class UsuarioClienteController : MainController
    {
        private readonly IUsuarioClienteService _usuarioClienteService;
        private readonly IClienteService _clienteService;
        private readonly IMapper _mapper;

        public UsuarioClienteController(INotificador notificador,
                                        IUser appUser,
                                        IUsuarioClienteService usuarioClienteService,
                                        IMapper mapper, 
                                        IClienteService clienteService) : base(notificador, appUser)
        {
            _usuarioClienteService = usuarioClienteService;
            _mapper = mapper;
            _clienteService = clienteService;
        }

        [Route("usuarios/{idCliente:int?}")]
        public async Task<ActionResult> Index()
        {
            return View(await _usuarioClienteService.ObterPaginacao());
        }

        [Route("usuarios/cliente/{idCliente:int}")]
        [ClaimsAuthorize(Role.Adm, Role.Tecnico)]
        public async Task<ActionResult> Index(int idCliente)
        {
            if (!await ValidaQueryStringCliente(idCliente)) return NotFound();

            var usuarios = await _usuarioClienteService.ObterPaginacao(idCliente);           

            return View(usuarios);
        }

        private async Task<bool> ValidaQueryStringCliente(int idCliente)
        {
            var cliente = await _clienteService.ObterPorId(idCliente);

            if (cliente is null) return false;

            ViewBag.NomeProprietario = cliente.NomePropriedade;
            ViewBag.IdCliente = cliente.Id;

            return true;
        }


        // GET: ClienteController/Details/5
        [Route("detalhe-usuario/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<UsuarioClienteViewModel>(await _usuarioClienteService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        // GET: ClienteController/Create
        [Route("novo-usuario")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClienteController/Create
        [HttpPost("novo-usuario")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente)]
        public async Task<ActionResult> Create(UsuarioClienteViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _usuarioClienteService.Adicionar(_mapper.Map<UsuarioCliente>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Usuario Cadastrado com Sucesso!", "Index", "UsuarioCliente");

            return View(model);
        }

        // GET: ClienteController/Edit/5
        [Route("editar-usuario/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<UsuarioClienteViewModel>(await _usuarioClienteService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        // POST: ClienteController/Edit/5
        [HttpPost("editar-usuario/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente)]
        public async Task<ActionResult> Edit(int id, UsuarioClienteViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _usuarioClienteService.Atualizar(_mapper.Map<UsuarioCliente>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Usuario Editado com Sucesso!", "Index", "UsuarioCliente");

            return View(model);
        }

        // GET: ClienteController/Delete/5
        [Route("remover-usuario/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<UsuarioClienteViewModel>(await _usuarioClienteService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        // POST: ClienteController/Delete/5
        [HttpPost("remover-usuario/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente)]
        public async Task<ActionResult> Delete(int id, UsuarioClienteViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _usuarioClienteService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Usuario Removido com Sucesso!", "Index", "UsuarioCliente");

            return View(model);
        }

    }
}
