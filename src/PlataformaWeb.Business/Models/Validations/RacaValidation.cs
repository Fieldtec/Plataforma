using FluentValidation;
using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class RacaValidation : AbstractValidator<Raca>
    {
        public RacaValidation(TipoOperacao operacao)
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

            When(x => !String.IsNullOrEmpty(x.CodigoBnd), () =>
            {
                RuleFor(x => x.CodigoBnd)
                    .MaximumLength(2)
                    .WithMessage("Código Bnb precisa ter no máximo 2 caracteres");
            });

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("O campo Id do Cliente precisa ser definido");

        }
    }
}
