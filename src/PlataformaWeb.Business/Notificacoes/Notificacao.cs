using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Notificacoes
{
    public class Notificacao
    {
        public string Mensagem { get; set; }
        public Notificacao(string message)
        {
            Mensagem = message;
        }
    }
}
