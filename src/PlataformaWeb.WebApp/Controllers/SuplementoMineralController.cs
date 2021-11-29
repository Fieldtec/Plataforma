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
    public class SuplementoMineralController : MainController
    {
        private readonly ISuplementoMineralService _suplementoMineralService;
        private readonly IFornecedorInsumoService _fornecedorInsumoService;
        private readonly IMapper _mapper;

        public SuplementoMineralController(INotificador notificador,
                                           IUser appUser,
                                           ISuplementoMineralService suplementoMineralService,
                                           IMapper mapper, IFornecedorInsumoService fornecedorInsumoService) : base(notificador, appUser)
        {
            _suplementoMineralService = suplementoMineralService;
            _mapper = mapper;
            _fornecedorInsumoService = fornecedorInsumoService;
        }

        [Route("suplementos")]
        public async Task<ActionResult> Index()
        {
            return View(await _suplementoMineralService.ObterPaginacao());
        }

        [Route("detalhe-suplemento/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<SuplementoMineralViewModel>(await _suplementoMineralService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("novo-suplemento")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create()
        {
            await CarregarFornecedores();

            return View();
        }

        [HttpPost("novo-suplemento")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create(SuplementoMineralViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _suplementoMineralService.Adicionar(_mapper.Map<SuplementoMineral>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Suplemento Cadastrado com Sucesso!", "Index", "SuplementoMineral");

            return View(model);
        }

        [Route("editar-suplemento/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<SuplementoMineralViewModel>(await _suplementoMineralService.ObterPorId(id));

            if (model is null) return NotFound();

            await CarregarFornecedores();

            return View(model);
        }

        [HttpPost("editar-suplemento/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, SuplementoMineralViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _suplementoMineralService.Atualizar(_mapper.Map<SuplementoMineral>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Suplemento Editado com Sucesso!", "Index", "SuplementoMineral");

            return View(model);
        }

        [Route("remover-suplemento/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<SuplementoMineralViewModel>(await _suplementoMineralService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-suplemento/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, SuplementoMineralViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _suplementoMineralService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Suplemento Removido com Sucesso!", "Index", "SuplementoMineral");

            return View(model);
        }

        [Route("buscar-suplemento")]
        public async Task<ActionResult> BuscarQuery([FromQuery] string query)
        {
            if (String.IsNullOrEmpty(query)) return Json(new { });

            var suplementos = await _suplementoMineralService.Buscar(x => x.Nome.ToUpper().StartsWith(query.ToUpper()));

            return Json(new { list = suplementos });
        }

        private async Task CarregarFornecedores()
        {
            var fornecedores = await _fornecedorInsumoService.Buscar(x => x.Status == Status.Ativado);
            ViewBag.fornecedores = fornecedores.OrderBy(x => x.Nome).ToList();
        }

    }
}
