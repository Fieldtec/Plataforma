using PlataformaWeb.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Enums
{
    public enum TipoEntradaLote
    {
        [Descricao("COMPRA")]
        Compra = 1,
        
        [Descricao("NASCIMENTO")]
        Nascimento = 2
    }
}
