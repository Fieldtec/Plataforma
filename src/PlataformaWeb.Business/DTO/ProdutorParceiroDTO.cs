using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class ProdutorParceiroDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string NomePropriedadeParceira { get; set; }
        public string Proprietario { get; set; }
        public string NomePropriedade { get; set; }
        public string Tecnico { get; set; }
    }
}
