using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Models
{

    public class LoteAnimalViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Local é obrigatório")]
        public PastoCurralConsultaViewModel Local { get; set; }

        [Required(ErrorMessage = "O campo Planejamento é obrigatório")]
        public PlanejamentoNutricionalConsultaViewModel Planejamento { get; set; }

        [Required(ErrorMessage = "O campo Raça é obrigatório")]
        [DisplayName("Raça")]
        public RacaViewModel Raca { get; set; }

        [Required(ErrorMessage = "O campo Categoria é obrigatório")]
        public CategoriaViewModel Categoria { get; set; }

        [Required(ErrorMessage = "O campo Quantidade de Animais é obrigatório")]
        [DisplayName("Quantidade de Animais")]
        public int? QuantidadeAnimais { get; set; }

        [DisplayName("Tipo da Entrada")]
        public TipoEntradaLote TipoEntrada { get; set; }

        [Required(ErrorMessage = "O campo Data de Entrada é obrigatório")]
        [DisplayName("Data da Entrada")]
        public DateTime? DataEntrada { get; set; }

        [Required(ErrorMessage = "O campo Idade é obrigatório")]
        [DisplayName("Idade Média")]
        public int? IdadeEntrada { get; set; }

        [DisplayName("Valor da Compra")]
        public decimal? ValorCompra { get; set; }

        [DisplayName("Peso médio")]
        public decimal? PesoEntrada { get; set; }

        [DisplayName("Produtor Parceiro")]
        public ProdutorParceiroViewModel ProdutorParceiro { get; set; }

        public List<MovimentacaoLoteEntradaViewModel> MovimentacoesNoLote { get; set; }

        public LoteAnimalViewModel()
        {
            MovimentacoesNoLote = new List<MovimentacaoLoteEntradaViewModel>();
        }
    }

    public class LoteAnimalDeletarViewModel
    {
        [Key]
        public int Id { get; set; }
        public PastoCurralConsultaViewModel Local { get; set; }
        public PlanejamentoNutricionalConsultaViewModel Planejamento { get; set; }
        [DisplayName("Raça")]
        public RacaViewModel Raca { get; set; }
        public CategoriaViewModel Categoria { get; set; }
        [DisplayName("Quantidade de Animais")]
        public int? QuantidadeAnimais { get; set; }

        [DisplayName("Tipo da Entrada")]
        public TipoEntradaLote TipoEntrada { get; set; }

        [DisplayName("Data da Entrada")]
        public DateTime? DataEntrada { get; set; }

        [DisplayName("Idade Média")]
        public int? IdadeEntrada { get; set; }

        [DisplayName("Valor da Compra")]
        public decimal? ValorCompra { get; set; }

        [DisplayName("Peso médio")]
        public decimal? PesoEntrada { get; set; }

        [DisplayName("Produtor Parceiro")]
        public ProdutorParceiroViewModel ProdutorParceiro { get; set; }

        public List<MovimentacaoLoteEntradaViewModel> MovimentacoesNoLote { get; set; }

        public LoteAnimalDeletarViewModel()
        {
            MovimentacoesNoLote = new List<MovimentacaoLoteEntradaViewModel>();
        }
    }

    public class MovimentacaoLoteEntradaViewModel
    {
        public int IdLote { get; set; }
        public DateTime DataEntrada { get; set; }
        public decimal PesoMedio { get; set; }
        public int QuantidadeAnimais { get; set; }
        public string ProdutorOrigem { get; set; }
    }

    public class LoteEntradaViewModel
    {
        public int Id { get; set; }
        public DateTime DataEntrada { get; set; }
        public int QuantidadeAnimais { get; set; }
        public PlanejamentoNutricionalConsultaViewModel Planejamento { get; set; }
    }

}
