using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class GerenciarPlanejamentoViewModel
    {
        public int IdPlanejamentoDestino { get; set; }
        public GerenciarPlanejamentoLoteViewModel Lote { get; set; }
    }

    public class GerenciarPlanejamentoLoteViewModel
    {
        public int IdLote { get; set; }
        public string Local { get; set; }
        public DateTime DataEntrada { get; set; }
        public int QuantidadeAnimais { get; set; }
        public int IdPlanejamento { get; set; }
        public TipoPlanejamentoNutricional Tipo { get; set; }
    }

}
