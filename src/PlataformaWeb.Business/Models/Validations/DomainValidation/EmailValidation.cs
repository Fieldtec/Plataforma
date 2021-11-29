using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PlataformaWeb.Business.Models.Validations.DomainValidation
{
    public class EmailValidation
    {
        public static bool Validar(string email)
        {
            if (email == null) return false;

            var regexEmail = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            return regexEmail.IsMatch(email);
        }
    }
}
