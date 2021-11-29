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
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Notificacoes;
using PlataformaWeb.WebApp.Extensions;
using PlataformaWeb.WebApp.Models;

namespace PlataformaWeb.WebApp.Controllers
{
    [Authorize]
    public class FornecimentoPastoController : MainController
    {
        private readonly IFornecimentoPastoService _service;
        private readonly IMapper _mapper;
        private readonly IPastoCurralService _pastoCurralService;

        public FornecimentoPastoController(INotificador notificador,
            IUser appUser,
            IFornecimentoPastoService service,
            IMapper mapper, 
            IPastoCurralService pastoCurralService)
            : base(notificador, appUser)
        {
            _service = service;
            _mapper = mapper;
            _pastoCurralService = pastoCurralService;
        }

        [Route("fornecimentos-pasto")]
        public async Task<IActionResult> Index()
        {
            var currais = await _pastoCurralService.Buscar(x => (x.Lotacao.HasValue && x.Lotacao.Value > 0)
                   && x.Tipo == TipoPastoCurral.Pasto);

            ViewBag.pastos = currais.OrderBy(x => x.Nome).ToList();

            return View();
        }

        [Route("novo-fornecimento-pasto")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<IActionResult> Create()
        {
            var currais = await _pastoCurralService.Buscar(x => (x.Lotacao.HasValue && x.Lotacao.Value > 0)
                   && x.Tipo == TipoPastoCurral.Pasto);

            ViewBag.pastos = currais.OrderBy(x => x.Nome).ToList();

            return View();
        }

        [HttpPost("prepara-dados-fornecimento-pasto")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<IActionResult> PrepararDados([FromBody] FiltroFornecimentoPastoDTO filtro)
        {
            if (filtro is null) return BadRequest();

            return CustomJsonResponse(await _service.BuscarDadosLancamento(filtro));
        }

        [HttpPost("novo-fornecimento-pasto")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<IActionResult> Create([FromBody] FornecimentoPastoViewModel model)
        {
            if (model is null) return BadRequest();

            await _service.Adicionar(_mapper.Map<FornecimentoPasto>(model));

            return CustomJsonResponse(model);
        }


        [HttpPost("buscar-fornecimento-pasto")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> BuscarFiltro([FromBody] FiltroFornecimentoPastoDTO filtro)
        {
            return CustomJsonResponse(await _service.Buscar(filtro));
        }

        [HttpPost("remover-fornecimento-pasto")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<IActionResult> Remover([FromBody] List<FornecimentoPastoDTO> models)
        {
            if (models is null) return BadRequest();

            await _service.Remover(models);

            return CustomJsonResponse(models);
        }

    }
}
