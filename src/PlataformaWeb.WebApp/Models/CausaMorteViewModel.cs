using System.ComponentModel.DataAnnotations;

namespace PlataformaWeb.WebApp.Models
{
    public class CausaMorteViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Nome precisa ter no máximo 100 caracteres")]
        public string Nome { get; set; }
    }
}
