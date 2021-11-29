using FluentValidation;
using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Validations
{
    public class LoteEntradaValidation : AbstractValidator<LoteEntrada>
    {
        public LoteEntradaValidation(TipoOperacao operacao)
        {
            if (operacao != TipoOperacao.Inclusao)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage("Id precisa ser definido");
            }

            RuleFor(x => x.IdLocal)
              .GreaterThan(0)
              .WithMessage("Local precisa ser definido");              

            RuleFor(x => x.IdPlanejamento)
                .GreaterThan(0)
                .WithMessage("Planejamento precisa ser definido");

            if (operacao == TipoOperacao.Inclusao)
            {
                RuleFor(x => x.DataEntrada.Date)
                    .LessThanOrEqualTo(DateTime.Now.Date)
                    .WithMessage("Data da Entrada não pode ser maior do que a Data de Hoje");
            }

            RuleFor(x => x.IdCliente)
                .GreaterThan(0)
                .WithMessage("Id do Cliente não foi informado");            

            RuleFor(x => x.AnimaisLote.Count)
                .GreaterThan(0)
                .WithMessage("É necessário cadastrar ao menos 1 um animal");

            RuleForEach(x => x.AnimaisLote)
                .SetValidator(new AnimalValidation(operacao));
        }
    }

    public class AnimalValidation : AbstractValidator<Animal>
    {
        public AnimalValidation(TipoOperacao operacao)
        {
            if (operacao == TipoOperacao.Atualizacao)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage("Id do Animal precisa ser definido");
            }

            RuleFor(x => x.TipoEntrada)
                .Must(TerTipoCompra)
                .WithMessage("Tipo da Entrada é inválido");

            RuleFor(x => x.IdRaca)
                .GreaterThan(0)
                .WithMessage("Raça precisa ser definida");

            RuleFor(x => x.IdCategoria)
                .GreaterThan(0)
                .WithMessage("Categoria precisa ser definida");

            if (operacao == TipoOperacao.Inclusao)
            {
                RuleFor(x => x.DataEntrada.Date)
                    .LessThanOrEqualTo(DateTime.Now.Date)
                    .WithMessage("Raça precisa ser definida");
            }

            RuleFor(x => x.PesoEntrada)
                .GreaterThan(0)
                .WithMessage("Peso do Animal precisa ser maior do que 0");

            RuleFor(x => x.IdadeEntrada)
                .GreaterThan(0)
                .WithMessage("Idade do Animal precisa ser maior do que 0");

            When(x => x.TipoEntrada == TipoEntradaLote.Compra, () =>
            {
                RuleFor(x => x.ValorCompra)
                .GreaterThan(0)
                .WithMessage("Valor da Compra previca ser definida");
            });

            When(x => x.TipoEntrada == TipoEntradaLote.Nascimento, () =>
            {
                RuleFor(x => x.IdProdutorOrigem)
                    .GreaterThan(0)
                    .WithMessage("É necessário informadr o Produtor de Origem");
            });

        }

        protected bool TerTipoCompra(TipoEntradaLote tipo)
        {
            bool temTipo = false;
            foreach (TipoEntradaLote item in Enum.GetValues(typeof(TipoEntradaLote)))
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

}
