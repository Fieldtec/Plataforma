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
    public class CausaMorteController : MainController
    {
        private readonly ICausaMorteService _causaMorteService;
        private readonly IMapper _mapper;

        public CausaMorteController(INotificador notificador, 
                                    IUser appUser, 
                                    ICausaMorteService causaMorteService, 
                                    IMapper mapper) : base(notificador, appUser)
        {
            _causaMorteService = causaMorteService;
            _mapper = mapper;
        }

        [Route("causas-da-morte")]
        public async Task<ActionResult> Index()
        {
            return View(await _causaMorteService.ObterPaginacao());
        }

        [Route("detalhe-causas-da-morte/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<CausaMorteViewModel>(await _causaMorteService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("nova-causa-da-morte")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("nova-causa-da-morte")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create(CausaMorteViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _causaMorteService.Adicionar(_mapper.Map<CausaMorte>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Causa da Morte Cadastrada com Sucesso!", "Index", "CausaMorte");

            return View(model);
        }

        [HttpPost("nova-causa-da-morte-modal")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> CreateModal([FromBody] CausaMorteViewModel model)
        {
            if (!ModelState.IsValid) return CustomJsonResponse();

            var causa = _mapper.Map<CausaMorte>(model);

            await _causaMorteService.Adicionar(causa);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(_mapper.Map<CausaMorteViewModel>(causa));
        }

        [Route("editar-causa-da-morte/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<CausaMorteViewModel>(await _causaMorteService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("editar-causa-da-morte/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, CausaMorteViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _causaMorteService.Atualizar(_mapper.Map<CausaMorte>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Causa da Morte Editada com Sucesso!", "Index", "CausaMorte");

            return View(model);
        }

        [Route("remover-causa-da-morte/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<CausaMorteViewModel>(await _causaMorteService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-causa-da-morte/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, CausaMorteViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _causaMorteService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Causa da Morte Removida com Sucesso!", "Index", "CausaMorte");

            return View(model);
        }

        [Route("buscar-causa-da-morte")]
        public async Task<ActionResult> BuscarQuery([FromQuery] string query)
        {
            if (String.IsNullOrEmpty(query)) return Json(new { });

            var fases = await _causaMorteService.Buscar(x => x.Nome.ToUpper().StartsWith(query.ToUpper()));

            return Json(new { list = fases });
        }

    }
}
