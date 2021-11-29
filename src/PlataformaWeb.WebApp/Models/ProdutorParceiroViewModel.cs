using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class ProdutorParceiroViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome do Produtor é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Nome do Produtor precisa ter no máximo 100 caracteres")]
        [DisplayName("Nome")]
        public string Nome { get; set; }
       
        [DisplayName("CPF/CNPJ")]
        public string CpfCnpj { get; set; }
                
        [DisplayName("Propriedade Parceira")]
        public int IdPropriedadeParceira { get; set; }

        public PropriedadeParceiraViewModel PropriedadeParceira { get; set; }
    }
}
