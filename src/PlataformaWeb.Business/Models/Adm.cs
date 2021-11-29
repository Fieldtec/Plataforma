using System;
using System.Collections.Generic;

namespace PlataformaWeb.Business.Models
{
    public class Adm : PessoaBase
    {
        public Adm()
        {
            Tecnico = new List<Tecnico>();
        }

        public virtual List<Tecnico> Tecnico { get; set; }
    }
}
