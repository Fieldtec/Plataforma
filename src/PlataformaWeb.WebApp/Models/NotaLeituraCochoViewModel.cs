using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class NotaLeituraCochoViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome da Nota é obrigatória")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Valor de AJuste da Nota é obrigatória")]
        [DisplayName("Ajuste")]
        public decimal? AjustePorcentagem { get; set; }
    }
}
