using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class LoteSaidaDTO
    {
        public int Id { get; set; }
        public int NumeroLote { get; set; }
        public DateTime DataEmbarque { get; set; }
        public TipoSaida TipoSaida { get; set; }
        public string ProdutorFrigorificoDestino { get; set; }
        public decimal QuantidadeAnimalPrevista { get; set; }
        public decimal QuantidadeAnimalEmbarcado { get; set; }
        public string NomePropriedade { get; set; }
        public string Proprietario { get; set; }
        public string Tecnico { get; set; }
    }
}
