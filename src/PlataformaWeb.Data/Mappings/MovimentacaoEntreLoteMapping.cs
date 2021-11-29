using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class MovimentacaoEntreLoteMapping : IEntityTypeConfiguration<MovimentacaoEntreLote>
    {
        public void Configure(EntityTypeBuilder<MovimentacaoEntreLote> builder)
        {
            builder.ToTable("movimentacaoentrelotes");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");
            builder.Property(e => e.DataMovimentacao).HasColumnName("datamov").HasColumnType("date");
            builder.Property(e => e.DataRegistro).HasColumnName("dataregistro").HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.IdLoteEntrada).HasColumnName("idloteentrada");
            builder.Property(e => e.IdLocalOrigem).HasColumnName("idlocalorigem");
            builder.Property(e => e.IdLocalDestino).HasColumnName("idlocaldestino");
            builder.Property(e => e.IdMotivo).HasColumnName("idmotivo");
            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");
            builder.Property(e => e.IdCliente).HasColumnName("idcliente");
            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");

            builder.Property(e => e.QuantidadeAnimais).HasColumnName("qtdanimais");

            builder.Property(e => e.Status).HasColumnName("status").HasDefaultValueSql("1");

            builder.HasOne(d => d.Cliente)
                .WithMany(p => p.MovimentacoesEntreLote)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MovimentacaoEntreLotesCliente");

            builder.HasOne(d => d.LocalDestino)
                .WithMany(p => p.MovimentacoesEntreLoteDestino)
                .HasForeignKey(d => d.IdLocalDestino)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MovimentacaoEntreLotesLocalDestino");

            builder.HasOne(d => d.LocalOrigem)
                .WithMany(p => p.MovimentacoesEntreLoteOrigem)
                .HasForeignKey(d => d.IdLocalOrigem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MovimentacaoEntreLotesLocalOrigem");

            builder.HasOne(d => d.LoteEntrada)
                .WithMany(p => p.MovimentacoesEntreLote)
                .HasForeignKey(d => d.IdLoteEntrada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MovimentacaoEntreLotesLote");

            builder.HasOne(d => d.Motivo)
                .WithMany(p => p.MovimentacoesEntreLote)
                .HasForeignKey(d => d.IdMotivo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MovimentacaoEntreLotesMotivo");
        }
    }
}
