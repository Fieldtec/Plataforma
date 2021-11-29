using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PlataformaWeb.Business.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly string _hostName;

        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
            _hostName = Environment.GetEnvironmentVariable("ASPNETCORE_URL_HOST") ?? String.Empty;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode == 404 && !httpContext.Response.HasStarted)
                {
                    httpContext.Response.Redirect($"{_hostName}/erro/404");
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);

                //if (_env.IsDevelopment())
                //    throw ex;
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            await LogUtils.StoreAsync(exception);

            //if (!_env.IsDevelopment()) 
            context.Response.Redirect($"{_hostName}/erro/500");
        }
    }
}
