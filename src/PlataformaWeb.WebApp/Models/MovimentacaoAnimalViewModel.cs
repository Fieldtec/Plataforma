using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlataformaWeb.WebApp.Models
{
    public class MovimentacaoAnimalViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Lote de Entrada precisa ser definido")]
        [DisplayName("Lote de Origem")]
        public LoteEntradaViewModel LoteOrigem { get; set; }

        [Required(ErrorMessage = "Lote de Destino precisa ser definido")]
        [DisplayName("Lote de Destino")]
        public LoteEntradaViewModel LoteDestino { get; set; }

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

        [Required(ErrorMessage = "Quantidade Animais precisa ser informada")]
        [DisplayName("Quantidade de Animais")]
        public int? QuantidadeAnimais { get; set; }
    }

}
