using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class CategoriaViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Nome precisa ter no máximo 100 caracteres")]
        public string Nome { get; set; }

        [DisplayName("Idade Mínima")]
        public int? IdadeMinima { get; set; }

        [DisplayName("Idade Máxima")]
        public int? IdadeMaxima { get; set; }

        [DisplayName("Sexo")]
        public string Sexo { get; set; }
    }
}
