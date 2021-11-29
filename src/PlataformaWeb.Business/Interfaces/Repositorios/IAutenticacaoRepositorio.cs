using PlataformaWeb.Business.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IAutenticacaoRepositorio
    {
        Task<UsuarioResponseDTO> Login(UsuarioLoginDTO usuario);
    }
}
