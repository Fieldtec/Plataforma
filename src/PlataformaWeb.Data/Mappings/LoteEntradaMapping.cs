using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class LoteEntradaMapping : IEntityTypeConfiguration<LoteEntrada>
    {
        public void Configure(EntityTypeBuilder<LoteEntrada> builder)
        {
            builder.ToTable("loteentrada");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");

            builder.Property(e => e.DataEntrada)
                .HasColumnName("dataentrada")
                .HasColumnType("date");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.IdLocal).HasColumnName("idlocal");
            builder.Property(e => e.IdPlanejamento).HasColumnName("idplanejamento");
            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");
            builder.Property(e => e.IdCliente).HasColumnName("idcliente");
            builder.Property(e => e.IdFasePlanejamento).HasColumnName("idfaseplanejamento");
            builder.Property(e => e.IdRacaoAtual).HasColumnName("idracaoatual");
            builder.Property(e => e.PesoMedioProjetado).HasColumnName("pesomedioproj");
            builder.Property(e => e.DiaAtual).HasColumnName("diaatual");

            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");
            
            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("1");

            builder.HasOne(d => d.Cliente)
                .WithMany(p => p.LotesDeEntrada)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("loteentradacliente");

            builder.HasOne(d => d.Local)
                .WithMany(p => p.LotesEntrada)
                .HasForeignKey(d => d.IdLocal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("loteEntradaLocal");

            builder.HasOne(d => d.Planejamento)
                .WithMany(p => p.LotesEntrada)
                .HasForeignKey(d => d.IdPlanejamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("loteentradaplanejamento");


            //builder.HasOne(d => d.FasePlanejamento)
            //    .WithMany()
            //    .HasForeignKey(d => d.IdFasePlanejamento)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("loteentradafase");

            builder.HasOne(d => d.RacaoAtual)
               .WithMany()
               .HasForeignKey(d => d.IdRacaoAtual)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("loteentradaracao");


        }
    }
}
