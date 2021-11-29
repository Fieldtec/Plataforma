using FluentValidation;
using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class LoteSaidaValidation : AbstractValidator<LoteSaida>
    {
        public LoteSaidaValidation(TipoOperacao operacao)
        {
            if (operacao == TipoOperacao.Atualizacao)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage("Id precisa ser informado");
            }

            RuleFor(x => x.NumeroLote)
                .GreaterThan(0)
                .WithMessage("Número do Lote não pode ser 0");

            RuleFor(x => x.DataEmbarque.Date)
                .NotEqual(DateTime.MinValue.Date)
                .WithMessage("Data de Embarque precisa ser informada");

            RuleFor(x => x.TipoSaida)
                .Must(TerTipoSaida)
                .WithMessage("Tipo Saída é inválido");

            When(x => x.TipoSaida == TipoSaida.Abate, () =>
            {
                RuleFor(x => x.IdFrigorificoDestino)
                    .GreaterThan(0)
                    .WithMessage("Frigorífico Destino precisa ser informado");
            });

            When(x => x.TipoSaida == TipoSaida.Venda, () =>
            {
                RuleFor(x => x.IdProdutorDestino)
                    .GreaterThan(0)
                    .WithMessage("Produtos Destino precisa ser informado");
            });

        }

        public bool TerTipoSaida(TipoSaida tipoSaida)
        {
            bool temSaida = false;

            foreach (TipoSaida item in Enum.GetValues(typeof(TipoSaida)))
            {
                if (item == tipoSaida)
                {
                    temSaida = true;
                    break;
                }
            }

            return temSaida;
        }
    }
}
