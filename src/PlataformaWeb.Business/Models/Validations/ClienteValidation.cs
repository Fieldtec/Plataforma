using PlataformaWeb.Business.Enums;
using System;
using FluentValidation;
using PlataformaWeb.Business.Models.Validations.DomainValidation;

namespace PlataformaWeb.Business.Models.Validations
{
    public class ClienteValidation : PessoaValidation<Cliente>
    {
        public ClienteValidation(TipoOperacao operacao)
        {
            if (operacao != TipoOperacao.Inclusao)
            {
                ValidarId();
            }

            ValidarNome();
            ValidarEmail();
            ValidarUsuario();

            if (operacao == TipoOperacao.Inclusao)
                ValidarSenha();

            ValidarTelefone();

            RuleFor(x => x.IdTecnico)
                .GreaterThan(0)
                .WithMessage("Técnico não definido");                
            
            RuleFor(x => x.NomePropriedade)
                .NotEmpty()
                .WithMessage("Nome da Propriedade é obrigatório")
                .MaximumLength(100)
                .WithMessage("Nome da Propriedade deve ter no máximo 100 caracteres");

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

            RuleFor(x => x.QtdeAnimais)
                .GreaterThan(0)
                .WithMessage("Qtd. de animais precisa ser maior do que 0");

            RuleFor(x => x.CpfCnpj)
                .Must(DocumentoValidacao.TemDocumentoValido)
                .WithMessage("CPF/CPNJ inválido");
        }

    }
}
