using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;

namespace PlataformaWeb.Business.Models
{
    public class Categoria : Entity
    {
        public Categoria()
        {
            PlanejamentoValoresPasto = new List<PlanejamentoValoresPasto>();
        }

        public string Nome { get; set; }
        public int? IdadeMinima { get; set; }
        public int? IdadeMaxima { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public string Sexo { get; set; }
        public virtual Cliente Cliente { get; set; } 
        public virtual List<PlanejamentoValoresPasto> PlanejamentoValoresPasto { get; set; }
        public virtual List<Animal> Animais { get; set; }
    }
}
