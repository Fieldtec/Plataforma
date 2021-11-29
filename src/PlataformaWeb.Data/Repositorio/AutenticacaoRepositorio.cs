using Microsoft.EntityFrameworkCore;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Data.Repositorio
{
    public class AutenticacaoRepositorio : IAutenticacaoRepositorio
    {
        private readonly PlataformaFieldContext _context;

        public AutenticacaoRepositorio(PlataformaFieldContext context)
        {
            _context = context;
        }

        public async Task<UsuarioResponseDTO> Login(UsuarioLoginDTO usuario)
        {
            UsuarioResponseDTO usuarioResponse = null;

            var pessoa = await _context.Pessoas
                .FirstOrDefaultAsync(x => (x.Usuario.ToUpper() == usuario.Usuario.ToUpper() || x.Email.ToUpper() == usuario.Usuario.ToUpper())
                            && x.Senha == usuario.Senha && x.Status == Business.Enums.Status.Ativado);

            if (pessoa != null)
            {
                usuarioResponse = new UsuarioResponseDTO(pessoa);
            }

            return usuarioResponse;
        }
    }
}
