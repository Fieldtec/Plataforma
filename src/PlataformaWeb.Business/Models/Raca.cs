using System;
using System.Collections.Generic;

namespace PlataformaWeb.Business.Models
{
    public class Raca : Entity
    {
        public string Nome { get; set; }
        public string CodigoBnd { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual List<Animal> Animais { get; set; }
        public Raca()
        {
            Animais = new List<Animal>();
        }
    }
}
