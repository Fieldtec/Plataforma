using FluentValidation;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Models.Validations.DomainValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class ProdutorParceiroValidation : AbstractValidator<ProdutorParceiro>
    {
        public ProdutorParceiroValidation(TipoOperacao operacao)
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

            When(x => !String.IsNullOrEmpty(x.CpfCnpj), () =>
            {
                RuleFor(x => x.CpfCnpj)
                    .Must(DocumentoValidacao.TemDocumentoValido)
                    .WithMessage("CPF/CNPJ inválido");
            });

            RuleFor(x => x.IdPropriedadeParceira)
                .GreaterThan(0)
                .WithMessage("O campo Id da Propriedade Parceira precisa ser definido");

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("O campo Id do Cliente precisa ser definido");
        }
    }
}
