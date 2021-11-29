using PlataformaWeb.Business.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlataformaWeb.WebApp.Models
{
    public class MovimentacaoEntreLoteDelecaoViewModel
    {
        [Key]
        public int Id { get; set; }
        public TipoPastoCurral Tipo { get; set; }

        [DisplayName("Data da Entrada")]
        public DateTime DataLote { get; set; }

        [DisplayName("Local de Origem")]
        public string LocalOrigem { get; set; }

        [DisplayName("Local de Destino")]
        public string LocalDestino { get; set; }
        public string Motivo { get; set; }

        [DisplayName("Data da Movimentação")]
        public DateTime DataMovimentacao { get; set; }

        [DisplayName("Total de Animais")]
        public int QuantidadeAnimais { get; set; }
    }

    public class MovimentacaoEntreLoteViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Lote de Entrada precisa ser definido")]
        [DisplayName("Lote de Entrada")]
        public LoteEntradaViewModel LoteEntrada { get; set; }

        [Required(ErrorMessage = "Local de Origem precisa ser definido")]
        [DisplayName("Local de Origem")]
        public PastoCurralConsultaViewModel LocalOrigem { get; set; }

        [Required(ErrorMessage = "Local de Destino precisa ser definido")]
        [DisplayName("Lote de Destino")]
        public PastoCurralConsultaViewModel LocalDestino { get; set; }

        [Required(ErrorMessage = "Motivo precisa ser definido")]
        public MotivoMovimentacaoViewModel Motivo { get; set; }

        [Required(ErrorMessage = "Data da Movimentação precisa ser informada")]
        [DisplayName("Data da Movimentação")]
        public DateTime? DataMovimentacao { get; set; }

        [DisplayName("Quantidade de Animais")]
        public int QuantidadeAnimais { get; set; }
    }

}
