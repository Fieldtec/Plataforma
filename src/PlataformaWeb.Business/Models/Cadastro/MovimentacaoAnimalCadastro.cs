using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Business.Models.Cadastro
{
    public class MovimentacaoAnimalCadastro : Entity
    {
        public PastoCurral LocalOrigem { get; set; }
        public PastoCurral LocalDestino { get; set; }
        public LoteEntrada LoteOrigem { get; set; }
        public LoteEntrada LoteDestino { get; set; }
        public MotivoMovimentacao Motivo { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public int QuantidadeAnimais { get; set; }
    }
}
