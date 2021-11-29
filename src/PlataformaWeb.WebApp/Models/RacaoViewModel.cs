using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class RacaoViewModel
    {
        public RacaoViewModel()
        {
            InsumosRacao = new List<RacaoInsumosViewModel>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tipo da Ração é obrigatório")]
        public TipoRacao Tipo { get; set; }

        [Required(ErrorMessage = "Nome da Ração é obrigatório")]
        [MaxLength(100, ErrorMessage = "Nome da Ração precisa ter no máximo 100 caracteres")]
        public string Nome { get; set; }

        public decimal? Gmd { get; set; }

        [DisplayName("Matéria Seca")]
        public decimal? MateriaSeca { get; set; }           
        
        [DisplayName("Valor KG")]
        public decimal? ValorKg { get; set; }

        [Required(ErrorMessage = "Data da Formulação é obrigatório")]
        [DisplayName("Data Formulação")]
        public DateTime? DataFormulacao { get; set; }

        public List<RacaoInsumosViewModel> InsumosRacao { get; set; }
    }

    public class RacaoInsumosViewModel
    {
        [Key]
        public int Id { get; set; }        

        [Required(ErrorMessage = "Percentual Matéria Seca precisa ser informado")]
        public decimal? PercentualMateriaSeca { get; set; }

        [Required(ErrorMessage = "KG da Matéria Seca precisa ser informado")]
        public decimal? KgMateriaSeca { get; set; }

        public decimal KgMateriaNatural { get; set; }

        public decimal InclusaoMateriaSeca { get; set; }

        public decimal InclusaoMateriaNatural { get; set; }

        [Required(ErrorMessage = "Valor do KG do Insumo precisa ser informado")]
        public decimal? ValorKg { get; set; }

        public decimal ValorInclusao { get; set; }

        [Required(ErrorMessage = "Insumo precisa ser informado")]
        public InsumoAlimentoConsultaViewModel InsumoAlimento { get; set; }        
    }


    public class RacaoConsultaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Gmd { get; set; }
        public decimal MateriaSeca { get; set; }
    }

}
