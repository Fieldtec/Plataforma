using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaWeb.Business.DTO;
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
    public class PlanejamentoNutricionalController : MainController
    {
        private readonly IPlanejamentoNutricionalService _planejamentoNutricionalService;
        private readonly IRacaoService _racaoService;
        private readonly ICategoriaService _categoriaService;
        private readonly ISuplementoMineralService _suplementoMineralService;
        private readonly IFaseDoAnoService _faseDoAnoService;
        private readonly IMapper _mapper;
        public PlanejamentoNutricionalController(INotificador notificador,
                                                IUser appUser,
                                                IPlanejamentoNutricionalService planejamentoNutricionalService,
                                                IMapper mapper, IRacaoService racaoService,
                                                ICategoriaService categoriaService,
                                                ISuplementoMineralService suplementoMineralService,
                                                IFaseDoAnoService faseDoAnoService) : base(notificador, appUser)
        {
            _planejamentoNutricionalService = planejamentoNutricionalService;
            _mapper = mapper;
            _racaoService = racaoService;
            _categoriaService = categoriaService;
            _suplementoMineralService = suplementoMineralService;
            _faseDoAnoService = faseDoAnoService;
        }

        [Route("planejamentos")]
        public async Task<ActionResult> Index()
        {
            return View(await _planejamentoNutricionalService.ObterPaginacao());
        }

        [Route("detalhe-planejamento/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<PlanejamentoNutricionalViewModel>(await _planejamentoNutricionalService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("novo-planejamento")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create()
        {
            await CarregarDados();

            return View();
        }

        [HttpPost("novo-planejamento")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create([FromBody] PlanejamentoNutricionalViewModel model)
        {
            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var planejamentoNutricional = _mapper.Map<PlanejamentoNutricional>(model);

            await _planejamentoNutricionalService.Adicionar(planejamentoNutricional);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(_mapper.Map<PlanejamentoNutricionalViewModel>(planejamentoNutricional));
        }

        [Route("editar-planejamento/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<PlanejamentoNutricionalViewModel>(await _planejamentoNutricionalService.ObterPorId(id));

            if (model is null) return NotFound();

            await CarregarDados();

            return View(model);
        }

        [Route("gerenciar-planejamentos/{idPlanejamento:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Gerenciar(int idPlanejamento)
        {
            if (idPlanejamento <= 0) return NotFound();

            var lotes = await _planejamentoNutricionalService.BuscarLotesGerenciamento(idPlanejamento);

            return CustomJsonResponse(lotes);
        }

        [Route("gerenciar-planejamentos")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Gerenciar()
        {
            var planejamentos = await _planejamentoNutricionalService.Buscar(x => x.Status == Status.Ativado);
            ViewBag.planejamentos = planejamentos.OrderBy(x => x.Nome).ToList();
            return View();
        }

        [HttpPost("gerenciar-planejamentos")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Gerenciar([FromBody] List<GerenciarPlanejamentoViewModel> models)
        {
            if (models == null) return NotFound();

            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var planejamentos = _mapper.Map<List<GerenciarPlanejamentoDTO>>(models);

            await _planejamentoNutricionalService.AlterarPlanejamentosNosLotes(planejamentos);

            return CustomJsonResponse();
        }


        [HttpPost("editar-planejamento/{id:int}")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, [FromBody] PlanejamentoNutricionalViewModel model)
        {
            if (id != model.Id)
            {
                AdicionarNotificacao("Requisição inválida");
                return CustomJsonResponse();
            }

            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var planejamentoNutricional = _mapper.Map<PlanejamentoNutricional>(model);

            await _planejamentoNutricionalService.Atualizar(planejamentoNutricional);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(_mapper.Map<PlanejamentoNutricionalViewModel>(planejamentoNutricional));
        }

        [Route("remover-planejamento/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<PlanejamentoNutricionalViewModel>(await _planejamentoNutricionalService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-planejamento/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, PlanejamentoNutricionalViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _planejamentoNutricionalService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Planejamento Removido com Sucesso!", "Index", "PlanejamentoNutricional");

            return View(model);
        }

        [Route("buscar-planejamento")]
        public async Task<ActionResult> BuscarQuery([FromQuery] string query, [FromQuery] TipoPlanejamentoNutricional tipo)
        {
            //if (String.IsNullOrEmpty(query)) return Json(new { });
            var where = PredicateBuilder.True<PlanejamentoNutricional>().And(x => x.Tipo == tipo);

            if (!String.IsNullOrEmpty(query))
                where = where.And(x => x.Nome.ToUpper().StartsWith(query.ToUpper()));

            var racoes = await _planejamentoNutricionalService.Buscar(where);

            return Json(new { list = racoes });
        }

        public async Task CarregarDados()
        {
            ViewBag.racoes = await _racaoService.Buscar(x => x.Status == Status.Ativado);
            ViewBag.categorias = await _categoriaService.Buscar(x => x.Status == Status.Ativado);
            ViewBag.suplementos = await _suplementoMineralService.Buscar(x => x.Status == Status.Ativado);
            ViewBag.faseAno = await _faseDoAnoService.Buscar(x => x.Status == Status.Ativado);
        }

    }
}
