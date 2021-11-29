using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class ProdutorParceiroMapping : IEntityTypeConfiguration<ProdutorParceiro>
    {
        public void Configure(EntityTypeBuilder<ProdutorParceiro> builder)
        {
            builder.ToTable("produtorparceiro");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.HasIndex(e => e.IdCliente)
                    .HasName("ProdutorParceiroCliente");

            builder.HasIndex(e => e.IdPropriedadeParceira)
                    .HasName("fki_ProdutorParceiroPropriedade");

            builder.Property(e => e.CpfCnpj)
                .HasColumnName("cpf_cnpj")
                .HasMaxLength(30);

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.IdCliente).HasColumnName("idcliente");

            builder.Property(e => e.IdPropriedadeParceira).HasColumnName("idpropriedadeparceira");

            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");

            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasColumnName("nome")
                .HasMaxLength(100);

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("1");

            builder.HasOne(d => d.Cliente)
                    .WithMany(p => p.Produtorparceiro)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ProdutorParceiroCliente");

            builder.HasOne(d => d.PropriedadeParceira)
                    .WithMany(p => p.ProdutoresParceiros)
                    .HasForeignKey(d => d.IdPropriedadeParceira)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ProdutorParceiroPropriedade");

        }
    }
}
