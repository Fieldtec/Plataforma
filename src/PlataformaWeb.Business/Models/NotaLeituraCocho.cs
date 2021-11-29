using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models
{
    public class NotaLeituraCocho : Entity
    {
        public string Nome { get; set; }
        public decimal? AjustePorcentagem { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }
    }

    public class LogNotaLeituraCocho
    {
        public int Id { get; set; }
        public int IdNota { get; set; }
        public string Nome { get; set; }
        public decimal? AjustePorcentagem { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int IdCliente { get; set; }
    }
}
