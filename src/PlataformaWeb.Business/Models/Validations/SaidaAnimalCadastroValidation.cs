using FluentValidation;
using PlataformaWeb.Business.Models.Cadastro;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class SaidaAnimalCadastroValidation : AbstractValidator<SaidaAnimalCadastro>
    {
        public SaidaAnimalCadastroValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id do Lote de Saída não informado");

            RuleFor(x => x.Lotes.Count > 0)
                .Equal(true)
                .WithMessage("Lançamento da saída precisa ter ao menos um local com animais embarcados");

            RuleForEach(x => x.Lotes)
                .SetValidator(new SaidaAnimalLoteValidation());            
        }
    }

    public class SaidaAnimalLoteValidation : AbstractValidator<SaidaAnimalLote>
    {
        public SaidaAnimalLoteValidation()
        {
            RuleFor(x => x.Local)
                .NotNull()
                .WithMessage("Local não informado");

            RuleFor(x => x.Lote)
                .NotNull()
                .WithMessage("Lote não informado");

            RuleFor(x => x.QuantidadeEmbarcado)
                .GreaterThan(0)
                .WithMessage(e => $"Quantidade embarcado no local {e.Local?.Nome} precisa ser maior do que 0");

            RuleFor(x => x.PesoMedio)
                .GreaterThan(0)
                .WithMessage(e => $"Peso médio no local {e.Local?.Nome} precisa ser maior do que 0");
        }
    }

    
}
