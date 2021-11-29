using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class FornecimentoPastoViewModel
    {
        public int Id { get; set; }
        public DateTime DataRealizado { get; set; }
        public int IdPasto { get; set; }
        public int IdLote { get; set; }
        public int QuantidadeAnimais { get; set; }
        public int IdSuplemento { get; set; }
        public decimal PrevisaoKg { get; set; }
        public decimal PrevisaoSaco { get; set; }
        public decimal RealizadoKg { get; set; }
        public decimal RealizadoSaco { get; set; }
        public OrigemSuplemento Origem { get; set; }
        public DestinoSuplemento Destino { get; set; }
    }
}
