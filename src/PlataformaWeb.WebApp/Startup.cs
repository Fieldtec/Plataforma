using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Data;
using PlataformaWeb.WebApp.Configuration;
using PlataformaWeb.WebApp.Extensions;

namespace PlataformaWeb.WebApp
{
    public class Startup
    {
        public Startup(IWebHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (hostEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityConfiguration(Configuration);

            services.AddDbContext<PlataformaFieldContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<List<NotasLeituraCochoSettings>>(options =>
            {
                Configuration.GetSection("NotasLeituraCochoSettings").Bind(options);
            });

            services.Configure<AppSettings>(options =>
            {
                Configuration.GetSection("AppSettings").Bind(options);
            });

            // AutoMapper Settings
            services.AddAutoMapperConfiguration();

            // MVC Settings
            services.AddMvcConfiguration();

            // Registrando todas as Injeções de Dependências
            services.RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
                string urlHost = Environment.GetEnvironmentVariable("ASPNETCORE_URL_HOST") ?? String.Empty;
                app.UseExceptionHandler($"{urlHost}/erro/500");
                //app.UseStatusCodePagesWithRedirects($"{urlHost}/erro/{0}");
                app.UseHsts();
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseGlobalizationConfig();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public static string GetAddressIP()
        {
            var hostname = Dns.GetHostName();

            return Dns.GetHostAddresses(Dns.GetHostName())
                .FirstOrDefault(ha => ha.AddressFamily == AddressFamily.InterNetwork)
                .ToString();
        }
    }
}
