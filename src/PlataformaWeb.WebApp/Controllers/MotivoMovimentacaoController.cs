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
    public class MotivoMovimentacaoController : MainController
    {
        private readonly IMotivoMovimentacaoService _motivoMovimentacaoService;
        private readonly IMapper _mapper;

        public MotivoMovimentacaoController(INotificador notificador,
                                    IUser appUser,
                                    IMotivoMovimentacaoService motivoMovimentacaoService,
                                    IMapper mapper) : base(notificador, appUser)
        {
            _motivoMovimentacaoService = motivoMovimentacaoService;
            _mapper = mapper;
        }

        [Route("motivo-movimentacoes")]
        public async Task<ActionResult> Index()
        {
            return View(await _motivoMovimentacaoService.ObterPaginacao());
        }

        [Route("detalhe-motivo-movimentacao/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<MotivoMovimentacaoViewModel>(await _motivoMovimentacaoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("novo-motivo-movimentacao")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("novo-motivo-movimentacao")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create(MotivoMovimentacaoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _motivoMovimentacaoService.Adicionar(_mapper.Map<MotivoMovimentacao>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Motivo Cadastrado com Sucesso!", "Index", "MotivoMovimentacao");

            return View(model);
        }

        [HttpPost("novo-motivo-movimentacao-modal")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> CreateModal([FromBody] MotivoMovimentacaoViewModel model)
        {
            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var motivo = _mapper.Map<MotivoMovimentacao>(model);

            await _motivoMovimentacaoService.Adicionar(motivo);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(_mapper.Map<MotivoMovimentacaoViewModel>(motivo));
        }

        [Route("editar-motivo-movimentacao/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<MotivoMovimentacaoViewModel>(await _motivoMovimentacaoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("editar-motivo-movimentacao/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, MotivoMovimentacaoViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _motivoMovimentacaoService.Atualizar(_mapper.Map<MotivoMovimentacao>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Motivo Editado com Sucesso!", "Index", "MotivoMovimentacao");

            return View(model);
        }

        [Route("remover-motivo-movimentacao/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<MotivoMovimentacaoViewModel>(await _motivoMovimentacaoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-motivo-movimentacao/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, MotivoMovimentacaoViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _motivoMovimentacaoService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Motivo Removido com Sucesso!", "Index", "MotivoMovimentacao");

            return View(model);
        }

        [Route("buscar-motivo-movimentacao")]
        public async Task<ActionResult> BuscarQuery([FromQuery] string query)
        {
            if (String.IsNullOrEmpty(query)) return Json(new { });

            var fases = await _motivoMovimentacaoService.Buscar(x => x.Nome.ToUpper().StartsWith(query.ToUpper()));

            return Json(new { list = fases });
        }

    }
}
