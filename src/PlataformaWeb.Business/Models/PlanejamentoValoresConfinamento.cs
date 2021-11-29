using System;
using System.Collections.Generic;

namespace PlataformaWeb.Business.Models
{
    public class PlanejamentoValoresConfinamento : Entity
    {
        public int IdPlanejamento { get; set; }
        public int IdRacao { get; set; }
        public int DiaInicio { get; set; }
        public decimal ImspvEsperado { get; set; }
        public decimal GmdEsperado { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int DiaFim { get; set; }
        public virtual PlanejamentoNutricional PlanejamentoNutricional { get; set; }
        public virtual Racao Racao { get; set; }
    }
}
