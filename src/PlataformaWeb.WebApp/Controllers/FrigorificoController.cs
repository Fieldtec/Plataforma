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
    public class FrigorificoController : MainController
    {
        private readonly IFrigorificoService _frigorificoService;
        private readonly IMapper _mapper;

        public FrigorificoController(INotificador notificador, 
                                     IUser appUser, 
                                     IFrigorificoService frigorificoService, 
                                     IMapper mapper) : base(notificador, appUser)
        {
            _frigorificoService = frigorificoService;
            _mapper = mapper;
        }

        [Route("frigorificos")]
        public async Task<ActionResult> Index()
        {
            return View(await _frigorificoService.ObterPaginacao());
        }

        [Route("detalhe-frigorifico/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<FrigorificoViewModel>(await _frigorificoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("novo-frigorifico")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("novo-frigorifico")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create(FrigorificoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _frigorificoService.Adicionar(_mapper.Map<Frigorifico>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Frigorifico Cadastrado com Sucesso!", "Index", "Frigorifico");

            return View(model);
        }

        [Route("editar-frigorifico/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<FrigorificoViewModel>(await _frigorificoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("editar-frigorifico/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, FrigorificoViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _frigorificoService.Atualizar(_mapper.Map<Frigorifico>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Frigorifico Editado com Sucesso!", "Index", "Frigorifico");

            return View(model);
        }

        [Route("remover-frigorifico/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<FrigorificoViewModel>(await _frigorificoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-frigorifico/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, FrigorificoViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _frigorificoService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Frigorifico Removido com Sucesso!", "Index", "Frigorifico");

            return View(model);
        }

        [Route("buscar-frigorifico")]
        public async Task<ActionResult> BuscarQuery([FromQuery] string query)
        {
            if (String.IsNullOrEmpty(query)) return Json(new { });

            var fases = await _frigorificoService.Buscar(x => x.Nome.ToUpper().StartsWith(query.ToUpper()));

            return Json(new { list = fases });
        }
    }
}
