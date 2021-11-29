using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class PlanejamentoNutricionalDTO 
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public TipoPlanejamentoNutricional Tipo { get; set; }
        public string NomePropriedade { get; set; }
        public string Proprietario { get; set; }
        public string Tecnico { get; set; }
        public PlanejamentoNutricionalDTO()
        {  }
    }
}
