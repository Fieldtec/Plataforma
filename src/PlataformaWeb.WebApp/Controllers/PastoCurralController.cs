using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class PastoCurralController : MainController
    {
        private readonly IPastoCurralService _pastoCurralService;
        private readonly IMapper _mapper;

        public PastoCurralController(INotificador notificador, 
                                     IUser appUser, 
                                     IPastoCurralService pastoCurralService, 
                                     IMapper mapper) : base(notificador, appUser)
        {
            _pastoCurralService = pastoCurralService;
            _mapper = mapper;
        }

        [Route("currais-pastos/{idCliente:int?}")]
        public async Task<ActionResult> Index()
        {
            return View(await _pastoCurralService.ObterPaginacao());
        }

        //private async Task<bool> ValidaQueryStringCliente(int idCliente)
        //{
        //    var cliente = await _clienteService.ObterPorId(idCliente);

        //    if (cliente is null) return false;

        //    ViewBag.NomeProprietario = cliente.NomePropriedade;
        //    ViewBag.IdCliente = cliente.Id;

        //    return true;
        //}


        // GET: ClienteController/Details/5
        [Route("detalhe-curral-pasto/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<PastoCurralViewModel>(await _pastoCurralService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        // GET: ClienteController/Create
        [Route("novo-curral-pasto")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClienteController/Create
        [HttpPost("novo-curral-pasto")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create(PastoCurralViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _pastoCurralService.Adicionar(_mapper.Map<PastoCurral>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Pasto/Curral Cadastrado com Sucesso!", "Index", "PastoCurral");

            return View(model);
        }

        // GET: ClienteController/Edit/5
        [Route("editar-curral-pasto/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<PastoCurralViewModel>(await _pastoCurralService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        // POST: ClienteController/Edit/5
        [HttpPost("editar-curral-pasto/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, PastoCurralViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            await _pastoCurralService.Atualizar(_mapper.Map<PastoCurral>(model));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Pasto/Curral Editado com Sucesso!", "Index", "PastoCurral");

            return View(model);
        }

        // GET: ClienteController/Delete/5
        [Route("remover-curral-pasto/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<PastoCurralViewModel>(await _pastoCurralService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        // POST: ClienteController/Delete/5
        [HttpPost("remover-curral-pasto/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, PastoCurralViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _pastoCurralService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Pasto/Curral Removido com Sucesso!", "Index", "PastoCurral");

            return View(model);
        }


        [Route("buscar-curral-pasto")]
        public async Task<ActionResult> BuscarQuery([FromQuery] string query, [FromQuery] TipoPastoCurral? tipo, TipoLotacaoLocal lotacao = TipoLotacaoLocal.Todos)
        {

            var where = PredicateBuilder.True<PastoCurral>();

            if (!String.IsNullOrEmpty(query))
               where = where.And(x => x.Nome.ToUpper().StartsWith(query.ToUpper()));

            if (tipo.HasValue)
                where = where.And(x => x.Tipo == tipo.Value);

            if (lotacao == TipoLotacaoLocal.SemLotacao)
                where = where.And(x => !x.Lotacao.HasValue || x.Lotacao == 0);
            else if (lotacao == TipoLotacaoLocal.ComLotacao)
                where = where.And(x => x.Lotacao > 0);

            var locais = await _pastoCurralService.Buscar(where);

            return Json(new { list = locais });
        }

    }
}
