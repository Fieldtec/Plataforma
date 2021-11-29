using System;
using System.Collections.Generic;

namespace PlataformaWeb.Business.Models
{
    public class MotivoMovimentacao : Entity
    {
        public string Nome { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual List<MovimentacaoEntreLote> MovimentacoesEntreLote { get; set; }
        public virtual List<MovimentacaoAnimal> MovimentacoesAnimal { get; set; }

        public MotivoMovimentacao()
        {
            MovimentacoesEntreLote = new List<MovimentacaoEntreLote>();
            MovimentacoesAnimal = new List<MovimentacaoAnimal>();
        }

    }


}
