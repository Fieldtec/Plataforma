using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models
{
    public class MovimentacaoEntreLote : Entity
    {
        public int IdLoteEntrada { get; set; }
        public LoteEntrada LoteEntrada { get; set; }
        public int IdLocalOrigem { get; set; }
        public PastoCurral LocalOrigem { get; set; }
        public int IdLocalDestino { get; set; }
        public PastoCurral LocalDestino { get; set; }
        public int IdMotivo { get; set; }
        public MotivoMovimentacao Motivo { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public int QuantidadeAnimais { get; set; }
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }

        public MovimentacaoEntreLote()
        {

        }

        public MovimentacaoEntreLote(MovimentacaoEntreLote entity)
        {
            IdCliente = entity.IdCliente;
            DataMovimentacao = entity.DataMovimentacao;
            IdLocalDestino = entity.IdLocalDestino;
            IdLocalOrigem = entity.IdLocalOrigem;
            IdMotivo = entity.IdMotivo;
            IdLoteEntrada = entity.IdLoteEntrada;
            QuantidadeAnimais = entity.QuantidadeAnimais;
            Status = entity.Status;
        }
    }
}
