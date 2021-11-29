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
    public class MovimentacaoEntreLoteController : MainController
    {
        private readonly IMovimentacaoEntreLoteService _service;
        private readonly IMotivoMovimentacaoService _motivoMovimentacaoService;
        private readonly IMapper _mapper;

        public MovimentacaoEntreLoteController(INotificador notificador,
                    IUser appUser,
                    IMovimentacaoEntreLoteService service,
                    IMapper mapper,
                    IMotivoMovimentacaoService motivoMovimentacaoService) : base(notificador, appUser)
        {
            _service = service;
            _mapper = mapper;
            _motivoMovimentacaoService = motivoMovimentacaoService;
        }

        [Route("movimentacoes-entre-locais")]
        public async Task<ActionResult> Index()
        {
            return View(await _service.ObterPaginacao());
        }

        [Route("detalhe-movimentacao-entre-local/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<MovimentacaoEntreLoteViewModel>(await _service.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("nova-movimentacao-entre-local")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create()
        {
            ViewBag.motivos = await _motivoMovimentacaoService.ObterPaginacao();

            return View();
        }

        [HttpPost("nova-movimentacao-entre-local")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create([FromBody] MovimentacaoEntreLoteViewModel model)
        {
            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var movimentacao = _mapper.Map<MovimentacaoEntreLote>(model);

            await _service.Adicionar(movimentacao);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(model);
        }

        //[Route("editar-movimentacao-entre-local/{id:int}")]
        //[ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        //public async Task<ActionResult> Edit(int id)
        //{
        //    if (id == 0) return NotFound();

        //    var model = _mapper.Map<MovimentacaoEntreLoteViewModel>(await _service.ObterPorId(id));

        //    if (model is null) return NotFound();

        //    return View(model);
        //}

        //[HttpPost("editar-movimentacao-entre-local/{id:int}")]
        //[IgnoreAntiforgeryToken]
        //[ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        //public async Task<ActionResult> Edit(int id, [FromBody] MovimentacaoEntreLoteViewModel model)
        //{
        //    if (id != model.Id)
        //    {
        //        AdicionarNotificacao("Requisição inválida");
        //        return CustomJsonResponse();
        //    }

        //    if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

        //    var movimentacao = _mapper.Map<MovimentacaoEntreLote>(model);

        //    await _service.Atualizar(movimentacao);

        //    if (OperacaoInvalida()) return CustomJsonResponse();

        //    return CustomJsonResponse(model);
        //}

        [Route("remover-movimentacao-entre-local/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<MovimentacaoEntreLoteDelecaoViewModel>(await _service.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-movimentacao-entre-local/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, MovimentacaoEntreLoteDelecaoViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _service.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Movimentação excluída com Sucesso!", "Index", "MovimentacaoEntreLote");

            return View(model);
        }

        //[Route("buscar-lote-ativo-local")]
        //public async Task<ActionResult> BuscarLoteAtivoPorLocal([FromQuery] int idLocal)
        //{
        //    if (idLocal == 0) return Json(new { });

        //    PlanejamentoNutricionalDTO planejamentoAtivo = null;
        //    var lote = await _service.Buscar(x => x.IdLocal == idLocal);

        //    if (lote.Count > 0)
        //    {
        //        var planejamentos = await _planejamentoNutricionalService.Buscar(x => x.Nome == lote.FirstOrDefault().Planejamento);
        //        planejamentoAtivo = planejamentos.FirstOrDefault();
        //    }

        //    return Json(new { lote, planejamentoAtivo });
        //}
    }
}
