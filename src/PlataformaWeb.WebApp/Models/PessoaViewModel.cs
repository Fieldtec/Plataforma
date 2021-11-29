using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PlataformaWeb.WebApp.Models
{
    public abstract class PessoaViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Nome precisa ter no máximo 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Email precisa ter no máximo 100 caracteres")]
        public string Email { get; set; }

        public string Telefone { get; set; }

        [Required(ErrorMessage = "O campo Usuário é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Usuário precisa ter no máximo 100 caracteres")]
        public string Usuario { get; set; }      
        
        public DateTime? DataRegistro { get; set; }

        public String Senha { get; set; }

        [Compare("Senha", ErrorMessage = "Campos Senha e Confirmação de Senha não são iguais")]
        [DisplayName("Confirmação de Senha")]
        public String ConfirmaSenha { get; set; }
    }
}
