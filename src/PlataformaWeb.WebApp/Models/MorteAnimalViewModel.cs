using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlataformaWeb.WebApp.Models
{
    public class MorteAnimalViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Local de Origem precisa ser definido")]
        [DisplayName("Local de Origem")]
        public PastoCurralConsultaViewModel Local { get; set; }

        [Required(ErrorMessage = "Lote de Entrada precisa ser definido")]
        [DisplayName("Lote de Entrada")]
        public LoteEntradaViewModel LoteEntrada { get; set; }

        [Required(ErrorMessage = "Motivo precisa ser definido")]
        public CausaMorteViewModel CausaMorte { get; set; }

        [Required(ErrorMessage = "Data da Morte precisa ser informada")]
        [DisplayName("Data da Morte")]
        public DateTime? DataMorte { get; set; }

        [Required(ErrorMessage = "Quantidade Animais precisa ser informada")]
        [DisplayName("Quantidade de Animais")]
        public int? QuantidadeAnimais { get; set; }
    }

}
