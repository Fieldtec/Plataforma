using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class FornecimentoPastoDTO
    {
        public int Id { get; set; }
        public DateTime DataRealizado { get; set; }
        public string Pasto { get; set; }
        public int QuantidadeAnimais { get; set; }
        public string Suplemento { get; set; }
        public decimal PrevisaoKg { get; set; }
        public decimal RealizadoKg { get; set; }
        public decimal PrevisaoSaco { get; set; }
        public decimal RealizadoSaco { get; set; }
        public OrigemSuplemento Origem { get; set; }
        public DestinoSuplemento Destino { get; set; }
    }

    public class PreparaDadosFornecimentoPastoDTO
    {
        public DateTime? UltimoFornecimento { get; set; }
        public int QuantidadeAnimais { get; set; }
        public string Suplemento { get; set; }
        public int IdSuplemento { get; set; }
        public int IdLote { get; set; }
        public string Fornecedor { get; set; }
        public decimal PesoEmbalagem { get; set; }
        public decimal PesoVivo { get; set; }
        public decimal PrevisaoSaco { get; set; }
        public decimal PrevisaoKg { get; set; }
        public decimal PesoProjetado { get; set; }
    }

    public class FiltroFornecimentoPastoDTO
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
        public int? IdPasto { get; set; }
    }
}
