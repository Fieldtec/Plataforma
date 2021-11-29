using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Notificacoes;
using PlataformaWeb.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Controllers
{
    [Authorize]
    public class RelatoriosController : MainController
    {
        private readonly IRelatorioService _relatorioService;
        private readonly IRacaService _racaService;
        private readonly ICategoriaService _categoriaService;

        public RelatoriosController(INotificador notificador, IUser appUser,
            IRelatorioService relatorioService, 
            IRacaService racaService,
            ICategoriaService categoriaService)
            : base(notificador, appUser)
        {
            _relatorioService = relatorioService;
            _racaService = racaService;
            _categoriaService = categoriaService;
        }

        [HttpPost("imprimir-relatorio")]
        [IgnoreAntiforgeryToken]        
        public async Task<IActionResult> ImprimirRelatorio([FromBody] FiltroRelatorioViewModel filtros)
        {
            if (!ModelState.IsValid) return CustomJsonResponse(ModelState);

            return CustomJsonResponse(await _relatorioService.Imprimir(filtros.NomeRelatorio, filtros.Filtro));
        }

        public async Task<IActionResult> ListaAnimal()
        {
            ViewBag.racas = await _racaService.Buscar(x => x.Status == Status.Ativado);
            ViewBag.categorias = await _categoriaService.Buscar(x => x.Status == Status.Ativado);

            return PartialView();
        }
    }
}
