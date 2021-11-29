using PlataformaWeb.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Enums
{
    public enum TipoRacao
    {
        [Descricao("ADAPTAÇÃO")]
        Adaptacao = 1,

        [Descricao("CRESCIMENTO")]
        Crescimento = 2,

        [Descricao("TERMINAÇÃO")]
        Terminacao = 3,

        [Descricao("TRANSIÇÃO")]
        Transicao = 4
    }

    public static class TipoRacaoExtension
    {
        public static TipoRacao ObterTipoRacaoPorDescricao(this string descricao)
        {
            TipoRacao tipoRacao = TipoRacao.Adaptacao;
            foreach (TipoRacao item in Enum.GetValues(typeof(TipoRacao)))
            {
                if (item.ObterDescricao() == descricao)
                {
                    tipoRacao = item;
                    break;
                }
            }

            return tipoRacao;
        }
    }
}
