using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class LogNotaLeituraCochoMapping : IEntityTypeConfiguration<LogNotaLeituraCocho>
    {
        public void Configure(EntityTypeBuilder<LogNotaLeituraCocho> builder)
        {
            builder.ToTable("lognotasleituracocho");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id).HasColumnName("id")
                   .HasDefaultValueSql("nextval('lognotasleituracocho_id_seq'::regclass)");

            builder.Property(e => e.IdNota).HasColumnName("idnota");
            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");

            builder.Property(e => e.AjustePorcentagem).HasColumnName("ajusteporcentagem");
            builder.Property(e => e.Nome).HasColumnName("nome");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.IdCliente).HasColumnName("idcliente");

            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");

        }
    }

    public class NotaLeituraCochoMapping : IEntityTypeConfiguration<NotaLeituraCocho>
    {
        public void Configure(EntityTypeBuilder<NotaLeituraCocho> builder)
        {
            builder.ToTable("notasleituracocho");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id).HasColumnName("id")
                   .HasDefaultValueSql("nextval('notasleituracocho_id_seq'::regclass)");

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");            

            builder.Property(e => e.IdCliente).HasColumnName("idcliente");

            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");

            builder.Property(e => e.Nome).HasColumnName("nome");
            builder.Property(e => e.AjustePorcentagem).HasColumnName("ajusteporcentagem");

            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");

            builder.Property(e => e.Status).HasColumnName("status").HasDefaultValueSql("1");

            builder.HasOne(d => d.Cliente)
                    .WithMany(c => c.NotasLeituraCocho)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("NotasLeituraCochoCliente");

        }
    }
}
