using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class SuplementoMineralViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Fornecedor de Insumo é obrigatório")]
        [DisplayName("Fornecedor de Insumo")]
        public int? IdFornecedor { get; set; }
        
        [Required(ErrorMessage = "Ganho Esperado é obrigatório")]
        [DisplayName("Ganho Esperado")]
        public decimal? GanhoEsperado { get; set; }

        [Required(ErrorMessage = "Consumo Esperado é obrigatório")]
        [DisplayName("Consumo % Peso Vivo")]
        public decimal? ConsumoEsperado { get; set; }

        [Required(ErrorMessage = "Estoque Mínimo Kg é obrigatório")]
        [DisplayName("Estoque Mínimo Kg")]
        public decimal? EstoqueMinimoKg { get; set; }

        [Required(ErrorMessage = "Estoque Mínimo Dias é obrigatório")]
        [DisplayName("Estoque Mínimo Dias")]
        public decimal? EstoqueMinimoDias { get; set; }

        [Required(ErrorMessage = "Peso da Embalagem é obrigatório")]
        [DisplayName("Peso da Embalagem")]
        public decimal? PesoEmbalagem { get; set; }

        [DisplayName("Cm. Cocho Indicado")]
        public decimal? CmCochoIndicado { get; set; }

        [DisplayName("Valor do KG")]
        public decimal? ValorKg { get; set; }

        [Required(ErrorMessage = "Nome do Suplemento é obrigatório")]
        public string Nome { get; set; }
        
        public FornecedorInsumoViewModel FornecedorInsumo { get; set; }
    }

    public class SuplementoMineralConsultaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string NomeFornecedorInsumo { get; set; }
    }
}
