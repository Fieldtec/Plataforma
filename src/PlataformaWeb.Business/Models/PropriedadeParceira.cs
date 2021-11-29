using System;
using System.Collections.Generic;

namespace PlataformaWeb.Business.Models
{
    public class PropriedadeParceira : Entity
    {
        public PropriedadeParceira()
        {
            ProdutoresParceiros = new List<ProdutorParceiro>();
        }

        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Inscricaoestadual { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual List<ProdutorParceiro> ProdutoresParceiros { get; set; }
    }
}
