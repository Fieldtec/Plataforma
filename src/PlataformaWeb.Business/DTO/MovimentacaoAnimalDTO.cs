using PlataformaWeb.Business.Enums;
using System;

namespace PlataformaWeb.Business.DTO
{
    public class MovimentacaoAnimalDTO
    {
        public int IdLoteOrigem { get; set; }
        public int IdLoteDestino { get; set; }
        public int IdLocalOrigem { get; set; }
        public int IdLocalDestino { get; set; }
        public string LocalOrigem { get; set; }
        public string LocalDestino { get; set; }
        public string Motivo { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public int QuantidadeAnimais { get; set; }
        public string NomePropriedade { get; set; }
        public string Proprietario { get; set; }
        public string Tecnico { get; set; }
        public TipoMovimentacaoEntreLotes Tipo { get; set; }
    }
}
