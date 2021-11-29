using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlataformaWeb.Console
{
    class Program
    {
        static void CallWebRequest()
        {
            WebRequest request = WebRequest.Create("http://18.191.14.117:8099/GeraReport");
            request.Method = "POST";
            request.Proxy = null;
            RelatorioDTO report = new RelatorioDTO("ListaAnimal", 15, "");
            byte[] byteArray = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(report));
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            
            System.Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                System.Console.WriteLine(responseFromServer);
            }

            // Close the response.
            response.Close();
        }


        static async Task Main(string[] args)
        {
            CallWebRequest();

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            //var client = new HttpClient(handler);

            HttpWebRequest client = (HttpWebRequest)WebRequest.Create("http://18.191.14.117:8099/GeraReport");
            client.Method = "POST";
            client.Proxy = null;
            

            HttpClient _httpClient = new HttpClient(handler);
            String url = "http://localhost:8099";
            System.Console.WriteLine(url);
            //_httpClient.BaseAddress = new Uri(url);
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
            
            


            var dados = ObterConteudo("ListaAnimal", "");

            System.Console.WriteLine("Chamando");

            //var response = Task.Run(() => _httpClient.PostAsync("/GeraReport", dados).Wait());

            var response = _httpClient.PostAsync("http://18.191.14.117:8099/GeraReport", dados).Result;

            //var response = await _httpClient.PostAsync("/GeraReport", dados).ConfigureAwait(false);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.Console.WriteLine("Chamou");
                var result = await DeserializarObjetoResponse<RelatorioResponseResultDTO>(response);
                System.Console.WriteLine(result.Mensagem);
            } else
            {
                System.Console.WriteLine("Deu Ruim");
            }

            System.Console.ReadKey();
        }

        static StringContent ObterConteudo(string nomeRelatorio, string filtros)
        {
            RelatorioDTO report = new RelatorioDTO(nomeRelatorio, 15, filtros ?? "");

            //object dados = 
            //    $"{{ \"Report\": " +
            //    $" [" +
            //    $"   {{ " +
            //    $"     \"reportname\": \"{nomeRelatorio}\", " +
            //    $"     \"idcliente\": \"{AppUser.ObterIdCliente()}\", " +
            //    $"     \"filtro\": \"{filtros ?? ""}\"  " +
            //    $"   }} " +
            //    $" ] " +
            //    $"}}";

            return ObterConteudo(report);
        }

        static StringContent ObterConteudo(object dado)
        {
            return new StringContent(
                JsonSerializer.Serialize(dado),
                Encoding.UTF8,
                "application/json");
        }

        static async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false), options);
        }

    }




    public class RelatorioDTO
    {
        public List<RelatorioParametrosDTO> Report { get; set; }

        public RelatorioDTO(string nomeRelatorio, int idCliente, string filtro)
        {
            this.Report = new List<RelatorioParametrosDTO>
            {
                new RelatorioParametrosDTO { reportname = nomeRelatorio, filtro = filtro, idcliente = idCliente }
            };
        }
    }

    public class RelatorioParametrosDTO
    {
        public string reportname { get; set; }
        public int idcliente { get; set; }
        public string filtro { get; set; }
    }

    public class RelatorioResponseResultDTO
    {
        public string Mensagem { get; set; }
    }
}
