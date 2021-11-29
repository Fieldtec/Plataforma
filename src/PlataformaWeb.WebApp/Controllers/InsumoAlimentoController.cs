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
    public class InsumoAlimentoController : MainController
    {
        private readonly IInsumoAlimentoService _insumoAlimentoService;
        private readonly IFornecedorInsumoService _fornecedorInsumoService;
        private readonly IMapper _mapper;

        public InsumoAlimentoController(INotificador notificador,
                                        IUser appUser,
                                        IInsumoAlimentoService insumoAlimentoService,
                                        IMapper mapper, 
                                        IFornecedorInsumoService fornecedorInsumoService) : base(notificador, appUser)
        {
            _insumoAlimentoService = insumoAlimentoService;
            _mapper = mapper;
            _fornecedorInsumoService = fornecedorInsumoService;
        }

        [Route("insumos")]
        public async Task<ActionResult> Index()
        {
            return View(await _insumoAlimentoService.ObterPaginacao());
        }

        [Route("detalhe-insumo/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<InsumoAlimentoViewModel>(await _insumoAlimentoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("novo-insumo")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create()
        {
            await CarregarFornecedores();

            return View();
        }
        
        [HttpPost("novo-insumo")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create(InsumoAlimentoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _insumoAlimentoService.Adicionar(_mapper.Map<InsumoAlimento>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Insumo Cadastrado com Sucesso!", "Index", "InsumoAlimento");

            return View(model);
        }

        [Route("editar-insumo/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<InsumoAlimentoViewModel>(await _insumoAlimentoService.ObterPorId(id));

            if (model is null) return NotFound();

            await CarregarFornecedores();

            return View(model);
        }

        [HttpPost("editar-insumo/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, InsumoAlimentoViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _insumoAlimentoService.Atualizar(_mapper.Map<InsumoAlimento>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Insumo Editado com Sucesso!", "Index", "InsumoAlimento");

            return View(model);
        }

        [Route("remover-insumo/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<InsumoAlimentoViewModel>(await _insumoAlimentoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-insumo/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, InsumoAlimentoViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _insumoAlimentoService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Insumo Removido com Sucesso!", "Index", "InsumoAlimento");

            return View(model);
        }

        [Route("buscar-insumo")]
        public async Task<ActionResult> BuscarQuery([FromQuery] string query)
        {
            if (String.IsNullOrEmpty(query)) return Json(new { });

            var fornecedoresInsumo = await _insumoAlimentoService.Buscar(x => x.Nome.ToUpper().StartsWith(query.ToUpper()));

            return Json(new { list = fornecedoresInsumo });
        }

        [Route("insumos-historico-ms/{id:int}")]
        public async Task<IActionResult> HistoricoMs(int id)
        {
            if (id <= 0) return NotFound();

            var logs = await _insumoAlimentoService.ObterLogsAlteracao(id);

            return CustomJsonResponse(logs);
        }

        [HttpPost("insumos-alterar-ms")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<IActionResult> AlterarMs([FromBody] InsumoAlimentoViewModel insumoAlimento)
        {
            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var model = _mapper.Map<InsumoAlimento>(insumoAlimento);

            await _insumoAlimentoService.AtualizarMateriaSeca(model);

            return CustomJsonResponse();
        }

        private async Task CarregarFornecedores()
        {
            var fornecedores = await _fornecedorInsumoService.Buscar(x => x.Status == Status.Ativado);
            ViewBag.fornecedores = fornecedores.OrderBy(x => x.Nome).ToList();
        }

    }
}
