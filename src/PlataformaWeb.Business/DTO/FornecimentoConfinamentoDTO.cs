using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class FornecimentoConfinamentoDTO
    {
        public int Id { get; set; }
        public DateTime DataFornecimento { get; set; }
        public int QuantidadeAnimais { get; set; }
        public string Curral { get; set; }
        public string NomeRacao { get; set; }
        public decimal MateriaSecaRacao { get; set; }
        public decimal KgPrevisto { get; set; }
        public decimal? KgRealizado { get; set; }
        public decimal KgDiferenca => (KgRealizado.HasValue ? KgRealizado.Value : 0) - KgPrevisto;
        public decimal? Ajuste { get; set; }
        public bool EhPrimeiroDia { get; set; }
    }

    public class FiltroFornecimentoConfinamentoDTO
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
        public int? IdCurral { get; set; }
        public int? IdRacao { get; set; }
    }

}
