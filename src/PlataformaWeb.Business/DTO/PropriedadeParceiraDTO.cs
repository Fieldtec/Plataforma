using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class PropriedadeParceiraDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string NomePropriedade { get; set; }
        public string Proprietario { get; set; }
        public string Tecnico { get; set; }

        public PropriedadeParceiraDTO()
        {  }
    }
}
