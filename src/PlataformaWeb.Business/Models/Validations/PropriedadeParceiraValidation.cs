using FluentValidation;
using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class PropriedadeParceiraValidation : AbstractValidator<PropriedadeParceira>
    {
        public PropriedadeParceiraValidation(TipoOperacao operacao)
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

            RuleFor(x => x.Cidade)
                .NotNull()
                .WithMessage("O campo Cidade é obrigatório")
                .MaximumLength(100)
                .WithMessage("O campo Cidade precisa ter no máximo 100 caracteres");

            RuleFor(x => x.Uf)
                .NotNull()
                .WithMessage("O campo UF é obrigatório")
                .MaximumLength(2)
                .WithMessage("O campo UF precisa ter no máximo 2 caracteres");

            When(x => !String.IsNullOrEmpty(x.Inscricaoestadual), () =>
            {
                RuleFor(x => x.Uf)                
                    .MaximumLength(30)
                    .WithMessage("O campo Inscrição Estadual precisa ter no máximo 30 caracteres");
            });

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("O campo Id do Cliente precisa ser definido");
        }
    }
}
