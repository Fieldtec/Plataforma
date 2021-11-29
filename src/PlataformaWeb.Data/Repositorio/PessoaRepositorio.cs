using Microsoft.EntityFrameworkCore;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Data.Repositorio
{
    public class PessoaRepositorio : IPessoaRepositorio
    {
        private readonly PlataformaFieldContext _context;

        public PessoaRepositorio(PlataformaFieldContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Função que verifica se já existe o EMail cadastrado no banco de dados.
        /// Quando o Id da Pessoa for Diferente de 0, irá verificar se o email existe para uma pessoa que não seja ele mesmo
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="id">Id da Pessoa</param>
        /// <returns>Flag indicando se existe email cadastrado</returns>
        public async Task<bool> ExisteEmail(string email, int id)
        {
            var where = PredicateBuilder.True<Pessoa>().And(x => x.Email == email && x.Status == Business.Enums.Status.Ativado);
            if (id != 0)
                where = where.And(x => x.Id != id);

            return await _context.Pessoas.AnyAsync(where);
        }

        /// <summary>
        /// Função que verifica se já existe o Usuário cadastrado no banco de dados.
        /// Quando o Id da Pessoa for Diferente de 0, irá verificar se o usuário existe para uma pessoa que não seja ele mesmo
        /// </summary>
        /// <param name="usuario">Usuario</param>
        /// <param name="id">Id da Pessoa</param>
        /// <returns>Flag indicando se existe usuario cadastrado</returns>
        public async Task<bool> ExisteUsuario(string usuario, int id)
        {
            var where = PredicateBuilder.True<Pessoa>().And(x => x.Usuario == usuario && x.Status == Business.Enums.Status.Ativado);
            if (id != 0)
                where = where.And(x => x.Id != id);

            return await _context.Pessoas.AnyAsync(where);
        }
                
    }
}
