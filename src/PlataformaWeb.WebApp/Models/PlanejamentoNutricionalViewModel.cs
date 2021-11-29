using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{
    public class PlanejamentoNutricionalConsultaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public TipoPlanejamentoNutricional Tipo { get; set; }
    }

    public class PlanejamentoNutricionalViewModel
    {
        public PlanejamentoNutricionalViewModel()
        {
            PlanejamentoValoresConfinamento = new List<PlanejamentoValoresConfinamentoViewModel>();
            PlanejamentoValoresPasto = new List<PlanejamentoValoresPastoViewModel>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome do Planejamento é obrigatório")]
        [MaxLength(100, ErrorMessage = "NNome do Planejamento precisa ter no máximo 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Tipo do Planejamento é obrigatório")]
        public TipoPlanejamentoNutricional Tipo { get; set; }

        public List<PlanejamentoValoresConfinamentoViewModel> PlanejamentoValoresConfinamento { get; set; }

        public List<PlanejamentoValoresPastoViewModel> PlanejamentoValoresPasto { get; set; }
    }

    public class PlanejamentoValoresConfinamentoViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Dia Início é obrigatório")]
        public int? DiaInicio { get; set; }

        [Required(ErrorMessage = "Ingestão de Matéria Seca/Peso Vivo Esperado é obrigatório")]
        public decimal? ImspvEsperado { get; set; }

        [Required(ErrorMessage = "Ganho Peso Esperado é obrigatório")]
        public decimal? GmdEsperado { get; set; }

        [Required(ErrorMessage = "Dia Fim é obrigatório")]
        public int? DiaFim { get; set; }

        [Required(ErrorMessage = "Ração é obrigatório")]
        public RacaoConsultaViewModel Racao { get; set; }
    }
    
    public class PlanejamentoValoresPastoViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingestão de Matéria Seca/Peso Vivo Esperado é obrigatório")]
        public decimal? ImspvEsperado { get; set; }

        [Required(ErrorMessage = "Ganho Peso Esperado é obrigatório")]
        public decimal? GmdEsperado { get; set; }

        [Required(ErrorMessage = "Categoria é obrigatória")]
        public CategoriaViewModel Categoria { get; set; }

        [Required(ErrorMessage = "Fase do Ano é obrigatória")]
        public FaseDoAnoViewModel FaseDoAno { get; set; }

        [Required(ErrorMessage = "Suplemento é obrigatório")]
        public SuplementoMineralConsultaViewModel SuplementoMineral { get; set; }       

    }





}
