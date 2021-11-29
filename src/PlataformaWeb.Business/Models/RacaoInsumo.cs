using System;
using System.Collections.Generic;

namespace PlataformaWeb.Business.Models
{
    public class RacaoInsumo : Entity
    {
        public int IdRacao { get; set; }
        public int IdInsumoAlimento { get; set; }
        public decimal PercentualMateriaSeca { get; set; }
        public decimal KgMateriaSeca { get; set; }
        public decimal KgMateriaNatural { get; set; }
        public decimal InclusaoMateriaSeca { get; set; }
        public decimal InclusaoMateriaNatural { get; set; }
        public decimal ValorKg { get; set; }
        public decimal ValorInclusao { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual InsumoAlimento InsumoAlimento { get; set; }
        public virtual Racao Racao { get; set; }

        internal void CalcularKgMaterialNatural()
        {
            KgMateriaNatural = Math.Round((KgMateriaSeca / PercentualMateriaSeca) * 100, 3);
        }

        internal void CalcularInclusaoMaterialSeca(decimal totalKgMateriaSecaInsumos)
        {
            InclusaoMateriaSeca = Math.Round((KgMateriaSeca * 100) / totalKgMateriaSecaInsumos, 3);
        }

        internal void CalcularInclusaoMateriaNatural(decimal totalKgMateriaNaturalInsumos)
        {
            InclusaoMateriaNatural = Math.Round((KgMateriaNatural * 100) / totalKgMateriaNaturalInsumos, 3);
        }

        internal void CalcularValorInclusao()
        {
            ValorInclusao = Math.Round(KgMateriaNatural * ValorKg, 3);
        }

    }
}
