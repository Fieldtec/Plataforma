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
using PlataformaWeb.Business.Models.Cadastro;
using PlataformaWeb.Business.Notificacoes;
using PlataformaWeb.WebApp.Extensions;
using PlataformaWeb.WebApp.Models;

namespace PlataformaWeb.WebApp.Controllers
{
    [Authorize]
    public class SaidaAnimalController : MainController
    {
        private readonly ILoteSaidaService _loteSaidaService;
        private readonly IPastoCurralService _pastoCurralService;
        private readonly IMapper _mapper;

        public SaidaAnimalController(INotificador notificador,
                        IUser appUser,
                        ILoteSaidaService loteSaidaService,
                        IMapper mapper, 
                        IPastoCurralService pastoCurralService) : base(notificador, appUser)
        {
            _loteSaidaService = loteSaidaService;
            _mapper = mapper;
            _pastoCurralService = pastoCurralService;
        }

        [Route("saidas-animais")]
        public async Task<ActionResult> Index()
        {
            var saidas = await _loteSaidaService.Buscar(x => x.QuantidadeAnimaEmbarcado > 0);
            return View(saidas.OrderByDescending(x => x.DataEmbarque).ToList());
        }

        [Route("detalhe-saida-animal/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<SaidaAnimalCadastroViewModel>(await _loteSaidaService.ObterSaidaAnimal(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("nova-saida-animal")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create()
        {
            var lotes = await _loteSaidaService.Buscar(x => x.QuantidadeAnimaEmbarcado == 0);
            ViewBag.lotes = lotes.OrderBy(x => x.NumeroLote).ToList();

            ViewBag.locais = await _pastoCurralService.BuscarLocaisAtivos();

            return View();
        }

        [HttpPost("nova-saida-animal")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create([FromBody] SaidaAnimalCadastroViewModel model)
        {
            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var saidaAnimal = _mapper.Map<SaidaAnimalCadastro>(model);

            await _loteSaidaService.LancarSaida(saidaAnimal);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(model);
        }

        [Route("editar-saida-animal/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<SaidaAnimalCadastroViewModel>(await _loteSaidaService.ObterSaidaAnimal(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("editar-saida-animal/{id:int}")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, [FromBody] SaidaAnimalCadastroViewModel model)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var saidaAnimal = _mapper.Map<SaidaAnimalCadastro>(model);

            await _loteSaidaService.AtualizarSaida(saidaAnimal);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(model);
        }

        [Route("remover-saida-animal/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<SaidaAnimalCadastroViewModel>(await _loteSaidaService.ObterSaidaAnimal(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-saida-animal/{id:int}")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, [FromBody] SaidaAnimalCadastroViewModel model)
        {
            if (id != model.Id) return NotFound();

            var saidaAnimal = _mapper.Map<SaidaAnimalCadastro>(model);

            await _loteSaidaService.ExcluirSaida(saidaAnimal);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(model);
        }
    }
}
