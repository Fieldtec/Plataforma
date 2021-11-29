using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class GerenciarPlanejamentoDTO
    {
        public int IdPlanejamentoDestino { get; set; }
        public GerenciarPlanejamentoLoteDTO Lote { get; set; }
    }

    public class GerenciarPlanejamentoLoteDTO
    {
        public int IdLote { get; set; }
        public string Local { get; set; }
        public DateTime DataEntrada { get; set; }
        public int QuantidadeAnimais { get; set; }
        public int IdPlanejamento { get; set; }
        public TipoPlanejamentoNutricional Tipo { get; set; }
    }
}
