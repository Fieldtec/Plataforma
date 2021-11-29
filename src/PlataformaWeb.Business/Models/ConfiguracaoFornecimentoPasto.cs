using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models
{
    public class ConfiguracaoFornecimentoPasto : Entity
    {
        public Status Segunda { get; set; }
        public Status Terca { get; set; }
        public Status Quarta { get; set; }
        public Status Quinta { get; set; }
        public Status Sexta { get; set; }
        public Status Sabado { get; set; }
        public Status Domingo { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; }
    }

}
