using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Notificacoes;

namespace PlataformaWeb.WebApp.Controllers
{
    public abstract class MainController : Controller
    {
        private readonly INotificador _notificador;
        protected readonly IUser AppUser;
        protected int IdUsuario { get; set; }

        protected MainController(INotificador notificador, IUser appUser)
        {
            _notificador = notificador;
            AppUser = appUser;

            if (AppUser.EstaAutenticado())
            {
                IdUsuario = AppUser.ObterId();
            }
        }

        protected T DeserializarObjeto<T>(string form)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(form, options);
        }

        protected JsonResult CustomJsonResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                AdicionarNotificacao(errorMsg);
            }

            return CustomJsonResponse();
        }


        protected JsonResult CustomJsonResponse(object data = null)
        {
            if (OperacaoInvalida())
            {
                return Json(new
                {
                    erros = _notificador.ObterNotificacoes().Select(x => x.Mensagem),
                    status = false
                });
            }

            return Json(new
            {
                data,
                status = true
            });
        }       


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        protected bool OperacaoInvalida()
        {
            return _notificador.TemNotificacao();            
        }

        protected void AdicionarNotificacao(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

        protected void RedirecionaPara(string mensagem, string action, string controller, string titulo = null, string tipoAlerta = null)
        {
            ViewBag.result = new
            {
                Titulo = titulo, 
                TipoAlerta = tipoAlerta,
                Mensagem = mensagem,
                Callback = Url.Action(action, controller, null, Request.Scheme)
            };
        }

    }
}
