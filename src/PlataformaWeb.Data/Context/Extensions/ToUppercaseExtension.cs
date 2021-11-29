using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlataformaWeb.Data.Context.Extensions
{
    public static class ToUppercaseExtension
    {
        public static void ApplyUpperCase(this ChangeTracker changeTracker)
        {
            foreach (var entry in changeTracker.Entries())
            {
                foreach (var prop in entry.Entity.GetType()
                    .GetProperties().Where(x => x.PropertyType == typeof(string) && x.GetValue(entry.Entity, null) != null))
                {
                    var value = prop.GetValue(entry.Entity, null).ToString();
                    if (!String.IsNullOrEmpty(value) && prop.Name != "Senha")
                    {
                        prop.SetValue(entry.Entity, value.ToUpper());
                    }
                }
            }
        }
    }
}
