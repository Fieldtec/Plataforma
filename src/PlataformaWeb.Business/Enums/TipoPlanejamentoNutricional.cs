using PlataformaWeb.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Enums
{
    public enum TipoPlanejamentoNutricional
    {
        [Descricao("CONFINAMENTO")]
        Confinamento = 1,

        [Descricao("PASTO")]
        Pasto = 2
    }
}
