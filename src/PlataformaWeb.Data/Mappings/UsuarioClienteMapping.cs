using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class UsuarioClienteMapping : IEntityTypeConfiguration<UsuarioCliente>
    {
        public void Configure(EntityTypeBuilder<UsuarioCliente> builder)
        {
            //builder.HasNoKey();

            builder.ToTable("usuariocliente");

            builder.HasIndex(e => e.IdCliente)
                   .HasName("fki_usuariocliente");

            //builder.Property(e => e.Id)
            //    .HasColumnName("id")
            //    .HasDefaultValueSql("nextval('pessoas_id_seq'::regclass)");

            builder.Property(e => e.IdCliente).HasColumnName("idcliente");

            builder.RegistrarPessoaMapping();

            builder.Property(e => e.Tipo)
                .HasDefaultValueSql("3");

            builder.HasOne(d => d.Cliente)
                   .WithMany(p => p.Usuariocliente)
                   .HasForeignKey(d => d.IdCliente)
                   .HasConstraintName("usuariocliente");
        }
    }
}
