using FluentValidation;
using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class RacaoValidation : AbstractValidator<Racao>
    {
        public RacaoValidation(TipoOperacao operacao, bool validarCamposAposRealizarCalculo)
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

            RuleFor(x => x.Tipo)
                .Must(TerTipoRacao)
                .WithMessage("Tipo da Ração não foi informado");

            RuleFor(x => x.DataFormulacao.Date == DateTime.MinValue)
                .Equal(false)
                .WithMessage("Data da Formulação é inválida");

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("Id do Cliente não foi informado");

            if (validarCamposAposRealizarCalculo)
            {
                RuleFor(x => x.MateriaSeca)
                    .GreaterThan(0)
                    .WithMessage("Materia Seca deve ser maior do que 0");

                When(x => x.ValorKg.HasValue, () =>
                {
                    RuleFor(x => x.ValorKg)
                        .GreaterThan(0)
                        .WithMessage("Valor do KG precisa ser maior do que 0");
                });
            }

            RuleFor(x => x.InsumosRacao.Count)
                .GreaterThan(0)
                .WithMessage("É necessário cadastrar ao menos 1 insumo");

            RuleForEach(x => x.InsumosRacao)
                .SetValidator(new RacaoInsumoValidation(validarCamposAposRealizarCalculo));
        }

        private bool TerTipoRacao(TipoRacao tipoRacao)
        {
            bool temTipo = false;

            foreach (TipoRacao item in Enum.GetValues(typeof(TipoRacao)))
            {
                if (tipoRacao == item)
                {
                    temTipo = true;
                    break;
                }
            }

            return temTipo;
        }

    }

    public class RacaoInsumoValidation : AbstractValidator<RacaoInsumo>
    {
        public RacaoInsumoValidation(bool aposCalculo)
        {
            RuleFor(x => x.IdInsumoAlimento)
                .GreaterThan(0)
                .WithMessage("Insumo deve ser informado");

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("Id do Cliente deve ser informado");

            RuleFor(x => x.PercentualMateriaSeca)
                .GreaterThan(0)
                .WithMessage("Percentual Matéria Seca deve ser maior do que à 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Percentual Matéria Seca deve ser menor ou igual à 100");

            RuleFor(x => x.KgMateriaSeca)
                .GreaterThan(0)
                .WithMessage("KG da Matéria Seca deve ser maior do que 0");


            if (aposCalculo)
            {

                RuleFor(x => x.KgMateriaNatural)
                    .GreaterThan(0)
                    .WithMessage("KG da Matéria Natural deve ser maior do que 0");

                RuleFor(x => x.InclusaoMateriaSeca)
                   .GreaterThan(0)
                   .WithMessage("Inclusão Matéria Seca deve ser maior do que 0");

                RuleFor(x => x.InclusaoMateriaNatural)
                   .GreaterThan(0)
                   .WithMessage("Inclusão Matéria Natural deve ser maior do que 0");

                RuleFor(x => x.ValorInclusao)
                    .GreaterThan(0)
                    .WithMessage("Valor da Inclusão deve ser maior do que 0");

            }

            RuleFor(x => x.ValorKg)
               .GreaterThan(0)
               .WithMessage("Valor do KG deve ser maior do que 0");



        }
    }




}
