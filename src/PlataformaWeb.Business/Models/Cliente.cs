using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;

namespace PlataformaWeb.Business.Models
{
    public class Cliente : PessoaBase
    {
        public Cliente()
        {
            Categoria = new List<Categoria>();
            Pastocurral = new List<PastoCurral>();
            Produtorparceiro = new List<ProdutorParceiro>();
            Propriedadeparceira = new List<PropriedadeParceira>();
            Raca = new List<Raca>();
            Usuariocliente = new List<UsuarioCliente>();
            FasesDoAno = new List<FaseDoAno>();
            FornecedoresInsumos = new List<FornecedorInsumo>();
            Racoes = new List<Racao>();
            InsumosRacoes = new List<RacaoInsumo>();
            PlanejamentosNutricionais = new List<PlanejamentoNutricional>();
            LotesDeEntrada = new List<LoteEntrada>();
            MotivosMovimentacoes = new List<MotivoMovimentacao>();
            CausaMortes = new List<CausaMorte>();
            Frigorificos = new List<Frigorifico>();
            MovimentacoesEntreLote = new List<MovimentacaoEntreLote>();
            MovimentacoesAnimal = new List<MovimentacaoAnimal>();
            NotasLeituraCocho = new List<NotaLeituraCocho>();
            LeiturasCocho = new List<LeituraCocho>();
            FornecimentosConfinamento = new List<FornecimentoConfinamento>();
            ConfiguracaoFornecimentoPasto = new List<ConfiguracaoFornecimentoPasto>();
            PrevisoesFornecimentoPasto = new List<PrevisaoFornecimentoPasto>();
            FornecimentosPasto = new List<FornecimentoPasto>();
        }

        public int IdTecnico { get; set; }
        public string NomePropriedade { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public int QtdeAnimais { get; set; }
        public DateTime? DataValidadeLicenca { get; set; }
        public string CpfCnpj { get; set; }
        public decimal? AreaHectare { get; set; }
        public virtual Tecnico Tecnico { get; set; } 
        public virtual List<Categoria> Categoria { get; set; } 
        public virtual List<PastoCurral> Pastocurral { get; set; } 
        public virtual List<ProdutorParceiro> Produtorparceiro { get; set; } 
        public virtual List<PropriedadeParceira> Propriedadeparceira { get; set; } 
        public virtual List<Raca> Raca { get; set; } 
        public virtual List<UsuarioCliente> Usuariocliente { get; set; } 
        public virtual List<FaseDoAno> FasesDoAno { get; set; }
        public virtual List<FornecedorInsumo> FornecedoresInsumos { get; set; }
        public virtual List<Racao> Racoes { get; set; }
        public virtual List<RacaoInsumo> InsumosRacoes { get; set; }
        public virtual List<PlanejamentoNutricional> PlanejamentosNutricionais { get; set; }
        public virtual List<LoteEntrada> LotesDeEntrada { get; set; }
        public virtual List<LoteSaida> LotesDeSaida { get; set; }
        public virtual List<Frigorifico> Frigorificos { get; set; }
        public virtual List<CausaMorte> CausaMortes { get; set; }
        public virtual List<MotivoMovimentacao> MotivosMovimentacoes { get; set; }
        public virtual List<MovimentacaoEntreLote> MovimentacoesEntreLote { get; set; }
        public virtual List<MovimentacaoAnimal> MovimentacoesAnimal { get; set; }
        public virtual List<NotaLeituraCocho> NotasLeituraCocho { get; set; }
        public virtual List<LeituraCocho> LeiturasCocho { get; set; }
        public virtual List<FornecimentoConfinamento> FornecimentosConfinamento { get; set; }
        public virtual List<ConfiguracaoFornecimentoPasto> ConfiguracaoFornecimentoPasto { get; set; }
        public virtual List<PrevisaoFornecimentoPasto> PrevisoesFornecimentoPasto { get; set; }
        public virtual List<FornecimentoPasto> FornecimentosPasto { get; set; }
    }
}
