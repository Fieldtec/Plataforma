using PlataformaWeb.Business.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations.DomainValidation
{
    public class TelefoneValidation
    {
        public static bool Validar(string telefone)
        {
            if (telefone == null) return false;

            var telefoneNumeros = telefone.ApenasNumeros();

            return TamanhoValido(telefoneNumeros);
        }

        private static bool TamanhoValido(string valor)
        {
            if (valor.Length == 10 || valor.Length == 11)
                return true;


            return false;
        }
    }
}
