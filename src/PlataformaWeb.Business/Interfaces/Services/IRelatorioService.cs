using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Services
{
    public interface IRelatorioService
    {
        Task<string> Imprimir(String nomeRelatorio, string filtros);
    }
}
