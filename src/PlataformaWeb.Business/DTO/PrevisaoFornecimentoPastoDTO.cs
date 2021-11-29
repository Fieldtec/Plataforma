using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class PrevisaoFornecimentoPastoDTO
    {
        public int Id { get; set; }
        public DateTime DataPrevisao { get; set; }
        public string Pasto { get; set; }
        public int QuantidadeAnimais { get; set; }
        public string Suplemento { get; set; }
        public decimal PrevisaoKg { get; set; }
        public decimal PrevisaoSaco { get; set; }
    }

    public class GeracaoFornecimentoPastoDTO
    {
        public DateTime DataInicial { get; set; }
        public int QuantidadeSemanas { get; set; }
        public int IdSuplemento { get; set; }
        public bool Segunda { get; set; }
        public bool Terca { get; set; }
        public bool Quarta { get; set; }
        public bool Quinta { get; set; }
        public bool Sexta { get; set; }
        public bool Sabado { get; set; }
        public bool Domingo { get; set; }
    }

    public class FiltroPrevisaoFornecimentoPastoDTO
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
        public int? IdPasto { get; set; }
    }
}
