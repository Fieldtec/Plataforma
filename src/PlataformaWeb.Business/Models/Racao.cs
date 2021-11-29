using PlataformaWeb.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlataformaWeb.Business.Models
{
    public class Racao : Entity
    {
        public Racao()
        {
            InsumosRacao = new List<RacaoInsumo>();
            PlanejamentoValoresConfinamento = new List<PlanejamentoValoresConfinamento>();
            FornecimentosConfinamento = new List<FornecimentoConfinamento>();
        }

        public TipoRacao Tipo { get; set; }
        public string Nome { get; set; }
        public decimal? Gmd { get; set; }
        public decimal MateriaSeca { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public decimal? ValorKg { get; set; }
        public DateTime DataFormulacao { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual List<RacaoInsumo> InsumosRacao { get; set; }

        public virtual List<PlanejamentoValoresConfinamento> PlanejamentoValoresConfinamento { get; set; }
        public virtual List<FornecimentoConfinamento> FornecimentosConfinamento { get; set; }

        internal void CalcularInformacoesInsumos()
        {
            CalcularValoresInsumos();
            CalcularMateriaSeca();
            CalcularValorKg();
        }

        void CalcularValoresInsumos()
        {
            foreach (var insumo in InsumosRacao)
            {
                insumo.CalcularKgMaterialNatural();
                insumo.CalcularInclusaoMaterialSeca(InsumosRacao.Sum(x => x.KgMateriaSeca));
                insumo.CalcularValorInclusao();
            }

            //Ficou de fora do loop anterior pois primeiro é necessário calcular todos os KG de Matéria Natural
            foreach (var insumo in InsumosRacao)
            {                
                insumo.CalcularInclusaoMateriaNatural(InsumosRacao.Sum(x => x.KgMateriaNatural));
            }

        }

        void CalcularMateriaSeca()
        {
            if (TemInsumos())
                MateriaSeca = Math.Round((InsumosRacao.Sum(x => x.KgMateriaSeca) * 100) / InsumosRacao.Sum(x => x.KgMateriaNatural), 3);
            else
                MateriaSeca = 0;
        }

        void CalcularValorKg()
        {
            if (TemInsumos())
                ValorKg = Math.Round(InsumosRacao.Sum(x => x.ValorInclusao) / InsumosRacao.Sum(x => x.KgMateriaNatural), 3);
            else
                ValorKg = null;
        }

        bool TemInsumos()
        {
            return InsumosRacao != null && InsumosRacao.Count > 0;
        }

    }
}
