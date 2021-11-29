using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class PastoCurralMapping : IEntityTypeConfiguration<PastoCurral>
    {
        public void Configure(EntityTypeBuilder<PastoCurral> builder)
        {
            builder.ToTable("pastocurral");

            builder.HasIndex(e => e.IdCliente)
                   .HasName("fki_PastoCurral");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Capacidade).HasColumnName("capacidade");

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.IdCliente).HasColumnName("idcliente");

            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");

            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");

            builder.Property(e => e.OrdemFornecimento).HasColumnName("ordemfornecimento");

            builder.Property(e => e.Linha)
                .HasColumnName("linha")
                .HasMaxLength(100);

            builder.Property(e => e.Lotacao).HasColumnName("lotacao");

            builder.Property(e => e.Metragemcocho)
                .HasColumnName("metragemcocho")
                .HasColumnType("numeric(15,2)");

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasColumnName("nome")
                .HasMaxLength(100);

            builder.Property(e => e.Numero).HasColumnName("numero");

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("1");

            builder.Property(e => e.Tipo).HasColumnName("tipo");

            builder.HasOne(d => d.Cliente)
                    .WithMany(p => p.Pastocurral)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PastoCurral");

        }
    }
}
