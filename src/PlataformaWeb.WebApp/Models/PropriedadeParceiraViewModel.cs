using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class PropriedadeParceiraViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome da Propriedade é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Nome da Propriedade precisa ter no máximo 100 caracteres")]
        [DisplayName("Nome da Propriedade")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Cidade é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Cidade precisa ter no máximo 100 caracteres")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Estado é obrigatório")]
        [MaxLength(2, ErrorMessage = "O campo Estado precisa ter no máximo 2 caracteres")]
        [DisplayName("UF")]
        public string Uf { get; set; }

        [DisplayName("Inscrição Estadual")]
        public string InscricaoEstadual { get; set; }

    }
}
