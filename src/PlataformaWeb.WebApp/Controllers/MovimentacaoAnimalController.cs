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
    public class MovimentacaoAnimalController : MainController
    {
        private readonly IMovimentacaoAnimalService _service;
        private readonly IMotivoMovimentacaoService _motivoMovimentacaoService;
        private readonly IPastoCurralService _pastoCurralService;
        private readonly IMapper _mapper;

        public MovimentacaoAnimalController(INotificador notificador,
                    IUser appUser,
                    IMovimentacaoAnimalService service,
                    IMapper mapper,
                    IMotivoMovimentacaoService motivoMovimentacaoService, IPastoCurralService pastoCurralService) : base(notificador, appUser)
        {
            _service = service;
            _mapper = mapper;
            _motivoMovimentacaoService = motivoMovimentacaoService;
            _pastoCurralService = pastoCurralService;
        }

        private bool ValidaParametros(int idLocalOrigem, int idLocalDestino, int idLoteOrigem, int idLoteDestino, long dataMov, MovimentacaoAnimalDTO model = null)
        {
            if (idLocalOrigem == 0 || idLocalDestino == 0
                || idLoteOrigem == 0 || idLoteDestino == 0 || new DateTime(dataMov) == DateTime.MinValue) return false;

            if (model != null)
            {
                if (idLocalOrigem != model.IdLocalOrigem || idLocalDestino != model.IdLocalDestino
                    || idLoteOrigem != model.IdLoteOrigem || idLoteDestino != model.IdLoteDestino || new DateTime(dataMov).CompareTo(model.DataMovimentacao) != 0) return false;
            }

            return true;
        }

        [Route("movimentacoes-animal")]
        public async Task<ActionResult> Index()
        {
            return View(await _service.ObterPaginacao());
        }

        [Route("detalhe-movimentacao-animal/{idLocalOrigem:int}/{idLocalDestino:int}/{idLoteOrigem:int}/{idLoteDestino:int}/{dataMov:long}")]
        public async Task<ActionResult> Details(int idLocalOrigem, int idLocalDestino, int idLoteOrigem, int idLoteDestino, long dataMov)
        {

            if (!ValidaParametros(idLocalOrigem, idLocalDestino, idLoteOrigem, idLoteDestino, dataMov)) return NotFound();

            var model = await _service.ObterMovimentacao(idLocalOrigem, idLocalDestino, idLoteOrigem, idLoteDestino, new DateTime(dataMov));

            if (model is null) return NotFound();

            return View(model);
        }

        [Route("nova-movimentacao-animal")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create()
        {
            ViewBag.motivos = await _motivoMovimentacaoService.ObterPaginacao();
            ViewBag.locais = await _pastoCurralService.Buscar(x => x.Status == Status.Ativado);

            return View();
        }

        [HttpPost("nova-movimentacao-animal")]
        [IgnoreAntiforgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Create([FromBody] MovimentacaoAnimalViewModel model)
        {
            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            var movimentacao = _mapper.Map<MovimentacaoAnimalCadastro>(model);

            await _service.Adicionar(movimentacao);

            if (OperacaoInvalida()) return CustomJsonResponse();

            return CustomJsonResponse(model);
        }
        
        [Route("remover-movimentacao-animal/{idLocalOrigem:int}/{idLocalDestino:int}/{idLoteOrigem:int}/{idLoteDestino:int}/{dataMov:long}")]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int idLocalOrigem, int idLocalDestino, int idLoteOrigem, int idLoteDestino, long dataMov)
        {
            if (!ValidaParametros(idLocalOrigem, idLocalDestino, idLoteOrigem, idLoteDestino, dataMov)) return NotFound();

            var model = await _service.ObterMovimentacao(idLocalOrigem, idLocalDestino, idLoteOrigem, idLoteDestino, new DateTime(dataMov));

            if (model is null) return NotFound();

            return View(model);
        }

        [HttpPost("remover-movimentacao-animal/{idLocalOrigem:int}/{idLocalDestino:int}/{idLoteOrigem:int}/{idLoteDestino:int}/{dataMov:long}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(Role.Tecnico, Role.Cliente, Role.UsuarioCliente)]
        public async Task<ActionResult> Delete(int idLocalOrigem, int idLocalDestino, int idLoteOrigem, int idLoteDestino, long dataMov, MovimentacaoAnimalDTO model)
        {
            if (!ValidaParametros(idLocalOrigem, idLocalDestino, idLoteOrigem, idLoteDestino, dataMov, model)) return NotFound();

            await _service.Remover(idLocalOrigem, idLocalDestino, idLoteOrigem, idLoteDestino, new DateTime(dataMov));

            if (OperacaoInvalida()) return View(model);

            RedirecionaPara("Movimentação excluída com Sucesso!", "Index", "MovimentacaoAnimal");

            return View(model);
        }

    }
}
