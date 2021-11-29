using PlataformaWeb.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Enums
{
    public struct Role
    {
        public const string Adm = "Administrador";
        public const string Tecnico = "Tecnico";
        public const string Cliente = "Cliente";
        public const string UsuarioCliente = "UsuarioCliente";
    }

    public enum TipoPessoa
    {
        [Descricao("Administrador")]
        Adm = 1,

        [Descricao("Tecnico")]
        Tecnico = 2,

        [Descricao("Cliente")]
        Cliente = 3,

        [Descricao("UsuarioCliente")]
        UsuarioCliente = 4
    }
}
