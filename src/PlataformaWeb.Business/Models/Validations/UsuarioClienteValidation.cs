using FluentValidation;
using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class UsuarioClienteValidation : PessoaValidation<UsuarioCliente>
    {
        public UsuarioClienteValidation(TipoOperacao operacao)
        {
            if (operacao != TipoOperacao.Inclusao)
            {
                ValidarId();
            }

            ValidarNome();
            ValidarEmail();
            ValidarUsuario();

            if (operacao == TipoOperacao.Inclusao)
                ValidarSenha();

            ValidarTelefone();

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("Cliente precisa ser definido");

        }
    }
}
