using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class FiltroRelatorioViewModel
    {
        [Required(ErrorMessage = "Nome do Relatório é Obrigatório")]
        public string NomeRelatorio { get; set; }

        public string Filtro { get; set; }
    }
}
