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
    public class CategoriaController : MainController
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IMapper _mapper;

        public CategoriaController(INotificador notificador, 
                                   IUser appUser, 
                                   IMapper mapper, 
                                   ICategoriaService categoriaService) : base(notificador, appUser)
        {
            _mapper = mapper;
            _categoriaService = categoriaService;
        }

        [Route("categorias/{idCliente:int?}")]
        public async Task<ActionResult> Index()
        {
            return View(await _categoriaService.ObterPaginacao());
        }

        [Route("detalhe-categoria/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<CategoriaViewModel>(await _categoriaService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("nova-categoria")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("nova-categoria")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create(CategoriaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _categoriaService.Adicionar(_mapper.Map<Categoria>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Categoria Cadastrada com Sucesso!", "Index", "Categoria");

            return View(model);
        }

        [Route("editar-categoria/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<CategoriaViewModel>(await _categoriaService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("editar-categoria/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, CategoriaViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _categoriaService.Atualizar(_mapper.Map<Categoria>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Categoria Editada com Sucesso!", "Index", "Categoria");

            return View(model);
        }

        [Route("remover-categoria/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<CategoriaViewModel>(await _categoriaService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }
                
        [HttpPost("remover-categoria/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, CategoriaViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _categoriaService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Categoria Removida com Sucesso!", "Index", "Categoria");

            return View(model);
        }

        [Route("buscar-categoria")]
        public async Task<ActionResult> BuscarQuery([FromQuery] string query)
        {
            if (String.IsNullOrEmpty(query)) return Json(new { });

            var categorias = await _categoriaService.Buscar(x => x.Nome.ToUpper().StartsWith(query.ToUpper()));

            return Json(new { list = categorias });
        }

    }
}
