using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Utils
{
    public static class StringUtils
    {
        public static string ApenasNumeros(this string valor)
        {
            var onlyNumber = "";
            foreach (var s in valor)
            {
                if (char.IsDigit(s))
                {
                    onlyNumber += s;
                }
            }
            return onlyNumber.Trim();
        }

    }
}
