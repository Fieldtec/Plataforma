using System;

namespace PlataformaWeb.Business.Models
{
    public class FornecimentoConfinamento : Entity
    {
        public DateTime DataFornecimento { get; set; }
        public int IdRacao { get; set; }
        public Racao Racao { get; set; }
        public decimal MateriaSecaRacao { get; set; }
        public int IdCurral { get; set; }
        public PastoCurral Curral { get; set; }
        public int QuantidadeAnimais { get; set; }
        public int IdLote { get; set; }
        public LoteEntrada Lote { get; set; }
        public decimal KgPrevisto { get; set; }
        public decimal? KgRealizado { get; set; }
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; }
        public decimal Ajuste { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
    }
}
