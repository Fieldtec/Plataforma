using FluentValidation;
using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class PlanejamentoNutricionalValidation : AbstractValidator<PlanejamentoNutricional>
    {
        public PlanejamentoNutricionalValidation(TipoOperacao operacao)
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
                .Must(TerTipoPlanejamento)
                .WithMessage("Tipo do Planejamento não foi informado");

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("Cliente não informado");

            RuleFor(x => x.PlanejamentoValoresConfinamento.Count > 0 || x.PlanejamentoValoresPasto.Count > 0)
               .Equal(true)
               .WithMessage("É necessário cadastrar ao menos 1 valor de planejamento");

            When(x => x.Tipo == TipoPlanejamentoNutricional.Confinamento, () =>
            {
                RuleFor(x => x.PlanejamentoValoresConfinamento.Count > 0)
                    .Equal(true)
                    .WithMessage("É necessário inserir ao menos 1 valor de confinamento");
            });

            When(x => x.Tipo == TipoPlanejamentoNutricional.Pasto, () =>
            {
                RuleFor(x => x.PlanejamentoValoresPasto.Count > 0)
                    .Equal(true)
                    .WithMessage("É necessário inserir ao menos 1 valor de pasto");
            });

            When(x => x.PlanejamentoValoresConfinamento.Count > 0, () =>
            {
                RuleForEach(x => x.PlanejamentoValoresConfinamento)
                    .SetValidator(new PlanejamentoValoresConfinamentoValidation());
            });

            When(x => x.PlanejamentoValoresPasto.Count > 0, () =>
            {
                RuleForEach(x => x.PlanejamentoValoresPasto)
                    .SetValidator(new PlanejamentoValoresPastoValidation());
            });
        }

        private bool TerTipoPlanejamento(TipoPlanejamentoNutricional tipo)
        {
            bool temTipo = false;

            foreach (TipoPlanejamentoNutricional item in Enum.GetValues(typeof(TipoPlanejamentoNutricional)))
            {
                if (tipo == item)
                {
                    temTipo = true;
                    break;
                }
            }

            return temTipo;
        }

    }

    public class PlanejamentoValoresConfinamentoValidation : AbstractValidator<PlanejamentoValoresConfinamento>
    {
        public PlanejamentoValoresConfinamentoValidation()
        {
            RuleFor(x => x.DiaInicio)
                .GreaterThan(0)
                .WithMessage("Dia Início precisa ser maior do que 0");

            RuleFor(x => x.DiaFim)
                .GreaterThan(0)
                .WithMessage("Dia Final precisa ser maior do que 0");

            RuleFor(x => x.DiaFim)
                .GreaterThan(x => x.DiaInicio)
                .WithMessage("Dia Final não pode ser menor que o Dia inicial");

            RuleFor(x => x.IdRacao)
                .GreaterThan(0)
                .WithMessage("Ração não informada");

        }
    }

    public class PlanejamentoValoresPastoValidation : AbstractValidator<PlanejamentoValoresPasto>
    {
        public PlanejamentoValoresPastoValidation()
        {
            RuleFor(x => x.IdCategoria)
                .GreaterThan(0)
                .WithMessage("Categoria não informada");

            RuleFor(x => x.IdSuplemento)
                .GreaterThan(0)
                .WithMessage("Suplemento não informado");

            RuleFor(x => x.IdFase)
                .GreaterThan(0)
                .WithMessage("Fase não informada");

        }
    }

}
