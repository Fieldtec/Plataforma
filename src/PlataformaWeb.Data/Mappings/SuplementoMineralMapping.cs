using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class SuplementoMineralMapping : IEntityTypeConfiguration<SuplementoMineral>
    {
        public void Configure(EntityTypeBuilder<SuplementoMineral> builder)
        {
            builder.ToTable("suplementomineral");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.CmCochoIndicado)
                .HasColumnName("cmcochoindicado")
                .HasColumnType("numeric(15,2)");

            builder.Property(e => e.ConsumoEsperado)
                .HasColumnName("consumoesperado")
                .HasColumnType("numeric(15,2)");

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.EstoqueMinimoDias)
                .HasColumnName("estoqueminimodias")
                .HasColumnType("numeric(15,2)");

            builder.Property(e => e.EstoqueMinimoKg)
                .HasColumnName("estoqueminimokg")
                .HasColumnType("numeric(15,2)");

            builder.Property(e => e.GanhoEsperado)
                .HasColumnName("ganhoesperado")
                .HasColumnType("numeric(15,2)");

            builder.Property(e => e.IdFornecedor).HasColumnName("idfornecedor");

            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");

            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasColumnName("nome")
                .HasMaxLength(100);

            builder.Property(e => e.PesoEmbalagem)
                .HasColumnName("pesoembalagem")
                .HasColumnType("numeric(15,2)");

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("1");

            builder.Property(e => e.ValorKg)
                .HasColumnName("valorkg")
                .HasColumnType("numeric(15,2)");

            builder.HasOne(d => d.FornecedorInsumo)
                .WithMany(p => p.SuplementosMinerais)
                .HasForeignKey(d => d.IdFornecedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fksuplementoMineral");

        }
    }
}
