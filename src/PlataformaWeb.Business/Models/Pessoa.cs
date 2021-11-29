using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models
{    
    public abstract class PessoaBase : Entity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public TipoPessoa Tipo { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public DateTime? DataRegistro { get; set; }
    }

    public class Pessoa : PessoaBase
    {
        
    }
}
