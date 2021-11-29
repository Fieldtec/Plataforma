using FluentValidation;
using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class TecnicoValidation : PessoaValidation<Tecnico>
    {
        public TecnicoValidation(TipoOperacao operacao)
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

            RuleFor(x => x.QtdeLicenca)
                .GreaterThan(0)
                .WithMessage("Quantidade de Licenças precisa ser maior do que 0");

            RuleFor(x => x.DataAquisicao == DateTime.MinValue)
                .Equal(false)
                .WithMessage("Data da Aquisição inválida");

        }
    }
}
