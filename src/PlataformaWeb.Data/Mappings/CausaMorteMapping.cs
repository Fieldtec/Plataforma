﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class CausaMorteMapping : IEntityTypeConfiguration<CausaMorte>
    {
        public void Configure(EntityTypeBuilder<CausaMorte> builder)
        {
            builder.ToTable("auxcausamorte");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

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

            builder.HasOne(d => d.Cliente)
                .WithMany(p => p.CausaMortes)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auxcausamortecliente");
        }
    }
}
