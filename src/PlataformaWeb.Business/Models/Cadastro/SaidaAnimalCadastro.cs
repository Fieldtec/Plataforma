using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Cadastro
{
    public class SaidaAnimalCadastro : Entity
    {
        public int NumeroLote { get; set; }
        public TipoSaida TipoSaida { get; set; }
        public string Destino { get; set; }
        public int QuantidadeAnimalPrevisto { get; set; }
        public int QuantidadeAnimalEmbarcado { get; set; }
        public DateTime DataEmbarque { get; set; }
        public List<SaidaAnimalLote> Lotes { get; set; }

        public SaidaAnimalCadastro()
        {
            Lotes = new List<SaidaAnimalLote>();
        }
    }

    public class SaidaAnimalLote
    {
        public PastoCurral Local { get; set; }
        public LoteEntrada Lote { get; set; }
        public int QuantidadeEmbarcado { get; set; }
        public decimal PesoMedio { get; set; }
    }
}
