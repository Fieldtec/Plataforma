using FluentValidation;
using PlataformaWeb.Business.Enums;

namespace PlataformaWeb.Business.Models.Validations
{
    public class CausaMorteValidation : AbstractValidator<CausaMorte>
    {
        public CausaMorteValidation(TipoOperacao operacao)
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

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("O campo Id do Cliente precisa ser definido");

        }
    }
}
