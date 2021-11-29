using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class MovimentacaoEntreLoteDTO
    {
        public int Id { get; set; }
        public DateTime DataLote { get; set; }
        public string LocalOrigem { get; set; }
        public string LocalDestino { get; set; }
        public string Motivo { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public int QuantidadeAnimais { get; set; }
        public string NomePropriedade { get; set; }
        public string Proprietario { get; set; }
        public string Tecnico { get; set; }
    }
}
