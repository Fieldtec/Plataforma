using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class PropriedadeParceiraMapping : IEntityTypeConfiguration<PropriedadeParceira>
    {
        public void Configure(EntityTypeBuilder<PropriedadeParceira> builder)
        {
            builder.ToTable("propriedadeparceira");

            builder.HasIndex(e => e.IdCliente)
                    .HasName("fki_Propriedadeparceira");

            builder.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");

            builder.Property(e => e.Cidade)
                .IsRequired()
                .HasColumnName("cidade")
                .HasMaxLength(100);

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.IdCliente).HasColumnName("idcliente");

            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");

            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");

            builder.Property(e => e.Inscricaoestadual)
                .HasColumnName("inscricaoestadual")
                .HasMaxLength(30);

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasColumnName("nome")
                .HasMaxLength(100);

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("1");

            builder.Property(e => e.Uf)
                .IsRequired()
                .HasColumnName("uf")
                .HasMaxLength(2)
                .IsFixedLength();

            builder.HasOne(d => d.Cliente)
                    .WithMany(p => p.Propriedadeparceira)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PropriedaedParceiraCliente");
        }
    }
}
