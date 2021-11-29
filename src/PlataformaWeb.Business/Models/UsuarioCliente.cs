using System;
using System.Collections.Generic;

namespace PlataformaWeb.Business.Models
{
    public class UsuarioCliente : PessoaBase
    {
        public int? IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }
    }
}
