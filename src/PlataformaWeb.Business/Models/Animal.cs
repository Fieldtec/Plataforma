using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;

namespace PlataformaWeb.Business.Models
{
    public class Animal : Entity
    {
        public TipoEntradaLote TipoEntrada { get; set; }
        public int IdLote { get; set; }
        public virtual LoteEntrada LoteEntrada { get; set; }
        public int IdRaca { get; set; }
        public virtual Raca Raca { get; set; }
        public int IdCategoria { get; set; }
        public virtual Categoria Categoria { get; set; }
        public int? IdProdutorOrigem { get; set; }
        public virtual ProdutorParceiro ProdutorPaceiro { get; set; }
        public DateTime DataEntrada { get; set; }
        public decimal PesoEntrada { get; set; }
        public int IdadeEntrada { get; set; }
        public decimal ValorCompra { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }

        public int? IdCausaMorte { get; set; }
        public CausaMorte CausaMorte { get; set; }
        public DateTime? DataMorte { get; set; }

        public int? IdLoteSaida { get; set; }
        public virtual LoteSaida LoteSaida { get; set; }
        public DateTime? DataSaida { get; set; }
        public decimal? PesoSaida { get; set; }
        public decimal? PesoProjetado { get; set; }

        public virtual List<MovimentacaoAnimal> Movimentacoes { get; set; }

        public Animal()
        {
            Movimentacoes = new List<MovimentacaoAnimal>();
        }

        public void RegistrarMorte(int idCausaMorte, DateTime dataMorte)
        {
            IdCausaMorte = idCausaMorte;
            DataMorte = dataMorte;
            Status = Status.Morte;
        }

        public void CancelarMorte()
        {
            IdCausaMorte = null;
            DataMorte = null;
            Status = Status.Ativado;
        }

        public void RegistrarSaida(int loteSaida, decimal pesoSaida, DateTime dataSaida)
        {
            IdLoteSaida = loteSaida;
            PesoSaida = pesoSaida;
            DataSaida = dataSaida;
            Status = Status.Fechado;
        }

        public void CancelarSaida()
        {
            IdLoteSaida = null;
            PesoSaida = null;
            DataSaida = null;
            Status = Status.Ativado;
        }

    }
}
