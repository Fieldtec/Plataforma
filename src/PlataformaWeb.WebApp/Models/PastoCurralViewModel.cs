using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class PastoCurralViewModel
    {
        [Key]
        public int Id { get; set; }
        public TipoPastoCurral Tipo { get; set; }
        public string Linha { get; set; }

        [DisplayName("Número")]
        public int? Numero { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Nome precisa ter no máximo 100 Caracteres")]
        public string Nome { get; set; }

        public int Capacidade { get; set; }

        [DisplayName("Lotação")]
        public int? Lotacao { get; set; }

        [DisplayName("Metragem Cocho")]
        public decimal? Metragemcocho { get; set; }        

        public int? OrdemFornecimento { get; set; }
    }

    public class PastoCurralConsultaViewModel
    {
        [Key]
        public int Id { get; set; }
        public TipoPastoCurral Tipo { get; set; }
        public string Nome { get; set; }
        public int Capacidade { get; set; }
        public int Lotacao { get; set; }
    }
}
