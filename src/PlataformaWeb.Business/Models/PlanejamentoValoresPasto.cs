using System;
using System.Collections.Generic;

namespace PlataformaWeb.Business.Models
{
    public partial class PlanejamentoValoresPasto : Entity
    {
        public int IdPlanejamento { get; set; }
        public int IdCategoria { get; set; }
        public int IdSuplemento { get; set; }
        public int IdFase { get; set; }
        public decimal ImspvEsperado { get; set; }
        public decimal GmdEsperado { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual FaseDoAno FaseDoAno { get; set; }
        public virtual PlanejamentoNutricional PlanejamentoNutricional { get; set; }
        public virtual SuplementoMineral SuplementoMineral { get; set; }
    }
}
