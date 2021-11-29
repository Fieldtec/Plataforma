using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaWeb.Business.DTO;
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
    public class MorteAnimalController : MainController
    {
        private readonly ILoteEntradaService _loteEntradaService;
        private readonly ICausaMorteService _causaMorteService;
        private readonly IPastoCurralService _pastoCurralService;
        private readonly IMapper _mapper;

        public MorteAnimalController(INotificador notificador,
                        IUser appUser,
                        ILoteEntradaService loteEntradaService,
                        ICausaMorteService causaMorteService,
                        IMapper mapper, IPastoCurralService pastoCurralService) : base(notificador, appUser)
        {
            _loteEntradaService = loteEntradaService;
            _causaMorteService = causaMorteService;
            _mapper = mapper;
            _pastoCurralService = pastoCurralService;
        }

        [Route("mortes-animais")]
        public async Task<ActionResult> Index()
        {
            return View(await _loteEntradaService.ObterMortesPaginacao());
        }

        [Route("novo-registro-morte")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create()
        {
            ViewBag.causas = await _causaMorteService.ObterPaginacao();
            ViewBag.locais = await _pastoCurralService.Buscar(x => x.Lotacao > 0);
            return View();
        }

        [HttpPost("novo-registro-morte")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create([FromBody] MorteAnimalViewModel model)
        {
            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var movimentacao = _mapper.Map<MorteAnimalCadastro>(model);

            await _loteEntradaService.RegistrarMortes(movimentacao);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(model);
        }

        [Route("detalhe-mortes-animais/{idLote:int}/{dtMorte:long}")]
        public async Task<ActionResult> Details(int idLote, long dtMorte)
        {

            if (!ValidaParametros(idLote, dtMorte)) return NotFound();

            var model = await _loteEntradaService.ObterMorte(idLote, new DateTime(dtMorte));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("remover-mortes-animais/{idLote:int}/{dtMorte:long}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int idLote, long dtMorte)
        {
            if (!ValidaParametros(idLote, dtMorte)) return NotFound();

            var model = await _loteEntradaService.ObterMorte(idLote, new DateTime(dtMorte));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-mortes-animais/{idLote:int}/{dtMorte:long}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int idLote, long dtMorte, MorteAnimalDTO model)
        {
            if (!ValidaParametros(idLote, dtMorte, model)) return NotFound();

            await _loteEntradaService.ExcluirMorte(model);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Mortes excluídas com Sucesso!", "Index", "MorteAnimal");

            return View(model);
        }


        private bool ValidaParametros(int idLote, long dataMorte, MorteAnimalDTO model = null)
        {
            if (idLote == 0 || new DateTime(dataMorte) == DateTime.MinValue) return false;

            if (model != null)
            {
                if (idLote != model.IdLote || new DateTime(dataMorte).Date.CompareTo(model.DataMorte.Date) != 0) 
                    return false;
            }

            return true;
        }

    }
}
