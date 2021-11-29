using System;

namespace PlataformaWeb.Business.Models
{
    public class MovimentacaoAnimal : Entity
    {
        public int IdAnimal { get; set; }
        public virtual Animal Animal { get; set; }
        public int IdLoteOrigem { get; set; }
        public LoteEntrada LoteOrigem { get; set; }
        public int IdLoteDestino { get; set; }
        public LoteEntrada LoteDestino { get; set; }
        public int IdLocalOrigem { get; set; }
        public PastoCurral LocalOrigem { get; set; }
        public int IdLocalDestino { get; set; }
        public PastoCurral LocalDestino { get; set; }
        public int IdMotivo { get; set; }
        public MotivoMovimentacao Motivo { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }

        public MovimentacaoAnimal()
        {

        }

        public MovimentacaoAnimal(MovimentacaoAnimal entity)
        {
            IdCliente = entity.IdCliente;
            DataMovimentacao = entity.DataMovimentacao;
            IdLocalDestino = entity.IdLocalDestino;
            IdLocalOrigem = entity.IdLocalOrigem;
            IdMotivo = entity.IdMotivo;
            IdLoteOrigem = entity.IdLoteOrigem;
            IdLoteDestino = entity.IdLoteDestino;
            IdAnimal = entity.IdAnimal;
            Status = entity.Status;
        }
    }

}
