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
    public class ProdutorParceiroController : MainController
    {
        private readonly IProdutorParceiroService _produtorParceiroService;
        private readonly IPropriedadeParceiraService _propriedadeParceiraService;
        private readonly IMapper _mapper;

        public ProdutorParceiroController(INotificador notificador,
                                          IUser appUser,
                                          IProdutorParceiroService produtorParceiroService,
                                          IMapper mapper, 
                                          IPropriedadeParceiraService propriedadeParceiraService) : base(notificador, appUser)
        {
            _produtorParceiroService = produtorParceiroService;
            _mapper = mapper;
            _propriedadeParceiraService = propriedadeParceiraService;
        }

        [Route("produtores-parceiros/{idCliente:int?}")]
        public async Task<ActionResult> Index()
        {
            return View(await _produtorParceiroService.ObterPaginacao());
        }

        [Route("detalhe-produtor-parceiro/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<ProdutorParceiroViewModel>(await _produtorParceiroService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("novo-produtor-parceiro")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create()
        {
            await CarregarPropriedades();

            return View();
        }

        [HttpPost("novo-produtor-parceiro")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create(ProdutorParceiroViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _produtorParceiroService.Adicionar(_mapper.Map<ProdutorParceiro>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Produtor Parceiro Cadastrado com Sucesso!", "Index", "ProdutorParceiro");

            return View(model);
        }

        [Route("editar-produtor-parceiro/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<ProdutorParceiroViewModel>(await _produtorParceiroService.ObterPorId(id));

            if (model is null) return NotFound();

            await CarregarPropriedades();

            return View(model);
        }

        [HttpPost("editar-produtor-parceiro/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, ProdutorParceiroViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _produtorParceiroService.Atualizar(_mapper.Map<ProdutorParceiro>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Produtor Parceiro Editado com Sucesso!", "Index", "ProdutorParceiro");

            return View(model);
        }

        [Route("remover-produtor-parceiro/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<ProdutorParceiroViewModel>(await _produtorParceiroService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-produtor-parceiro/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, ProdutorParceiroViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _produtorParceiroService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Produtor Parceiro Removido com Sucesso!", "Index", "ProdutorParceiro");

            return View(model);
        }

        [Route("buscar-produtor-parceiro")]
        public async Task<ActionResult> BuscarQuery([FromQuery] string query)
        {
            if (String.IsNullOrEmpty(query)) return Json(new { });

            var racoes = await _produtorParceiroService.Buscar(x => x.Nome.ToUpper().StartsWith(query.ToUpper()));

            return Json(new { list = racoes });
        }

        private async Task CarregarPropriedades()
        {
            var propriedades = _mapper.Map<IEnumerable<PropriedadeParceiraViewModel>>(await _propriedadeParceiraService.Buscar(x => x.Status == Status.Ativado));
            ViewBag.propriedades = propriedades.OrderBy(x => x.Nome).ToList();
        }

    }
}
