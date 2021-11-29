using System;
using System.Collections.Generic;

namespace PlataformaWeb.Business.Models
{
    public class Frigorifico : Entity
    {
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual List<LoteSaida> LotesSaida { get; set; }

        public Frigorifico()
        {
            LotesSaida = new List<LoteSaida>();
        }
    }


}
