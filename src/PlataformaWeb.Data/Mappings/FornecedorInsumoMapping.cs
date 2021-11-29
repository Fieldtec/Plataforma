using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class FornecedorInsumoMapping : IEntityTypeConfiguration<FornecedorInsumo>
    {
        public void Configure(EntityTypeBuilder<FornecedorInsumo> builder)
        {
            builder.ToTable("fornecedoreinsumos");

            builder.HasIndex(e => e.IdCliente)
                .HasName("fki_fornecedoreInsumos");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Cidade)
                .IsRequired()
                .HasColumnName("cidade")
                .HasMaxLength(100);

            builder.Property(e => e.ContatoPessoa)
                .HasColumnName("contatopessoa")
                .HasMaxLength(100);

            builder.Property(e => e.CpfCnpj)
                .HasColumnName("cpf_cnpj")
                .HasMaxLength(30);

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(100);

            builder.Property(e => e.IdCliente).HasColumnName("idcliente");

            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");

            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasColumnName("nome")
                .HasMaxLength(100);

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("1");

            builder.Property(e => e.Telefone)
                .HasColumnName("telefone")
                .HasMaxLength(20);

            builder.Property(e => e.Uf)
                .IsRequired()
                .HasColumnName("uf")
                .HasMaxLength(2)
                .IsFixedLength();

            builder.HasOne(d => d.Cliente)
                .WithMany(p => p.FornecedoresInsumos)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FornecedoreInsumos");
        }
    }
}
