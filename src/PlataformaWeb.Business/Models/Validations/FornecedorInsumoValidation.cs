using FluentValidation;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Models.Validations.DomainValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class FornecedorInsumoValidation : AbstractValidator<FornecedorInsumo>
    {
        public FornecedorInsumoValidation(TipoOperacao operacao)
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

            RuleFor(x => x.Cidade)
                .NotEmpty()
                .WithMessage("Cidade é obrigatório")
                .MaximumLength(100)
                .WithMessage("Cidade deve ter no máximo 100 caracteres");

            RuleFor(x => x.Uf)
                .NotEmpty()
                .WithMessage("Estado é obrigatório")
                .MaximumLength(2)
                .WithMessage("Estado deve ter no máximo 2 caracteres");


            When(x => !String.IsNullOrEmpty(x.CpfCnpj), () =>
            {
                RuleFor(x => x.CpfCnpj)
                    .Must(DocumentoValidacao.TemDocumentoValido)
                    .WithMessage("CPF/CPNJ inválido");
            });

            When(x => !String.IsNullOrEmpty(x.Telefone), () =>
            {
                RuleFor(x => x.Telefone)
                    .MaximumLength(20)
                    .WithMessage("Telefone deve ter no máximo 20 caracteres");
            });

            When(x => !String.IsNullOrEmpty(x.Email), () =>
            {
                RuleFor(x => x.Email)
                    .Must(EmailValidation.Validar)
                    .MaximumLength(100)
                    .WithMessage("Email deve ter no máximo 100 caracteres");
            });

            When(x => !String.IsNullOrEmpty(x.ContatoPessoa), () =>
            {
                RuleFor(x => x.ContatoPessoa)
                    .MaximumLength(100)
                    .WithMessage("Contato deve ter no máximo 100 caracteres");
            });

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("Id do Cliente não informado");

        }
    }
}
