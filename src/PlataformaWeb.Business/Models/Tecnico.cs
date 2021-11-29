using System;
using System.Collections.Generic;

namespace PlataformaWeb.Business.Models
{
    public class Tecnico : PessoaBase
    {
        public Tecnico()
        {
            Cliente = new List<Cliente>();
        }

        public int IdAdm { get; set; }
        public int QtdeLicenca { get; set; }
        public DateTime DataAquisicao { get; set; }
        public DateTime? DataUltimaAlteracao { get; set; }
        public int? IdAdmAlteracao { get; set; }
        public virtual Adm Adm { get; set; }
        public virtual List<Cliente> Cliente { get; set; }
    }
}
