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
    public class FornecedorInsumoController : MainController
    {
        private readonly IFornecedorInsumoService _fornecedorInsumoService;
        private readonly IMapper _mapper;

        public FornecedorInsumoController(INotificador notificador,
                                          IUser appUser, 
                                          IFornecedorInsumoService fornecedorInsumoSerivce, 
                                          IMapper mapper) : base(notificador, appUser)
        {
            _fornecedorInsumoService = fornecedorInsumoSerivce;
            _mapper = mapper;
        }

        [Route("fornecedores-de-insumo")]
        public async Task<ActionResult> Index()
        {
            return View(await _fornecedorInsumoService.ObterPaginacao());
        }

        [Route("detalhe-fornecedor-insumo/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<FornecedorInsumoViewModel>(await _fornecedorInsumoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("novo-fornecedor-insumo")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("novo-fornecedor-insumo")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create(FornecedorInsumoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _fornecedorInsumoService.Adicionar(_mapper.Map<FornecedorInsumo>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Fornecedor de Insumo Cadastrado com Sucesso!", "Index", "FornecedorInsumo");

            return View(model);
        }

        [HttpPost("novo-fornecedor-modal")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<JsonResult> CreateModal([FromBody] FornecedorInsumoViewModel model)
        {
            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            //converte antes para pegar o Id após salvar
            var fornecedor = _mapper.Map<FornecedorInsumo>(model);

            await _fornecedorInsumoService.Adicionar(fornecedor);

            return CustomJsonResponse(_mapper.Map<FornecedorInsumoViewModel>(fornecedor));
            
        }


        [Route("editar-fornecedor-insumo/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<FornecedorInsumoViewModel>(await _fornecedorInsumoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("editar-fornecedor-insumo/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, FornecedorInsumoViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _fornecedorInsumoService.Atualizar(_mapper.Map<FornecedorInsumo>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Fornecedor de Insumo Editado com Sucesso!", "Index", "FornecedorInsumo");

            return View(model);
        }

        [Route("remover-fornecedor-insumo/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<FornecedorInsumoViewModel>(await _fornecedorInsumoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-fornecedor-insumo/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, FornecedorInsumoViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _fornecedorInsumoService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Fornecedor de Insumo Removido com Sucesso!", "Index", "FornecedorInsumo");

            return View(model);
        }

        [Route("buscar-fornecedor")]
        public async Task<ActionResult> BuscarQuery([FromQuery] string query)
        {
            if (String.IsNullOrEmpty(query)) return Json(new { });

            var fornecedoresInsumo = await _fornecedorInsumoService.Buscar(x => x.Nome.ToUpper().StartsWith(query.ToUpper()));

            return Json(new { list = _mapper.Map<IEnumerable<FornecedorInsumoViewModel>>(fornecedoresInsumo) });
        }

    }
}
