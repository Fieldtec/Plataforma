using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models
{
    public class CausaMorte : Entity
    {
        public string Nome { get; set; }        
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual List<Animal> Animais { get; set; }

        public CausaMorte()
        {
            Animais = new List<Animal>();
        }
    }
}
