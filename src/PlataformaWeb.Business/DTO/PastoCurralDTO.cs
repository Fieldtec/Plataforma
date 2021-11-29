using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class PastoCurralDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public TipoPastoCurral Tipo { get; set; }
        public int Capacidade { get; set; }
        public int? Lotacao { get; set; }
        public string Proprietario { get; set; }
        public string NomePriedade { get; set; }
        public string Tecnico { get; set; }

        public PastoCurralDTO()
        {  }
        
    }

    public class LocalLoteDTO
    {
        public int Id { get; set; }
        public int IdLote { get; set; }
        public string Nome { get; set; }
        public int Lotacao { get; set; }
    }
}
