using System;
using System.Collections.Generic;

namespace PlataformaWeb.Business.Models
{
    public class ProdutorParceiro : Entity
    {
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public int IdPropriedadeParceira { get; set; }
        public virtual PropriedadeParceira PropriedadeParceira { get; set; } 
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual List<Animal> Animais { get; set; }
        public virtual List<LoteSaida> LotesSaida { get; set; }
        public ProdutorParceiro()
        {
            Animais = new List<Animal>();
            LotesSaida = new List<LoteSaida>();
        }
    }
}
