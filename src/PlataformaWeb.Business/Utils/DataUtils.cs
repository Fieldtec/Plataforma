using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Utils
{
    public static class DataUtils
    {
        public static bool EstaEntre(this DateTime dataComparacao, DateTime dataInicial, DateTime dataFinal)
        {
            //if (dataComparacao.HasValue && dataInicial.HasValue && dataFinal.HasValue)
            //{
                if (DateTime.Compare(dataComparacao, dataInicial) >= 0 && DateTime.Compare(dataFinal, dataComparacao) >= 0)
                    return true;
            //}

            return false;
        }

    }
}
