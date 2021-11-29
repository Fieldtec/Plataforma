using PlataformaWeb.Business.Models.Cadastro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlataformaWeb.Business.Models
{
    public class LoteEntrada : Entity
    {
        public int IdLocal { get; set; }
        public virtual PastoCurral Local { get; set; }
        public int IdPlanejamento { get; set; }
        public virtual PlanejamentoNutricional Planejamento { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }  
        public virtual List<Animal> AnimaisLote { get; set; }
        public virtual List<MovimentacaoEntreLote> MovimentacoesEntreLote { get; set; }
        public virtual List<MovimentacaoAnimal> MovimentacoesAnimalDestino { get; set; }
        public virtual List<MovimentacaoAnimal> MovimentacoesAnimalOrigem { get; set; }
        public virtual List<LeituraCocho> LeiturasCocho { get; set; }
        public virtual List<FornecimentoConfinamento> FornecimentosConfinamento { get; set; }
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }
        public int? IdFasePlanejamento { get; set; }
        //public PlanejamentoValoresConfinamento FasePlanejamento { get; set; }
        public int? IdRacaoAtual { get; set; }
        public Racao RacaoAtual { get; set; }
        public decimal? PesoMedioProjetado { get; set; }
        public int? DiaAtual { get; set; }

        public virtual List<PrevisaoFornecimentoPasto> PrevisaoFornecimentosPasto { get; set; }
        public virtual List<FornecimentoPasto> FornecimentosPasto { get; set; }

        public LoteEntrada()
        {
            AnimaisLote = new List<Animal>();
            MovimentacoesEntreLote = new List<MovimentacaoEntreLote>();
            MovimentacoesAnimalDestino = new List<MovimentacaoAnimal>();
            MovimentacoesAnimalOrigem = new List<MovimentacaoAnimal>();
            FornecimentosConfinamento = new List<FornecimentoConfinamento>();
            LeiturasCocho = new List<LeituraCocho>();
            PrevisaoFornecimentosPasto = new List<PrevisaoFornecimentoPasto>();
            FornecimentosPasto = new List<FornecimentoPasto>();
        }

        public LoteEntrada(LoteAnimalCadastro loteAnimal, bool comAnimais = true)
            : this()
        {
            Id = loteAnimal.Id;
            IdLocal = loteAnimal.Local?.Id ?? 0;
            IdPlanejamento = loteAnimal.Planejamento?.Id ?? 0;
            DataEntrada = loteAnimal.DataEntrada;
            Status = loteAnimal.Status;

            if (comAnimais)
            {
                for (int i = 0; i < loteAnimal.QuantidadeAnimais; i++)
                {
                    //gerando lista de animais
                    AnimaisLote.Add(new Animal
                    {
                        Id = 0,
                        TipoEntrada = loteAnimal.TipoEntrada,
                        IdLote = loteAnimal.Id,
                        IdRaca = loteAnimal.Raca?.Id ?? 0,
                        IdCategoria = loteAnimal.Categoria?.Id ?? 0,
                        DataEntrada = loteAnimal.DataEntrada,
                        PesoEntrada = loteAnimal.PesoEntrada,
                        IdadeEntrada = loteAnimal.IdadeEntrada,
                        IdProdutorOrigem = loteAnimal.ProdutorParceiro?.Id,
                        ValorCompra = loteAnimal.ValorCompra,
                        Status = Enums.Status.Ativado
                    });
                }
            }

        }

        #region ... Lógicas de Negócio ...

        /// <summary>
        /// Realiza a transferência de animais de um lote para outro e retorna a Lista de Animais transferidos
        /// </summary>
        /// <param name="loteDestino">Lote Destino</param>
        /// <param name="quantidadeAnimais">Quantidade de Animais a serem transferidos para o lote</param>
        /// <returns>Lista de Animais transferidos</returns>
        public List<Animal> TransferirAnimais(LoteEntrada loteDestino, int quantidadeAnimais)
        {
            var animaisLoteOrigem = this.AnimaisLote.Take(quantidadeAnimais).ToList();
            animaisLoteOrigem.ForEach(x => x.IdLote = loteDestino.Id);
            loteDestino.AnimaisLote.AddRange(animaisLoteOrigem);

            return animaisLoteOrigem;
        }

        public List<Animal> RealizarMorteAnimais(int quantidadeAnimais, int idCausaMorte, DateTime dataMorte)
        {
            var animais = this.AnimaisLote.Take(quantidadeAnimais).ToList();

            animais.ForEach(x => x.RegistrarMorte(idCausaMorte, dataMorte));

            return animais;
        }

        public List<Animal> RealizarSaidaAnimais(LoteSaida loteSaida, int quantidadeAnimais, decimal pesoMedio)
        {
            var animais = this.AnimaisLote.Take(quantidadeAnimais).ToList();
            animais.ForEach(x => x.RegistrarSaida(loteSaida.Id, pesoMedio, loteSaida.DataEmbarque));

            return animais;
        }

        #endregion

    }
}
