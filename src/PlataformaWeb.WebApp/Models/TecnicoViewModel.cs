using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class TecnicoViewModel : PessoaViewModel
    {        
        [DisplayName("Qtd. Licença")]
        public int QtdeLicenca { get; set; }

        [Required(ErrorMessage = "O campo Data Aquisição é obrigatório")]
        [DisplayName("Data de Aquisição")]
        public DateTime? DataAquisicao { get; set; }        
    }
}
