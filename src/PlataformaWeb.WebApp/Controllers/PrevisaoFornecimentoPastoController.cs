using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Notificacoes;
using PlataformaWeb.WebApp.Extensions;
using PlataformaWeb.WebApp.Models;

namespace PlataformaWeb.WebApp.Controllers
{
    [Authorize]
    public class PrevisaoFornecimentoPastoController : MainController
    {
        private readonly IPrevisaoFornecimentoPastoService _service;
        private readonly IPastoCurralService _pastoCurralService;
        private readonly ISuplementoMineralService _suplementoMineralService;
        private readonly IMapper _mapper;

        public PrevisaoFornecimentoPastoController(INotificador notificador,
            IUser appUser,
            IPrevisaoFornecimentoPastoService service,
            IPastoCurralService pastoCurralService,
            ISuplementoMineralService suplementoMineralService, 
            IMapper mapper) : base(notificador, appUser)
        {
            _service = service;
            _pastoCurralService = pastoCurralService;
            _suplementoMineralService = suplementoMineralService;
            _mapper = mapper;
        }

        //[ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        [Route("previsoes-fornecimentos-pasto")]
        public async Task<IActionResult> Index()
        {
            var currais = await _pastoCurralService.Buscar(x => (x.Lotacao.HasValue && x.Lotacao.Value > 0)
                   && x.Tipo == TipoPastoCurral.Pasto);

            ViewBag.pastos = currais.OrderBy(x => x.Nome).ToList();           

            return View();
        }

        [HttpPost("buscar-previsao-fornecimento-pasto")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> BuscarFiltro([FromBody] FiltroPrevisaoFornecimentoPastoDTO filtro)
        {
            return CustomJsonResponse(await _service.Buscar(filtro));
        }

        [Route("gerar-fornecimento-pasto")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<IActionResult> Gerar()
        {
            ViewBag.suplementos = await _suplementoMineralService.Buscar(x => x.Status == Status.Ativado);
            var previsaoConfiguracao = await _service.ObterConfiguracaoFornecimento();
            ViewBag.configuracao = _mapper.Map<ConfiguracaoFornecimentoPastoViewModel>(previsaoConfiguracao);

            return View();
        }

        [HttpPost("gerar-fornecimento-pasto")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<IActionResult> Gerar([FromBody] GeracaoFornecimentoPastoDTO model)
        {
            if (model is null) return BadRequest();

            await _service.GerarPrevisoes(model);

            return CustomJsonResponse(model);
        }

        //[HttpPost("existe-fornecimento-pasto")]
        //[IgnoreAntiforgeryToken]
        //[ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        //public async Task<IActionResult> Existe([FromBody] GeracaoFornecimentoPastoDTO model)
        //{
        //    if (model is null) return BadRequest();

        //    await _service.GerarPrevisoes(model);

        //    return CustomJsonResponse(model);
        //}

        [HttpPost("remover-previsao-fornecimento-pasto")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<IActionResult> Remover([FromBody] List<PrevisaoFornecimentoPastoDTO> models)
        {
            if (models is null) return BadRequest();

            await _service.RemoverPrevisoes(models);

            return CustomJsonResponse(models);
        }

    }
}
