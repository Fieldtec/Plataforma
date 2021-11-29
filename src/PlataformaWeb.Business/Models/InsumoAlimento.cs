using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models
{
    public class InsumoAlimento : Entity
    {
        public InsumoAlimento()
        {
            InsumosRacoes = new List<RacaoInsumo>();
            AtualizacoesMateriaSeca = new List<AtualizacaoMateriaSeca>();
        }

        public int IdFornecedor { get; set; }
        public string Nome { get; set; }
        public decimal ValorKg { get; set; }
        public decimal MateriaSeca { get; set; }
        public decimal EstoqueMinimoKg { get; set; }
        public decimal EstoqueMinimoDias { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public DateTime? DataAtualizacaoMateriaSeca { get; set; }
        public virtual FornecedorInsumo FornecedorInsumo { get; set; }
        public virtual List<RacaoInsumo> InsumosRacoes { get; set; }
        public virtual List<AtualizacaoMateriaSeca> AtualizacoesMateriaSeca { get; set; }
    }
}
