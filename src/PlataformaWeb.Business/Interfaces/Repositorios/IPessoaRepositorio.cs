using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IPessoaRepositorio 
    {
        Task<bool> ExisteUsuario(string usuario, int id);        
        Task<bool> ExisteEmail(string email, int id);
    }
}
