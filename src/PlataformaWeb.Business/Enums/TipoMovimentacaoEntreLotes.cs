using PlataformaWeb.Business.Extensions;

namespace PlataformaWeb.Business.Enums
{
    public enum TipoMovimentacaoEntreLotes
    {
        [Descricao("LOTE ENTRE CURRAIS")]
        EntreCurrais = 1,

        [Descricao("LOTE ENTRE PASTOS")]
        EntrePastos = 2
    }
}
