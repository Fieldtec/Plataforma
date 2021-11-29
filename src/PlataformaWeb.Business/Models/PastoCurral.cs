using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;

namespace PlataformaWeb.Business.Models
{
    public class PastoCurral : Entity
    {
        public TipoPastoCurral Tipo { get; set; }
        public string Linha { get; set; }
        public int? Numero { get; set; }
        public string Nome { get; set; }
        public int Capacidade { get; set; }
        public int? Lotacao { get; set; }
        public decimal? Metragemcocho { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }
        public int? OrdemFornecimento { get; set; }
        public virtual List<LoteEntrada> LotesEntrada { get; set; }
        public virtual List<MovimentacaoEntreLote> MovimentacoesEntreLoteDestino { get; set; }
        public virtual List<MovimentacaoEntreLote> MovimentacoesEntreLoteOrigem { get; set; }
        public virtual List<MovimentacaoAnimal> MovimentacoesAnimalDestino { get; set; }
        public virtual List<MovimentacaoAnimal> MovimentacoesAnimalOrigem { get; set; }
        public virtual List<LeituraCocho> LeiturasCocho { get; set; }
        public virtual List<FornecimentoConfinamento> FornecimentosConfinamento { get; set; }
        public virtual List<PrevisaoFornecimentoPasto> PrevisoesFornecimentoPasto { get; set; }
        public virtual List<FornecimentoPasto> FornecimentosPasto { get; set; }

        public PastoCurral()
        {
            LotesEntrada = new List<LoteEntrada>();
            MovimentacoesEntreLoteDestino = new List<MovimentacaoEntreLote>();
            MovimentacoesEntreLoteOrigem = new List<MovimentacaoEntreLote>();
            MovimentacoesAnimalDestino = new List<MovimentacaoAnimal>();
            MovimentacoesAnimalOrigem = new List<MovimentacaoAnimal>();
            LeiturasCocho = new List<LeituraCocho>();
            FornecimentosConfinamento = new List<FornecimentoConfinamento>();
            PrevisoesFornecimentoPasto = new List<PrevisaoFornecimentoPasto>();
            FornecimentosPasto = new List<FornecimentoPasto>();
        }

        public void IncluirAnimais(int quantidadeAnimais)
        {
            if (!Lotacao.HasValue) Lotacao = 0;
            Lotacao += quantidadeAnimais;
        }

        public void RetirarAnimais(int quantidadeAnimais)
        {
            if (!Lotacao.HasValue) Lotacao = 0;
            Lotacao -= quantidadeAnimais;

            if (Lotacao < 0) Lotacao = 0;
        }

    }
}
