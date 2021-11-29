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
    public class PropriedadeParceiraController : MainController
    {
        private readonly IPropriedadeParceiraService _propriedadeParceiraService;
        private readonly IMapper _mapper;

        public PropriedadeParceiraController(INotificador notificador, 
                                             IUser appUser, 
                                             IPropriedadeParceiraService propriedadeParceiraService, 
                                             IMapper mapper) : base(notificador, appUser)
        {
            _propriedadeParceiraService = propriedadeParceiraService;
            _mapper = mapper;
        }

        [Route("propriedades-parceiras/{idCliente:int?}")]
        public async Task<ActionResult> Index()
        {
            return View(await _propriedadeParceiraService.ObterPaginacao());
        }

        [Route("detalhe-propriedade-parceira/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<PropriedadeParceiraViewModel>(await _propriedadeParceiraService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("nova-propriedade-parceira")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("nova-propriedade-parceira")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create(PropriedadeParceiraViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _propriedadeParceiraService.Adicionar(_mapper.Map<PropriedadeParceira>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Propriedade Parceira Cadastrada com Sucesso!", "Index", "PropriedadeParceira");

            return View(model);
        }

        [Route("editar-propriedade-parceira/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<PropriedadeParceiraViewModel>(await _propriedadeParceiraService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("editar-propriedade-parceira/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, PropriedadeParceiraViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _propriedadeParceiraService.Atualizar(_mapper.Map<PropriedadeParceira>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Propriedade Parceira Editada com Sucesso!", "Index", "PropriedadeParceira");

            return View(model);
        }

        [Route("remover-propriedade-parceira/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<PropriedadeParceiraViewModel>(await _propriedadeParceiraService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-propriedade-parceira/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, PropriedadeParceiraViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _propriedadeParceiraService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Propriedade Parceira Removida com Sucesso!", "Index", "PropriedadeParceira");

            return View(model);
        }

        [Route("buscar-propriedade-parceira")]
        public async Task<ActionResult> BuscarQuery([FromQuery] string query)
        {            
            if (String.IsNullOrEmpty(query)) return Json(new { });

            var propriedadesParceiras = await _propriedadeParceiraService.Buscar(x => x.Nome.ToUpper().StartsWith(query.ToUpper()));

            return Json(new { list = _mapper.Map<IEnumerable<PropriedadeParceiraViewModel>>(propriedadesParceiras) });
        }

    }
}
