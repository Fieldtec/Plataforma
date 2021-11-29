using System;

namespace PlataformaWeb.Business.Models
{
    public class PrevisaoFornecimentoPasto : Entity
    {
        public DateTime DataPrevisao { get; set; }
        public int IdPasto { get; set; }
        public PastoCurral Pasto { get; set; }
        public int IdLote { get; set; }
        public LoteEntrada Lote { get; set; }
        public int QuantidadeAnimais { get; set; }
        public int IdSuplemento { get; set; }
        public SuplementoMineral Suplemento { get; set; }
        public decimal? PrevisaoKg { get; set; }
        public decimal? PrevisaoSaco { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; }
    }

}
