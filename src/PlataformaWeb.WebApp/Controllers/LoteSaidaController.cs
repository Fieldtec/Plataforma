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
    public class LoteSaidaController : MainController
    {
        private readonly ILoteSaidaService _loteSaidaService;
        private readonly IFrigorificoService _frigorificoService;
        private readonly IProdutorParceiroService _produtorParceiroService;
        private readonly IMapper _mapper;

        public LoteSaidaController(INotificador notificador,
                                   IUser appUser,
                                   ILoteSaidaService loteSaidaService,
                                   IMapper mapper, IFrigorificoService frigorificoService, IProdutorParceiroService produtorParceiroService) : base(notificador, appUser)
        {
            _loteSaidaService = loteSaidaService;
            _mapper = mapper;
            _frigorificoService = frigorificoService;
            _produtorParceiroService = produtorParceiroService;
        }

        [Route("lotes-de-saida")]
        public async Task<ActionResult> Index()
        {
            return View(await _loteSaidaService.ObterPaginacao());
        }

        [Route("detalhe-lote-saida/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<LoteSaidaViewModel>(await _loteSaidaService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("novo-lote-saida")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create()
        {
            await CarregarDados();

            return View();
        }

        [HttpPost("novo-lote-saida")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create([FromBody] LoteSaidaViewModel model)
        {
            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var loteAnimal = _mapper.Map<LoteSaida>(model);

            await _loteSaidaService.Adicionar(loteAnimal);

            if (OperacaoInvalida()) return CustomJsonResponse();            

            return CustomJsonResponse(_mapper.Map<LoteSaidaViewModel>(loteAnimal));
        }

        [Route("editar-lote-saida/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<LoteSaidaViewModel>(await _loteSaidaService.ObterPorId(id));

            if (model is null) return NotFound();

            await CarregarDados();

            return View(model);
        }

        [HttpPost("editar-lote-saida/{id:int}")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, [FromBody] LoteSaidaViewModel model)
        {
            if (id != model.Id) return BadRequest();            

            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var loteSaida = _mapper.Map<LoteSaida>(model);

            await _loteSaidaService.Atualizar(loteSaida);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(_mapper.Map<LoteSaidaViewModel>(loteSaida));
        }

        [Route("remover-lote-saida/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<LoteSaidaViewModel>(await _loteSaidaService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-lote-saida/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, LoteSaidaViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _loteSaidaService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Lote de Saída Removido com Sucesso!", "Index", "LoteSaida");

            return View(model);
        }

        [HttpGet("buscar-lote-saida")]
        public async Task<ActionResult> BuscarQuery()
        {
            var where = PredicateBuilder.True<LoteSaida>().And(x => x.QuantidadeAnimaEmbarcado == 0);           

            var lotes = await _loteSaidaService.Buscar(where);

            return Json(new { list = lotes.OrderBy(x => x.NumeroLote).ToList() });
        }

        private async Task CarregarDados()
        {
            ViewBag.frigorificos = await _frigorificoService.Buscar(x => x.Status == Status.Ativado);
            ViewBag.produtores = await _produtorParceiroService.Buscar(x => x.Status == Status.Ativado);
        }

    }
}
