using PlataformaWeb.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Enums
{
    public enum OrigemSuplemento
    {
        [Descricao("ESTOQUE PRINCIPAL")]
        EstoquePrincipal = 1,

        [Descricao("ESTOQUE PASTO")]
        EstoquePasto = 2
    }

    public enum DestinoSuplemento
    {
        [Descricao("FORNECIDO NO COCHO")]
        FornecidoNoCocho = 1,

        [Descricao("ESTOCADO")]
        Estocado = 2
    }
}
