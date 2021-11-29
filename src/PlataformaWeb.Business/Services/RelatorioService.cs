using Microsoft.Extensions.Options;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Notificacoes;
using PlataformaWeb.Business.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class RelatorioService : Service, IRelatorioService
    {
        private readonly HttpClient _httpClient;
        private readonly WebRequest _webClient;
        private readonly IOptions<AppSettings> _appSettings;

        public RelatorioService(INotificador notificador, 
            IUser appUser,
            HttpClient httpClient, 
            IOptions<AppSettings> appSettings) : base(notificador, appUser)
        {            
            _appSettings = appSettings;
            _webClient = WebRequest.Create(_appSettings.Value.RelatorioAPI);
            _webClient.Method = "POST";
            _webClient.Proxy = null;

            //_httpClient = httpClient;
            //_httpClient.BaseAddress = new Uri(_appSettings.Value.RelatorioAPI);
        }

        public async Task<string> Imprimir(string nomeRelatorio, string filtros)
        {
            if (!ValidarDados(nomeRelatorio)) return null;
            String caminhoRelatorio = null;
            if (!String.IsNullOrEmpty(filtros))
            {
                filtros = $" AND {filtros}";
            }

            var dados = ObterConteudo(nomeRelatorio, filtros);

            _webClient.ContentType = "application/json";
            _webClient.ContentLength = dados.Length;

            try
            {
                Stream dataStream = _webClient.GetRequestStream();
                dataStream.Write(dados, 0, dados.Length);
                dataStream.Close();
                WebResponse response = await _webClient.GetResponseAsync();

                var httpResponse = ((HttpWebResponse)response);
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);
                        string responseFromServer = reader.ReadToEnd();
                        var result = base.DeserializarObjetoResponse<RelatorioResponseResultDTO>(responseFromServer);
                        caminhoRelatorio = result.Mensagem;
                        var nomeArquivo = caminhoRelatorio.Split("\\").LastOrDefault();
                        caminhoRelatorio = $"{AppUser.ObterIdCliente()}/{nomeArquivo}";
                    }
                }
                else if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    Notificar("Nenhuma informação encontrada para gerar o relatório");
                }
                else
                {
                    await LogUtils.StoreAsync(new Exception("Não foi possível imprimir o relatório"), new { nomeRelatorio, filtros, response });
                    Notificar("Não foi possível imprimir o relatório");
                }

                response.Close();
            }
            catch (Exception)
            {
                Notificar("Nenhuma informação encontrada para gerar o relatório");
            }

            return caminhoRelatorio;
        }

        private byte[] ObterConteudo(string nomeRelatorio, string filtros)
        {
            RelatorioDTO report = new RelatorioDTO(nomeRelatorio, AppUser.ObterIdCliente(), filtros ?? "");
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(report));
        }

        private bool ValidarDados(string nomeRelatorio)
        {
            if (AppUser.ObterIdCliente() <= 0)
            {
                Notificar("Cliente não definido");
                return false;
            }

            if (String.IsNullOrEmpty(nomeRelatorio))
            {
                Notificar("Nome do Relatório não definido");
                return false;
            }
                
            //if (filtros is null)
            //{
            //    Notificar("Filtros do Relatório não definido");
            //    return false;
            //}

            return true;
        }

    }
}
