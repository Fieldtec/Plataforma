using FluentValidation;
using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class SuplementoMineralValidation : AbstractValidator<SuplementoMineral>
    {
        public SuplementoMineralValidation(TipoOperacao operacao)
        {
            if (operacao != TipoOperacao.Inclusao)
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

            RuleFor(x => x.IdFornecedor)
                .GreaterThan(0)
                .WithMessage("Id do Fornecedor não foi informado");

        }
    }
}
