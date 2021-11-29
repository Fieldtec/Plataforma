using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models
{
    public class LoteSaida : Entity
    {
        public int NumeroLote { get; set; }
        public DateTime DataEmbarque { get; set; }
        public TipoSaida TipoSaida { get; set; }
        public int? IdProdutorDestino { get; set; }
        public virtual ProdutorParceiro ProdutorDestino { get; set; }
        public int? IdFrigorificoDestino { get; set; }
        public virtual Frigorifico FrigorificoDestino { get; set; }
        public int QuantidadeAnimalPrevisto { get; set; }
        public int QuantidadeAnimaEmbarcado { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual List<Animal> Animais { get; set; }

        public LoteSaida()
        {
            Animais = new List<Animal>();
        }

        public LoteSaida(LoteSaida lote)
        {
            Id = lote.Id;
            NumeroLote = lote.NumeroLote;
            Status = lote.Status;
            DataEmbarque = lote.DataEmbarque;
            TipoSaida = lote.TipoSaida;
            IdProdutorDestino = lote.IdProdutorDestino;
            IdFrigorificoDestino = lote.IdFrigorificoDestino;
            QuantidadeAnimalPrevisto = lote.QuantidadeAnimalPrevisto;
            QuantidadeAnimaEmbarcado = lote.QuantidadeAnimaEmbarcado;
            IdCliente = lote.IdCliente;
        }

    }
}
