using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class RacaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CodigoBnd { get; set; }
        public string NomePropriedade { get; set; }
        public string Proprietario { get; set; }
        public string Tecnico { get; set; }
        public RacaDTO()
        { }
    }
}
