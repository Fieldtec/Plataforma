using FluentValidation;
using PlataformaWeb.Business.Enums;
using System;

namespace PlataformaWeb.Business.Models.Validations
{
    public class FrigorificoValidation : AbstractValidator<Frigorifico>
    {
        public FrigorificoValidation(TipoOperacao operacao)
        {
            if (operacao == TipoOperacao.Atualizacao)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage("Id precisa ser definido");
            }

            RuleFor(x => x.Nome)
               .NotNull()
               .WithMessage("O campo Nome é obrigatório")
               .MaximumLength(100)
               .WithMessage("O campo Nome precisa ter no máximo 100 caracteres");

            When(x => !String.IsNullOrEmpty(x.Cidade), () =>
            {
                RuleFor(x => x.Cidade)
                   .MaximumLength(100)
                   .WithMessage("O campo Cidade precisa ter no máximo 100 caracteres");
            });

            When(x => !String.IsNullOrEmpty(x.Uf), () =>
            {
                RuleFor(x => x.Uf)
                   .MaximumLength(100)
                   .WithMessage("O campo UF precisa ter no máximo 2 caracteres");
            });

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("O campo Id do Cliente precisa ser definido");

        }
    }
}
