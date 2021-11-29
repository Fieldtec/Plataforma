using System;

namespace PlataformaWeb.Business.Models.Cadastro
{
    public class MorteAnimalCadastro : Entity
    {
        public DateTime DataMorte { get; set; }
        public LoteEntrada LoteEntrada { get; set; }
        public PastoCurral Local { get; set; }
        public CausaMorte CausaMorte { get; set; }
        public int QuantidadeAnimais { get; set; }
    }
}
