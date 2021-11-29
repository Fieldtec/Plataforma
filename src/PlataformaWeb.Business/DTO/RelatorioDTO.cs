using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
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
