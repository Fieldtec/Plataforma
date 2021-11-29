using PlataformaWeb.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Enums
{
    public enum TipoPastoCurral
    {
        [Descricao("CURRAL")]
        Curral = 1,

        [Descricao("PASTO")]
        Pasto = 2
    }

    public enum TipoLotacaoLocal
    {
        Todos = 1,
        ComLotacao = 2,
        SemLotacao = 3,
    }
}
