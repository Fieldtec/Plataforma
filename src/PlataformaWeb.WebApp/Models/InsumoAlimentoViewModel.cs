using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class InsumoAlimentoViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Fornecedor de Insumos é obrigatório")]
        [DisplayName("Fornecedor de Insumo")]
        public int? IdFornecedor { get; set; }
        public string Nome { get; set; }

        [Required(ErrorMessage = "Valor do Kg é obrigatório")]
        [DisplayName("Valor do Kg")]
        public decimal? ValorKg { get; set; }

        [Required(ErrorMessage = "Matéria Seca é obrigatório")]
        [DisplayName("Materia Seca")]
        public decimal? MateriaSeca { get; set; }

        [Required(ErrorMessage = "Estoque Mínimo KG é obrigatório")]
        [DisplayName("Estoque Mínimo KG")]
        public decimal? EstoqueMinimoKg { get; set; }

        [Required(ErrorMessage = "Estoque Mínimo Dias é obrigatório")]
        [DisplayName("Estoque Mínimo Dias")]
        public decimal? EstoqueMinimoDias { get; set; }        

        public FornecedorInsumoViewModel FornecedorInsumo { get; set; }
    }

    public class InsumoAlimentoConsultaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal? ValorKg { get; set; }
        public decimal? MateriaSeca { get; set; }
        public string NomeFornecedorInsumo { get; set; }
    }
}
