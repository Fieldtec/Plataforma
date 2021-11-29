using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class MorteAnimalDTO
    {
        public DateTime DataMorte { get; set; }
        public int QuantidadeAnimais { get; set; }
        public string CausaMorte { get; set; }
        public string LocalOrigem { get; set; }
        public int IdLote { get; set; }
    }
}
