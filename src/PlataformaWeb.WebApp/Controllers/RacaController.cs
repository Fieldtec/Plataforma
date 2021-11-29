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
    public class RacaController : MainController
    {
        private readonly IRacaService _racaService;
        private readonly IMapper _mapper;

        public RacaController(INotificador notificador, 
                              IUser appUser, 
                              IRacaService racaService, 
                              IMapper mapper) : base(notificador, appUser)
        {
            _racaService = racaService;
            _mapper = mapper;
        }

        [Route("racas/{idCliente:int?}")]
        public async Task<ActionResult> Index()
        {
            return View(await _racaService.ObterPaginacao());
        }

        [Route("detalhe-raca/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<RacaViewModel>(await _racaService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("nova-raca")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("nova-raca")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create(RacaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _racaService.Adicionar(_mapper.Map<Raca>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Raça Cadastrada com Sucesso!", "Index", "Raca");

            return View(model);
        }

        [Route("editar-raca/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<RacaViewModel>(await _racaService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("editar-raca/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, RacaViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _racaService.Atualizar(_mapper.Map<Raca>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Raça Editada com Sucesso!", "Index", "Raca");

            return View(model);
        }

        [Route("remover-raca/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<RacaViewModel>(await _racaService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-raca/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, RacaViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _racaService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Raça Removida com Sucesso!", "Index", "Raca");

            return View(model);
        }

        [Route("buscar-raca")]
        public async Task<ActionResult> BuscarQuery([FromQuery] string query)
        {
            if (String.IsNullOrEmpty(query)) return Json(new { });

            var racoes = await _racaService.Buscar(x => x.Nome.ToUpper().StartsWith(query.ToUpper()));

            return Json(new { list = racoes });
        }

    }
}
