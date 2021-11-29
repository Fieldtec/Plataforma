using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class RacaoMapping : IEntityTypeConfiguration<Racao>
    {
        public void Configure(EntityTypeBuilder<Racao> builder)
        {
            builder.ToTable("racao");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");

            builder.Property(e => e.DataFormulacao)
                .HasColumnName("dataformulacao")
                .HasColumnType("date");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.Gmd)
                .HasColumnName("gmd")
                .HasColumnType("numeric(15,3)");

            builder.Property(e => e.IdCliente).HasColumnName("idcliente");

            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");

            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");

            builder.Property(e => e.MateriaSeca)
                .HasColumnName("materiaseca")
                .HasColumnType("numeric(15,3)")
                .HasComment("Valor calculado ");

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasColumnName("nome")
                .HasMaxLength(100);

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("1");

            builder.Property(e => e.Tipo).HasColumnName("tipo");

            builder.Property(e => e.ValorKg)
                .HasColumnName("valorkg")
                .HasColumnType("numeric(15,3)")
                .HasComment("Valor calculado pelos alimentos que compõem a mesma");

            builder.HasOne(d => d.Cliente)
                .WithMany(p => p.Racoes)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RacaoCliente");
        }
    }
}
