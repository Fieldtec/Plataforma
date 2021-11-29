using FluentValidation;
using PlataformaWeb.Business.Enums;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class FaseDoAnoValidation : AbstractValidator<FaseDoAno>
    {
        public FaseDoAnoValidation(TipoOperacao operacao)
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

            RuleFor(x => x.DataInicio)
                .NotNull()
                .WithMessage("Data Inícial não foi definida");

            RuleFor(x => x.DataFim)
                .NotNull()
                .WithMessage("Data Final não foi definida");

            When(x => x.DataInicio.HasValue && x.DataFim.HasValue, () =>
            {
                RuleFor(x => x.DataFim)
                    .GreaterThanOrEqualTo(x => x.DataInicio)
                    .WithMessage("Idade Fim não pode ser menor do que a Idade Início");
            });

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("O campo Id do Cliente precisa ser definido");

        }
    }
}
