using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlataformaWeb.Business.Notificacoes
{   
    public class Notificador : INotificador
    {
        private List<Notificacao> _notificacoes;
        public Notificador()
        {
            _notificacoes = new List<Notificacao>();
        }

        public IReadOnlyCollection<Notificacao> ObterNotificacoes()
        {
            return _notificacoes;
        }

        public void Handle(Notificacao notificacao)
        {
            _notificacoes.Add(notificacao);
        }

        public bool TemNotificacao()
        {
            return _notificacoes.Any();
        }
    }
}
