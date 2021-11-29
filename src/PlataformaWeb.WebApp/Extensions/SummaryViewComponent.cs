using Microsoft.AspNetCore.Mvc;
using PlataformaWeb.Business.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly INotificador _notificador;

        public SummaryViewComponent(INotificador notificador)
        {
            _notificador = notificador;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notificacoes = await Task.FromResult(_notificador.ObterNotificacoes());

            foreach (var notificacao in notificacoes)            
                ViewData.ModelState.AddModelError(string.Empty, notificacao.Mensagem);

            return View();
        }
    }
}
