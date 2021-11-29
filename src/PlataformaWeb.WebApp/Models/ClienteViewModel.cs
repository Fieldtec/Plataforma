using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class ClienteViewModel : PessoaViewModel
    {
        [Required(ErrorMessage = "Nome da Propriedade é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Nome da Propriedade precisa ter no máximo 100 caracteres")]
        [DisplayName("Nome da Propriedade")]
        public string NomePropriedade { get; set; }

        [Required(ErrorMessage = "Cidade é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Cidade precisa ter no máximo 100 caracteres")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Estado é obrigatório")]
        [MaxLength(2, ErrorMessage = "O campo Estado precisa ter no máximo 2 caracteres")]
        [DisplayName("UF")]
        public string Uf { get; set; }

        [DisplayName("Qtd. Animais")]
        public int QtdeAnimais { get; set; }

        [DisplayName("Validade Licença")]
        public DateTime? DataValidadeLicenca { get; set; }

        [DisplayName("CPF/CNPJ")]
        public string CpfCnpj { get; set; }

        [DisplayName("Area/Hectare")]
        public decimal? AreaHectare { get; set; }
    }

    public class UsuarioClienteViewModel : PessoaViewModel
    {
        public int IdCliente { get; set; }
    }
}
