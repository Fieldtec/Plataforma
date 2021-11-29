using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class LoteAnimalDTO
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string Planejamento { get; set; }
        public DateTime DataEntrada { get; set; }
        public int QuantidadeAnimais { get; set; }
        public string Raca { get; set; }
        public string Categoria { get; set; }
        public TipoEntradaLote TipoEntrada { get; set; }
        public string Tecnico { get; set; }
        public string NomePropriedade { get; set; }
        public string Proprietario { get; set; }
    }

    public class LoteEntradaPlanejamentoDTO
    {
        public int Id { get; set; }
        public int IdLocal { get; set; }
        public int QuantidadeAnimais { get; set; }
        public decimal PesoMedioProjetado { get; set; }
    }

}
