using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class RacaViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Nome precisa ter no máximo 100 caracteres")]
        public string Nome { get; set; }

        [DisplayName("Código Bnd")]
        public string CodigoBnd { get; set; }
    }
}
