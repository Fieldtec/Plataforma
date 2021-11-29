using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models
{
    public class LeituraCocho : Entity
    {
        public int IdLocal { get; set; }
        public PastoCurral Local { get; set; }
        public int IdLote { get; set; }
        public LoteEntrada LoteEntrada { get; set; }
        public int IdPlanejamento { get; set; }
        public PlanejamentoNutricional Planejamento { get; set; }
        public DateTime DataLeitura { get; set; }
        public decimal AjusteGramas { get; set; }
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; }
        public int QuantidadeAnimais { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
    }
}
