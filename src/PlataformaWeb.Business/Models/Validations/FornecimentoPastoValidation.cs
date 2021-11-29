using FluentValidation;
using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class FornecimentoPastoValidation : AbstractValidator<FornecimentoPasto>
    {
        public FornecimentoPastoValidation()
        {
            RuleFor(x => x.DataRealizado)
                .NotEqual(DateTime.MinValue)
                .WithMessage("Data do Fornecimento precisa ser informado");

            RuleFor(x => x.IdPasto)
                .GreaterThan(0)
                .WithMessage("Pasto precisa ser definido");

            RuleFor(x => x.IdLote)
                .GreaterThan(0)
                .WithMessage("Lote precisa ser definido");

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("Cliente ser definido");

            RuleFor(x => x.IdSuplemento)
                .GreaterThan(0)
                .WithMessage("Suplemento precisa ser definido");

            RuleFor(x => x.Origem)
                .Must(TerOrigem)
                .WithMessage("Origem precisa ser informado");

            RuleFor(x => x.Destino)
                .Must(TerDestino)
                .WithMessage("Destino precisa ser informado");

        }

        protected bool TerOrigem(OrigemSuplemento origem)
        {
            bool temTipo = false;
            foreach (OrigemSuplemento item in Enum.GetValues(typeof(OrigemSuplemento)))
            {
                if (origem == item)
                {
                    temTipo = true;
                    break;
                }
            }

            return temTipo;
        }

        protected bool TerDestino(DestinoSuplemento destino)
        {
            bool temTipo = false;
            foreach (DestinoSuplemento item in Enum.GetValues(typeof(DestinoSuplemento)))
            {
                if (destino == item)
                {
                    temTipo = true;
                    break;
                }
            }

            return temTipo;
        }


    }
}
