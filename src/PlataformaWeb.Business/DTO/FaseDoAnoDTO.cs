using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class FaseDoAnoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string NomePropriedade { get; set; }
        public string Proprietario { get; set; }
        public string Tecnico { get; set; }
    }
}
