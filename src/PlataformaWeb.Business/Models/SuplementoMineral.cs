using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models
{
    public class SuplementoMineral : Entity
    {
        public SuplementoMineral()
        {
            PlanejamentoValoresPasto = new List<PlanejamentoValoresPasto>();
            PrevisaoFornecimentosPasto = new List<PrevisaoFornecimentoPasto>();
            FornecimentosPasto = new List<FornecimentoPasto>();
        }

        public int IdFornecedor { get; set; }
        public decimal GanhoEsperado { get; set; }
        public decimal ConsumoEsperado { get; set; }
        public decimal EstoqueMinimoKg { get; set; }
        public decimal EstoqueMinimoDias { get; set; }
        public decimal PesoEmbalagem { get; set; }
        public decimal? CmCochoIndicado { get; set; }
        public decimal? ValorKg { get; set; }
        public string Nome { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public virtual FornecedorInsumo FornecedorInsumo { get; set; }
        public virtual List<PlanejamentoValoresPasto> PlanejamentoValoresPasto { get; set; }
        public virtual List<PrevisaoFornecimentoPasto> PrevisaoFornecimentosPasto { get; set; }
        public virtual List<FornecimentoPasto> FornecimentosPasto { get; set; }
    }
}
