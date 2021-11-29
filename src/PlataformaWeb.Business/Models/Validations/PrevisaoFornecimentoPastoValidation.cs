using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class PrevisaoFornecimentoPastoValidation : AbstractValidator<PrevisaoFornecimentoPasto>
    {
        public PrevisaoFornecimentoPastoValidation()
        {
            RuleFor(x => x.DataPrevisao.Date)
                .GreaterThanOrEqualTo(DateTime.Now.Date)
                .WithMessage("Data Previsão não pode ser menor que a Data de hoje");

            RuleFor(x => x.IdPasto)
                .GreaterThan(0)
                .WithMessage("Pasto previsa ser informado");

            RuleFor(x => x.IdLote)
                .GreaterThan(0)
                .WithMessage("Lote precisa ser informado");

            RuleFor(x => x.QuantidadeAnimais)
                .GreaterThan(0)
                .WithMessage("Quantidade de Animais precisa ser maior do que 0");

            RuleFor(x => x.IdSuplemento)
                .GreaterThan(0)
                .WithMessage("Suplemento precisa ser informado");

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("Cliente precisa ser informado");
        }
    }
}
