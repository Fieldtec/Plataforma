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
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Notificacoes;
using PlataformaWeb.WebApp.Extensions;
using PlataformaWeb.WebApp.Models;

namespace PlataformaWeb.WebApp.Controllers
{
    [Authorize]
    public class NotaLeituraCochoController : MainController
    {
        private readonly INotaLeituraCochoService _service;
        private readonly IMapper _mapper;
        public NotaLeituraCochoController(INotificador notificador,
            IUser appUser,
            INotaLeituraCochoService service,
            IMapper mapper) : base(notificador, appUser)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("notas-leitura-cocho")]
        public async Task<IActionResult> Index()
        {
            var notas = _mapper.Map<List<NotaLeituraCochoViewModel>>(await _service.ObterTodos());
            return View(notas);
        }

        [HttpPost("nova-nota-leitura-cocho")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit([FromBody] NotaLeituraCochoViewModel model)
        {
            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var nota = _mapper.Map<NotaLeituraCocho>(model);

            await _service.Adicionar(nota);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(_mapper.Map<NotaLeituraCochoViewModel>(nota));
        }

        [HttpPost("remover-nota-leitura-cocho")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete([FromBody] int id)
        {
            if (id <= 0) return NotFound();

            await _service.Remover(id);

            return CustomJsonResponse();
        }

        [HttpPost("editar-notas-leitura-cocho")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Edit([FromBody] List<NotaLeituraCochoViewModel> model)
        {
            if (model is null || model.Count() == 0)
            {
                AdicionarNotificacao("Nenhuma Informação foi enviada para ser alterada");
                return CustomJsonResponse();
            }

            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var notas = _mapper.Map<List<NotaLeituraCocho>>(model);

            await _service.Atualizar(notas);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(model);
        }

    }
}
