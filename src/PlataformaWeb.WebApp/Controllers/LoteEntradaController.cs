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
    public class LoteEntradaController : MainController
    {
        private readonly ILoteEntradaService _loteEntradaService;
        private readonly IPlanejamentoNutricionalService _planejamentoNutricionalService;
        private readonly IPastoCurralService _pastoCurralService;
        private readonly IProdutorParceiroService _produtorParceiroService;
        private readonly IRacaService _racaService;
        private readonly ICategoriaService _categoriaService;
        private readonly IMapper _mapper;


        public LoteEntradaController(INotificador notificador,
                                     IUser appUser,
                                     ILoteEntradaService loteEntradaService,
                                     IMapper mapper,
                                     IPlanejamentoNutricionalService planejamentoNutricionalService, 
                                     IPastoCurralService pastoCurralService, 
                                     IProdutorParceiroService produtorParceiroService, 
                                     IRacaService racaService, 
                                     ICategoriaService categoriaService) : base(notificador, appUser)
        {
            _loteEntradaService = loteEntradaService;
            _mapper = mapper;
            _planejamentoNutricionalService = planejamentoNutricionalService;
            _pastoCurralService = pastoCurralService;
            _produtorParceiroService = produtorParceiroService;
            _racaService = racaService;
            _categoriaService = categoriaService;
        }

        [Route("lotes-de-entrada")]
        public async Task<ActionResult> Index()
        {
            return View(await _loteEntradaService.ObterPaginacao());
        }

        [Route("detalhe-lote-entrada/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<LoteAnimalViewModel>(await _loteEntradaService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("novo-lote-entrada")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create()
        {
            await CarregarDados();

            return View();
        }

        [HttpPost("novo-lote-entrada")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create([FromBody] LoteAnimalViewModel model)
        {
            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var loteAnimal = _mapper.Map<LoteAnimalCadastro>(model);

            await _loteEntradaService.Adicionar(loteAnimal);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(model);
        }

        [Route("editar-lote-entrada/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();


            var model = _mapper.Map<LoteAnimalViewModel>(await _loteEntradaService.ObterPorId(id));


            if (model is null) return NotFound();

            await CarregarDados();
            ViewBag.planejamentos = await _planejamentoNutricionalService.Buscar(x => x.Tipo == (TipoPlanejamentoNutricional)model.Local.Tipo && x.Status == Status.Ativado);

            return View(model);
        }

        [HttpPost("editar-lote-entrada/{id:int}")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit(int id, [FromBody] LoteAnimalViewModel model)
        {
            if (id != model.Id)
            {
                AdicionarNotificacao("Requisição inválida");
                return CustomJsonResponse();
            }

            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var LoteAnimal = _mapper.Map<LoteAnimalCadastro>(model);

            await _loteEntradaService.Atualizar(LoteAnimal);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(model);
        }

        [Route("remover-lote-entrada/{id:int}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var model = _mapper.Map<LoteAnimalDeletarViewModel>(await _loteEntradaService.ObterPorId(id));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-lote-entrada/{id:int}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int id, LoteAnimalDeletarViewModel model)
        {
            if (id != model.Id) return NotFound();

            await _loteEntradaService.Remover(id);

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Lote de Entrada Removido com Sucesso!", "Index", "LoteEntrada");

            return View(model);
        }

        [Route("buscar-lote-ativo-local")]
        public async Task<ActionResult> BuscarLoteAtivoPorLocal([FromQuery] int idLocal)
        {
            if (idLocal == 0) return Json(new { });

            PlanejamentoNutricionalDTO planejamentoAtivo = null;
            var lote = await _loteEntradaService.Buscar(x => x.IdLocal == idLocal);

            if (lote.Count > 0)
            {
                var planejamentos = await _planejamentoNutricionalService.Buscar(x => x.Nome == lote.FirstOrDefault().Planejamento);
                planejamentoAtivo = planejamentos.FirstOrDefault();
            }

            return Json(new { lote, planejamentoAtivo });
        }
              
        public async Task CarregarDados()
        {
            ViewBag.locais = await _pastoCurralService.Buscar(x => x.Status == Status.Ativado);
            ViewBag.produtores = await _produtorParceiroService.Buscar(x => x.Status == Status.Ativado);
            ViewBag.racas = await _racaService.Buscar(x => x.Status == Status.Ativado);
            ViewBag.categorias = await _categoriaService.Buscar(x => x.Status == Status.Ativado);
        }

    }
}
