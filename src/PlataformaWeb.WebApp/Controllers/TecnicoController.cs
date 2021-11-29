using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Notificacoes;
using PlataformaWeb.WebApp.Extensions;
using PlataformaWeb.WebApp.Models;

namespace PlataformaWeb.WebApp.Controllers
{
    [Authorize]
    [ClaimsAuthorize(Role.Adm)]
    public class TecnicoController : MainController
    {
        private readonly ITecnicoService _tecnicoService;
        private readonly IMapper _mapper;

        public TecnicoController(INotificador notificador,
                                 IUser appUser,
                                 ITecnicoService tecnicoService,
                                 IMapper mapper) : base(notificador, appUser)
        {
            _tecnicoService = tecnicoService;
            _mapper = mapper;
        }

        // GET: TecnicoController
        [Route("tecnicos")]
        //[ClaimsAuthorize(Role.Adm)]
        public async Task<ActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<TecnicoViewModel>>(await _tecnicoService.ObterTodos()));
        }

        // GET: TecnicoController/Details/5
        [Route("detalhe-tecnico/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<TecnicoViewModel>(await _tecnicoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        // GET: TecnicoController/Create
        [Route("novo-tecnico")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TecnicoController/Create
        [HttpPost("novo-tecnico")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TecnicoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _tecnicoService.Adicionar(_mapper.Map<Tecnico>(model));

            if (OperacaoInvalida()) return View(model);
                        
            RedirecionaPara("Técnico Cadastrado com Sucesso!", "Index", "Tecnico");

            return View(model);
        }

        // GET: TecnicoController/Edit/5
        [Route("editar-tecnico/{id:int}")]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<TecnicoViewModel>(await _tecnicoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        // POST: TecnicoController/Edit/5
        [HttpPost("editar-tecnico/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, TecnicoViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _tecnicoService.Atualizar(_mapper.Map<Tecnico>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Técnico Editado com Sucesso!", "Index", "Tecnico");

            return View(model);
        }

        // GET: TecnicoController/Delete/5
        [Route("remover-tecnico/{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<TecnicoViewModel>(await _tecnicoService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        // POST: TecnicoController/Delete/5
        [HttpPost("remover-tecnico/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, TecnicoViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _tecnicoService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Técnico Removido com Sucesso!", "Index", "Tecnico");

            return View(model);
        }
    }
}
