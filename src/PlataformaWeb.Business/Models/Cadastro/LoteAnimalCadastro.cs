using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Cadastro
{
    public class LoteAnimalCadastro : Entity
    {
        public LoteAnimalCadastro()
        {
            MovimentacoesNoLote = new List<MovimentacaoLoteEntrada>();
        }

        public PastoCurral Local { get; set; }
        public PlanejamentoNutricional Planejamento { get; set; }
        public Raca Raca { get; set; }
        public Categoria Categoria { get; set; }
        public int QuantidadeAnimais { get; set; }
        public TipoEntradaLote TipoEntrada { get; set; }
        public DateTime DataEntrada { get; set; }
        public int IdadeEntrada { get; set; }
        public decimal ValorCompra { get; set; }
        public decimal PesoEntrada { get; set; }
        public ProdutorParceiro ProdutorParceiro { get; set; }
        public List<MovimentacaoLoteEntrada> MovimentacoesNoLote { get; set; }
    }

    public class MovimentacaoLoteEntrada
    {
        public int IdLote { get; set; }
        public DateTime DataEntrada { get; set; }
        public decimal PesoMedio { get; set; }
        public int QuantidadeAnimais { get; set; }
        public string ProdutorOrigem { get; set; }
    }

}
