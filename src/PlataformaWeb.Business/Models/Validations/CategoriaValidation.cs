using FluentValidation;
using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class CategoriaValidation : AbstractValidator<Categoria>
    {
        public CategoriaValidation(TipoOperacao operacao)
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

            When(x => x.IdadeMaxima.HasValue && x.IdadeMinima.HasValue, () =>
            {
                RuleFor(x => x.IdadeMaxima)
                    .GreaterThanOrEqualTo(x => x.IdadeMinima)
                    .WithMessage("Idade Máxima não pode ser menor do que a Idade Mínima");
            });

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("O campo Id do Cliente precisa ser definido");
        }
    }
}
