using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class NotaLeituraCochoValidation : AbstractValidator<NotaLeituraCocho>
    {
        public NotaLeituraCochoValidation()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Nome da Nota é obrigatório")
                .MaximumLength(100)
                .WithMessage("NOme da Nota precisa ter no máximo 100 caracteres");

            RuleFor(x => x.AjustePorcentagem)
                .NotNull()
                .WithMessage("Ajuste da porcentagem é obrigatório");

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("Cliente precisa ser informado");
        }
    }
}
