using PlataformaWeb.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Enums
{
    public enum Sexo
    {
        [Descricao("MASCULINO")]
        M = 'M',

        [Descricao("FEMININO")]
        F = 'F'
    }
    public static class SexoExtension
    {
        public static Sexo ObterSexo(this string descricao)
        {
            Sexo sexo = Sexo.M;
            foreach (Sexo item in Enum.GetValues(typeof(Sexo)))
            {               
                if (item.ObterDescricao() == descricao)
                {
                    sexo = item ;
                    break;
                }
            }
            return sexo;
        }
    }
}
