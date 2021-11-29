using FluentValidation;
using PlataformaWeb.Business.Models.Cadastro;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class MorteAnimalCadastroValidation : AbstractValidator<MorteAnimalCadastro>
    {
        public MorteAnimalCadastroValidation()
        {
            RuleFor(x => x.DataMorte.Date)
                .LessThanOrEqualTo(DateTime.Now.Date)
                .WithMessage("Data da Morte não pode ser maior do que a Data de Hoje");

            RuleFor(x => x.CausaMorte)
                .NotNull()
                .WithMessage("Causa da Morte precisa ser informado");

            RuleFor(X => X.Local)
                .NotNull()
                .WithMessage("Local precisa ser informado");

            RuleFor(x => x.LoteEntrada)
                .NotNull()
                .WithMessage("Lote de Entrada precisa ser informado");

            RuleFor(x => x.QuantidadeAnimais)
                .GreaterThan(0)
                .WithMessage("Quantidade de Animais precisa ser maior do que 0");
        }
    }
}
