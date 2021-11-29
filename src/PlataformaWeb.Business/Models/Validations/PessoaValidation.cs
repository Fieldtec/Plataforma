using FluentValidation;
using PlataformaWeb.Business.Models.Validations.DomainValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public abstract class PessoaValidation<T> : AbstractValidator<T> where T : PessoaBase
    {
        protected void ValidarId()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("O Id é inválido");
        }

        protected void ValidarNome()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("O campo Nome é obrigatório")
                .MaximumLength(100)
                .WithMessage("O campo Nome precisa ter no máximo 100 caracteres");
        }

        protected void ValidarEmail()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O campo Email é obrigatório")
                .Must(EmailValidation.Validar)
                .WithMessage("O campo Email é inválido")
                .MaximumLength(100)
                .WithMessage("O campo Email precisa ter no máximo 100 caracteres");
        }

        protected void ValidarUsuario()
        {
            RuleFor(x => x.Usuario)
                .NotEmpty()
                .WithMessage("O campo Usuario é obrigatório")
                .MaximumLength(100)
                .WithMessage("O campo Usuario precisa ter no máximo 100 caracteres");
        }

        protected void ValidarSenha()
        {
            RuleFor(x => x.Senha)
                .NotEmpty()
                .WithMessage("O campo Senha é obrigatório")
                .MaximumLength(100)
                .WithMessage("O campo Senha precisa ter no máximo 100 caracteres");
        }

        protected void ValidarTelefone()
        {
            When(x => !String.IsNullOrEmpty(x.Telefone), () =>
            {
                RuleFor(x => x.Telefone)
                    .MaximumLength(20)
                    .WithMessage("O campo Email precisa ter no máximo 20 caracteres");
            });
        }


    }
}
