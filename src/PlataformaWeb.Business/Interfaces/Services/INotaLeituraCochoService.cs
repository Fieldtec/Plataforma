using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Services
{
    public interface INotaLeituraCochoService
    {
        Task Adicionar(NotaLeituraCocho nota);
        Task Remover(int id);
        Task Atualizar(List<NotaLeituraCocho> notas);
        Task<List<NotaLeituraCocho>> ObterTodos();
    }
}
