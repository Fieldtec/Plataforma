using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class PlanejamentoValoresConfinamentoMapping : IEntityTypeConfiguration<PlanejamentoValoresConfinamento>
    {
        public void Configure(EntityTypeBuilder<PlanejamentoValoresConfinamento> builder)
        {
            builder.ToTable("planejamentovaloresconfinamento");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.DiaInicio).HasColumnName("diaInicio");

            builder.Property(e => e.DiaFim).HasColumnName("diafim");

            builder.Property(e => e.GmdEsperado)
                .HasColumnName("gmdesperado")
                .HasColumnType("numeric(18,3)");

            builder.Property(e => e.IdPlanejamento).HasColumnName("idplanejamento");

            builder.Property(e => e.IdRacao).HasColumnName("idracao");

            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");

            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");

            builder.Property(e => e.ImspvEsperado)
                .HasColumnName("imspvesperado")
                .HasColumnType("numeric(18,3)");

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("1");

            builder.HasOne(d => d.PlanejamentoNutricional)
                .WithMany(p => p.PlanejamentoValoresConfinamento)
                .HasForeignKey(d => d.IdPlanejamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlanejamentoValoresConfinamentoIdPlan");

            builder.HasOne(d => d.Racao)
                .WithMany(p => p.PlanejamentoValoresConfinamento)
                .HasForeignKey(d => d.IdRacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlanejamentoValoresConfinamentoIdRacao");
        }
    }

}
