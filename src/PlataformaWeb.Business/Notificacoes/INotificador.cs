using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Notificacoes
{
    public interface INotificador
    {
        bool TemNotificacao();
        IReadOnlyCollection<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}
