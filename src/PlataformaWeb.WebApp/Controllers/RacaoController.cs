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
    public class RacaoController : MainController
    {
        private readonly IRacaoService _racaoService;
        private readonly IInsumoAlimentoService _insumoAlimentoService;
        private readonly IMapper _mapper;

        public RacaoController(INotificador notificador,
                               IUser appUser,
                               IRacaoService racaoService,
                               IMapper mapper, 
                               IInsumoAlimentoService insumoAlimentoService) : base(notificador, appUser)
        {
            _racaoService = racaoService;
            _mapper = mapper;
            _insumoAlimentoService = insumoAlimentoService;
        }

        [Route("racoes")]
        public async Task<ActionResult> Index()
        {
            return View(await _racaoService.ObterPaginacao());
        }

        [Route("detalhe-racao/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<RacaoViewModel>(await _racaoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("nova-racao")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create()
        {
            await CarregarInsumos();

            return View();
        }

        [HttpPost("nova-racao")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create([FromBody] RacaoViewModel model)
        {
            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var racao = _mapper.Map<Racao>(model);

            await _racaoService.Adicionar(racao);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(_mapper.Map<RacaoViewModel>(racao));
        }

        [Route("editar-racao/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<RacaoViewModel>(await _racaoService.ObterPorId(id));

            if (model is null) return NotFound();

            await CarregarInsumos();

            return View(model);
        }

        [HttpPost("editar-racao/{id:int}")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, [FromBody] RacaoViewModel model)
        {
            if (id != model.Id)
            {
                AdicionarNotificacao("Requisição inválida");
                return CustomJsonResponse();
            }

            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var racao = _mapper.Map<Racao>(model);

            await _racaoService.Atualizar(racao);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(_mapper.Map<RacaoViewModel>(racao));
        }

        [Route("remover-racao/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<RacaoViewModel>(await _racaoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-racao/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, RacaoViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _racaoService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Ração Removida com Sucesso!", "Index", "Racao");

            return View(model);
        }

        [HttpPost("remover-insumo-racao/{idRacaoInsumo:int}")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> DeleteInsumo(int idRacaoInsumo, [FromBody] RacaoViewModel model)
        {
            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var racao = _mapper.Map<Racao>(model);

            await _racaoService.RemoverInsumo(racao, idRacaoInsumo);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(_mapper.Map<RacaoViewModel>(racao));
        }

        [HttpPost("calcular-insumo-racao")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public ActionResult CalcularValores([FromBody] RacaoViewModel model)
        {
            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var racao = _mapper.Map<Racao>(model);

            _racaoService.CalcularValores(racao);

            return CustomJsonResponse(_mapper.Map<RacaoViewModel>(racao));
        }

        [Route("buscar-racao")]
        public async Task<ActionResult> BuscarQuery([FromQuery] string query)
        {
            if (String.IsNullOrEmpty(query)) return Json(new { });

            var racoes = await _racaoService.Buscar(x => x.Nome.ToUpper().StartsWith(query.ToUpper()));

            return Json(new { list = racoes });
        }

        public async Task CarregarInsumos()
        {
            var insumos = await _insumoAlimentoService.Buscar(x => x.Status == Status.Ativado);
            ViewBag.insumos = insumos.OrderBy(x => x.Nome).ToList();
        }

    }
}
