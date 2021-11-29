using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.DTO
{
    public class UsuarioLoginDTO
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
    }

    public class UsuarioResponseDTO
    {
        public int Id { get; set; }
        public String Email { get; set; }
        public String Usuario { get; set; }
        public String Nome { get; set; }
        public int IdCliente { get; set; } //Preenchido quando o tipo pessoa for cliente porém é o usuário do cliente.
        public TipoPessoa Tipo { get; set; }
        public Status Status { get; set; }
        public UsuarioResponseDTO()
        { }

        public UsuarioResponseDTO(Pessoa pessoa)
        {
            this.Id = pessoa.Id;
            this.Email = pessoa.Email;
            this.Usuario = pessoa.Usuario;
            this.Nome = pessoa.Nome;
            this.Tipo = pessoa.Tipo;
            this.Status = pessoa.Status;
        }
    }

}
