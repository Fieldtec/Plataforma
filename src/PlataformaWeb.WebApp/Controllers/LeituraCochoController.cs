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
using PlataformaWeb.Business.Notificacoes;
using PlataformaWeb.WebApp.Extensions;

namespace PlataformaWeb.WebApp.Controllers
{
    [Authorize]
    public class LeituraCochoController : MainController
    {
        private readonly ILeituraCochoService _service;
        private readonly IPastoCurralService _pastoCurralService;
        private readonly INotaLeituraCochoService _notaLeituraService;
        private readonly IMapper _mapper;

        public LeituraCochoController(INotificador notificador,
            IUser appUser, ILeituraCochoService service, IMapper mapper,
            IPastoCurralService pastoCurralService, INotaLeituraCochoService notaLeituraService)
            : base(notificador, appUser)
        {
            _service = service;
            _mapper = mapper;
            _pastoCurralService = pastoCurralService;
            _notaLeituraService = notaLeituraService;
        }

        [Route("leituras-de-cocho")]
        public async Task<IActionResult> Index()
        {
            var currais = await _pastoCurralService.Buscar(x => (x.Lotacao.HasValue && x.Lotacao.Value > 0) 
                    && x.Tipo == TipoPastoCurral.Curral);

            ViewBag.currais = currais.OrderBy(x => x.Nome).ToList();

            return View();
        }

        [HttpPost("filtro-leituras-de-cocho")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Buscar([FromBody] FiltroLeituraCochoDTO filtro)
        {
            var leituras = await _service.ObterTodosFiltro(filtro);
            return CustomJsonResponse(leituras);
        }

        [Route("nova-leitura-de-cocho")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<IActionResult> Create()
        {
            ViewBag.notas = await _notaLeituraService.ObterTodos();

            return View();
        }

        [HttpPost("nova-leitura-de-cocho")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<IActionResult> BuscarLeituraData([FromBody] List<LeituraCochoInsercaoDTO> models)
        {
            if (models == null || models.Count == 0)
            {
                AdicionarNotificacao("Nenhuma informação enviada para ser salvar");
                return CustomJsonResponse();
            }

            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            await _service.InserirLeitura(models);

            return CustomJsonResponse(models);
        }

        [HttpPost("filtro-leitura-cocho")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> BuscarLeituraData([FromBody] DateTime data)
        {
            var leituras = await _service.ObterLeiturasInsercao(data);
            return CustomJsonResponse(leituras);
        }


    }
}
