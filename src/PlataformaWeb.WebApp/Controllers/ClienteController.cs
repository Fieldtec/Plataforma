using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [ClaimsAuthorize(Role.Adm, Role.Tecnico)]
    public class ClienteController : MainController
    {
        private readonly IClienteService _clienteService;
        private readonly IMapper _mapper;

        public ClienteController(INotificador notificador,
                                 IUser appUser,
                                 IClienteService clienteService,
                                 IMapper mapper) : base(notificador, appUser)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }


        [Route("buscar-clientes")]
        [ClaimsAuthorize(Role.Tecnico, Role.Adm)]
        public async Task<ActionResult> BuscarClientes()
        {
            return Json(new { list = await _clienteService.ObterPaginacao() });
        }

        [Route("clientes")]
        //[ClaimsAuthorize(Role.Adm)]
        public async Task<ActionResult> Index()
        {
            if (AppUser.EhTecnico())
            {
                ViewBag.LicencasDisponiveis = await _clienteService.ObterLicencasDisponiveis();
            }

            return View(await _clienteService.ObterPaginacao());
        }

        // GET: ClienteController/Details/5
        [Route("detalhe-cliente/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<ClienteViewModel>(await _clienteService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        // GET: ClienteController/Create
        [Route("novo-cliente")]
        [ClaimsAuthorize(Role.Tecnico)]
        public async Task<ActionResult> Create()
        {
            var qtdLicensas = await _clienteService.ObterLicencasDisponiveis();
            if (qtdLicensas == 0)
            {
                RedirecionaPara("Técnico não possui licenças disponíveis", "Index", "Cliente", "Atenção", "warning");
            }

            return View();
        }

        // POST: ClienteController/Create
        [HttpPost("novo-cliente")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico)]
        public async Task<ActionResult> Create(ClienteViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _clienteService.Adicionar(_mapper.Map<Cliente>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Cliente Cadastrado com Sucesso!", "Index", "Cliente");

            return View(model);
        }

        // GET: ClienteController/Edit/5
        [Route("editar-cliente/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<ClienteViewModel>(await _clienteService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        // POST: ClienteController/Edit/5
        [HttpPost("editar-cliente/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico)]
        public async Task<ActionResult> Edit(int id, ClienteViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _clienteService.Atualizar(_mapper.Map<Cliente>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Cliente Editado com Sucesso!", "Index", "Cliente");

            return View(model);
        }

        // GET: ClienteController/Delete/5
        [Route("remover-cliente/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<ClienteViewModel>(await _clienteService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        // POST: ClienteController/Delete/5
        [HttpPost("remover-cliente/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico)]
        public async Task<ActionResult> Delete(int id, ClienteViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _clienteService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Cliente Removido com Sucesso!", "Index", "Cliente");

            return View(model);
        }
    }
}
