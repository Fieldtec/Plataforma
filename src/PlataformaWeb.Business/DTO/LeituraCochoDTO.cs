using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class LeituraCochoDTO
    {
        public int Id { get; set; }
        public DateTime DataLeitura { get; set; }
        public string Curral { get; set; }
        public string Nota { get; set; }
        public decimal Ajuste { get; set; }
    }

    public class LeituraCochoInsercaoDTO
    {
        public int? Id { get; set; }
        public DateTime? DataLeitura { get; set; }
        public string Curral { get; set; }
        public int IdLote { get; set; }
        public int IdCurral { get; set; }
        public int QuantidadeAnimais { get; set; }
        public string Nota { get; set; }
        public decimal? Ajuste { get; set; }
        public bool TemLancamentoInsumo { get; set; }
        public decimal? RealizadoMateriaNatural { get; set; }
        public decimal? RealizadoMateriaSeca { get; set; }
    }

    public class FiltroLeituraCochoDTO
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
        public int? IdCurral { get; set; }
    }
}
