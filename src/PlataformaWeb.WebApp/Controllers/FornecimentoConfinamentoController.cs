using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Notificacoes;
using PlataformaWeb.WebApp.Extensions;

namespace PlataformaWeb.WebApp.Controllers
{

    [Authorize]
    public class FornecimentoConfinamentoController : MainController
    {
        private readonly IFornecimentoConfinamentoService _service;
        private readonly IPastoCurralService _pastoCurralService;
        private readonly IRacaoService _racaoService;

        public FornecimentoConfinamentoController(INotificador notificador,
            IUser appUser, IFornecimentoConfinamentoService service, 
            IRacaoService racaoService, 
            IPastoCurralService pastoCurralService)
            : base(notificador, appUser)
        {
            _service = service;
            _racaoService = racaoService;
            _pastoCurralService = pastoCurralService;
        }

        [Route("fornecimento-racao-confinamento")]
        public async Task<IActionResult> Index()
        {
            var currais = await _pastoCurralService.Buscar(x => (x.Lotacao.HasValue && x.Lotacao.Value > 0)
                    && x.Tipo == TipoPastoCurral.Curral);

            ViewBag.currais = currais.OrderBy(x => x.Nome).ToList();

            var racoes = await _racaoService.ObterPaginacao();
            ViewBag.racoes = racoes.OrderBy(x => x.Nome).ToList();

            return View();
        }

        [HttpPost("filtro-fornecimento")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Buscar([FromBody] FiltroFornecimentoConfinamentoDTO filtro)
        {
            var leituras = await _service.ObterTodosFiltro(filtro);
            return CustomJsonResponse(leituras);
        }

        [Route("confirmar-kg-fornecimento")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("recalcular-kg-previsto")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<IActionResult> RecalcularKgPrevisto([FromBody] int id)
        {
            if (id == 0)
            {
                AdicionarNotificacao("Nenhuma informação enviada para Recálculo.");
                return CustomJsonResponse();
            }

            var fornecimento = await _service.RecalcularPrevisto(id);

            return CustomJsonResponse(fornecimento);
        }

        [HttpPost("confirmar-kg-fornecimento")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<IActionResult> Create([FromBody] List<FornecimentoConfinamentoDTO> models)
        {
            if (models == null || models.Count == 0)
            {
                AdicionarNotificacao("Nenhuma informação enviada para ser salvar");
                return CustomJsonResponse();
            }

            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            await _service.InserirKgRealizado(models);

            return CustomJsonResponse(models);
        }

        [HttpPost("remover-kg-fornecimento")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<IActionResult> Delete([FromBody] List<FornecimentoConfinamentoDTO> models)
        {
            if (models == null || models.Count == 0)
            {
                AdicionarNotificacao("Nenhuma informação enviada para ser salvar");
                return CustomJsonResponse();
            }

            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            await _service.RemoverForncimentos(models);

            return CustomJsonResponse(models);
        }

        [HttpPost("filtro-data-fornecimento")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> BuscarFornecimentoData([FromBody] DateTime data)
        {
            var list = await _service.ObterPorData(data);
            return CustomJsonResponse(list);
        }
    }
}
