using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class RacaoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataFormulacao { get; set; }
        public TipoRacao Tipo { get; set; }
        public decimal? Gmd { get; set; }
        public decimal MateriaSeca { get; set; }
        public decimal? ValorKg { get; set; }
        public string NomePropriedade { get; set; }
        public string Proprietario { get; set; }
        public string Tecnico { get; set; }
    }
}
