using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class PessoaMapping : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("pessoas");

            builder.RegistrarPessoaMapping();
        }
    }

    public static class PessoaMappingExtension
    {
        public static void RegistrarPessoaMapping<T>(this OwnedNavigationBuilder<T, PessoaBase> builder) where T : class
        {

            builder.Property(e => e.Id)
               .HasColumnName("id")
               .HasDefaultValueSql("nextval('pessoas_id_seq'::regclass)");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasColumnName("email")
                .HasMaxLength(100);

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasColumnName("nome")
                .HasMaxLength(100);

            builder.Property(e => e.Senha)
                .IsRequired()
                .HasColumnName("senha")
                .HasMaxLength(100);

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("1");

            builder.Property(e => e.Telefone)
                .HasColumnName("telefone")
                .HasMaxLength(20);

            builder.Property(e => e.Usuario)
                .IsRequired()
                .HasColumnName("usuario")
                .HasMaxLength(100);

            builder.Property(e => e.Tipo).HasColumnName("tipo");

        }

        public static void RegistrarPessoaMapping<T>(this EntityTypeBuilder<T> builder) where T : PessoaBase
        {

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
               .HasColumnName("id")
               .HasDefaultValueSql("nextval('pessoas_id_seq'::regclass)");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasColumnName("email")
                .HasMaxLength(100);

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasColumnName("nome")
                .HasMaxLength(100);

            builder.Property(e => e.Senha)
                .IsRequired()
                .HasColumnName("senha")
                .HasMaxLength(100);

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("1");

            builder.Property(e => e.Telefone)
                .HasColumnName("telefone")
                .HasMaxLength(20);

            builder.Property(e => e.Usuario)
                .IsRequired()
                .HasColumnName("usuario")
                .HasMaxLength(100);

            builder.Property(e => e.Tipo).HasColumnName("tipo");

        }
    }

}
