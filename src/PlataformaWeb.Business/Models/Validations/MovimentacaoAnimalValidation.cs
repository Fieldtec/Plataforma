using FluentValidation;
using PlataformaWeb.Business.Enums;
using System;

namespace PlataformaWeb.Business.Models.Validations
{
    public class MovimentacaoAnimalValidation : AbstractValidator<MovimentacaoAnimal>
    {
        public MovimentacaoAnimalValidation(TipoOperacao operacao, TipoOperacao operacaoLoteDestino)
        {
            if (operacao == TipoOperacao.Atualizacao)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage("Id precisa ser definido");
            }

            if (operacaoLoteDestino == TipoOperacao.Atualizacao)
            {
                RuleFor(x => x.IdLoteDestino)
                   .GreaterThan(0)
                   .WithMessage("Lote de Destino precisa ser definido");
            }

            RuleFor(x => x.IdLoteOrigem)
               .GreaterThan(0)
               .WithMessage("Lote de Origem precisa ser definido");

            RuleFor(x => x.IdLocalOrigem)
               .GreaterThan(0)
               .WithMessage("Local de Origem precisa ser definido");

            RuleFor(x => x.IdLocalDestino)
               .GreaterThan(0)
               .WithMessage("Local de Destino precisa ser definido");

            RuleFor(x => x.IdMotivo)
               .GreaterThan(0)
               .WithMessage("Motivo precisa ser definido");

            RuleFor(x => x.DataMovimentacao.Date)
               .NotEqual(DateTime.MinValue.Date)
               .WithMessage("Data da Movimentação precisa ser definido")
               .LessThanOrEqualTo(DateTime.Now.Date)
               .WithMessage("Data da Movimentação precisa ser menor ou igual a Data de Hoje");

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("O campo Id do Cliente precisa ser definido");

            RuleFor(x => x.IdAnimal)
                .GreaterThan(0)
                .WithMessage("Id do Animal precisa ser definido");

        }
    }
}
