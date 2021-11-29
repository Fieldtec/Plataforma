using PlataformaWeb.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Enums
{
    public enum TipoSaida
    {
        [Descricao("ABATE")]
        Abate = 1,

        [Descricao("VENDA")]
        Venda = 2
    }
}
