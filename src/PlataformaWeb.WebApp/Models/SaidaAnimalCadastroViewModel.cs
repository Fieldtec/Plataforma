using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class SaidaAnimalCadastroViewModel
    {
        public int Id { get; set; }
        public int NumeroLote { get; set; }
        public TipoSaida TipoSaida { get; set; }
        public string Destino { get; set; }
        public int QuantidadeAnimalPrevisto { get; set; }
        public int QuantidadeAnimalEmbarcado { get; set; }
        public DateTime DataEmbarque { get; set; }
        public List<SaidaAnimalLoteViewModel> Lotes { get; set; }
    }

    public class SaidaAnimalLoteViewModel
    {
        public PastoCurralConsultaViewModel Local { get; set; }
        public LoteEntradaViewModel Lote { get; set; }
        public int QuantidadeEmbarcado { get; set; }
        public decimal PesoMedio { get; set; }
    }
}
