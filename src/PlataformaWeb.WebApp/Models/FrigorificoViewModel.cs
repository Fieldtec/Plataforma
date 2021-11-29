using System.ComponentModel.DataAnnotations;

namespace PlataformaWeb.WebApp.Models
{
    public class FrigorificoViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Nome precisa ter no máximo 100 caracteres")]
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
    }
}
