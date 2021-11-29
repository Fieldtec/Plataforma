using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PlataformaWeb.Business.Extensions
{
    public static class CustomAttributes
    {
        public static string ObterDescricao(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return null;
            var attribute = (DescricaoAttribute)fieldInfo.GetCustomAttribute(typeof(DescricaoAttribute));
            return attribute?.StringValue;
        }
    }

    public class DescricaoAttribute : Attribute
    {
        public string StringValue { get; set; }
        public DescricaoAttribute(String _stringValue)
        {
            StringValue = _stringValue;
        }
    }

}
