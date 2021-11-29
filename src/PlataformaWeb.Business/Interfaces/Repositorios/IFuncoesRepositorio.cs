using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IFuncoesRepositorio
    {
        Task AtualizaGmd(int idCliente);
        Task AtualizaLote(int idCliente, DateTime dataBase);
        Task AtualizaLote(int idCliente, int idLote, DateTime dataBase);
        //Task<decimal> RetornaGmd(int idPlanejamento)

    }
}
