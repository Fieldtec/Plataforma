using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;

namespace PlataformaWeb.Business.Models
{
    public class PlanejamentoNutricional : Entity
    {
        public PlanejamentoNutricional()
        {
            PlanejamentoValoresConfinamento = new List<PlanejamentoValoresConfinamento>();
            PlanejamentoValoresPasto = new List<PlanejamentoValoresPasto>();
            LotesEntrada = new List<LoteEntrada>();
            LeiturasCocho = new List<LeituraCocho>();
        }

        public TipoPlanejamentoNutricional Tipo { get; set; }
        public string Nome { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual List<PlanejamentoValoresConfinamento> PlanejamentoValoresConfinamento { get; set; }
        public virtual List<PlanejamentoValoresPasto> PlanejamentoValoresPasto { get; set; }
        public virtual List<LoteEntrada> LotesEntrada { get; set; }
        public virtual List<LeituraCocho> LeiturasCocho { get; set; }
    }
}
