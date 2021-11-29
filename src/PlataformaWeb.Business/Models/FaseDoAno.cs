using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models
{
    public class FaseDoAno : Entity
    {
        public FaseDoAno()
        {
            PlanejamentoValoresPasto = new List<PlanejamentoValoresPasto>();
        }

        public string Nome { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual List<PlanejamentoValoresPasto> PlanejamentoValoresPasto { get; set; }
    }
}
