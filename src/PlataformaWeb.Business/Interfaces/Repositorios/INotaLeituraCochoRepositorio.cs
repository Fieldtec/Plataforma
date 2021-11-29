using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface INotaLeituraCochoRepositorio : IRepositorio<NotaLeituraCocho>
    {
        Task AdicionarLog(List<NotaLeituraCocho> notas);
    }
}
