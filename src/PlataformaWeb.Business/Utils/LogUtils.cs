using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Utils
{
    public static class LogUtils
    {
        public async static Task StoreAsync(Exception e, object model = null)
        {
            StringBuilder erro = new StringBuilder();
            erro.AppendLine();
            erro.AppendLine(DateTime.Now.ToString());
            erro.AppendLine("".PadLeft(60, '-'));
            erro.AppendLine("Dados da exceção: ");

            while (e != null)
            {
                erro.AppendLine(e.Message);
                erro.AppendLine(e.StackTrace);
                e = e.InnerException;
            }

            if (model != null)
            {
                erro.AppendLine("".PadLeft(60, '-'));
                erro.AppendLine("Tipo do Objeto com erro: " + model.GetType().FullName);
                erro.AppendLine("Dados do Objeto: ");
                foreach (var p in model.GetType().GetProperties())
                {
                    erro.AppendLine(string.Format("{0}: {1}", p.Name, p.GetValue(model, null)));
                }
            }

            string logFolder = Path.Combine(Environment.CurrentDirectory, "Temp", "Logs");

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            await File.WriteAllTextAsync(logFolder + "/" + DateTime.Now.Ticks + ".txt", erro.ToString());

        }
    }
}
