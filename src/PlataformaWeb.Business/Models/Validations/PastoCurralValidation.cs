using FluentValidation;
using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class PastoCurralValidation : AbstractValidator<PastoCurral>
    {
        public PastoCurralValidation(TipoOperacao operacao)
        {
            if (operacao == TipoOperacao.Atualizacao)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage("Id precisa ser definido");
            }

            RuleFor(x => x.Tipo)
                .Must(TerTipoValido)
                .WithMessage("Tipo inválido");

            When(x => !String.IsNullOrEmpty(x.Linha), () =>
            {
                RuleFor(x => x.Linha)
                    .MaximumLength(100)
                    .WithMessage("O campo Linha precisa ter no máximo 100 caracteres");
            });

            RuleFor(x => x.Nome)
                .NotNull()
                .WithMessage("O campo Nome é obrigatório")
                .MaximumLength(100)
                .WithMessage("O campo Nome precisa ter no máximo 100 caracteres");

            RuleFor(x => x.Capacidade)
                .GreaterThan(0)
                .WithMessage("O campo Capacidade precisa ser maior do que 0");

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("O campo Id do Cliente precisa ser definido");

        }

        private bool TerTipoValido(TipoPastoCurral tipo)
        {
            return tipo == TipoPastoCurral.Curral || tipo == TipoPastoCurral.Pasto;
        }
    }
}
