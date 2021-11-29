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
    public class FaseDoAnoController : MainController
    {
        private readonly IFaseDoAnoService _faseDoAnoService;
        private readonly IMapper _mapper;

        public FaseDoAnoController(INotificador notificador, 
                                   IUser appUser, 
                                   IFaseDoAnoService faseDoAnoService, 
                                   IMapper mapper) : base(notificador, appUser)
        {
            _faseDoAnoService = faseDoAnoService;
            _mapper = mapper;
        }

        [Route("fases-do-ano")]
        public async Task<ActionResult> Index()
        {
            return View(await _faseDoAnoService.ObterPaginacao());
        }

        [Route("detalhe-fase-do-ano/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<FaseDoAnoViewModel>(await _faseDoAnoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("nova-fase-do-ano")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("nova-fase-do-ano")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create(FaseDoAnoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _faseDoAnoService.Adicionar(_mapper.Map<FaseDoAno>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Fase do Ano Cadastrada com Sucesso!", "Index", "FaseDoAno");

            return View(model);
        }

        [Route("editar-fase-do-ano/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<FaseDoAnoViewModel>(await _faseDoAnoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("editar-fase-do-ano/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, FaseDoAnoViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _faseDoAnoService.Atualizar(_mapper.Map<FaseDoAno>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Fase do Ano Editada com Sucesso!", "Index", "FaseDoAno");

            return View(model);
        }

        [Route("remover-fase-do-ano/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<FaseDoAnoViewModel>(await _faseDoAnoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-fase-do-ano/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, FaseDoAnoViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _faseDoAnoService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Fase do Ano Removida com Sucesso!", "Index", "FaseDoAno");

            return View(model);
        }

        [Route("buscar-fase")]
        public async Task<ActionResult> BuscarQuery([FromQuery] string query)
        {
            if (String.IsNullOrEmpty(query)) return Json(new { });

            var fases = await _faseDoAnoService.Buscar(x => x.Nome.ToUpper().StartsWith(query.ToUpper()));

            return Json(new { list = fases });
        }

    }
}
