using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class LeituraCochoMapping : IEntityTypeConfiguration<LeituraCocho>
    {
        public void Configure(EntityTypeBuilder<LeituraCocho> builder)
        {
            builder.ToTable("leituracocho");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id).HasColumnName("id")
                   .HasDefaultValueSql("nextval('leituracocho_id_seq'::regclass)");

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");
            builder.Property(e => e.DataLeitura).HasColumnName("dataleitura");
            builder.Property(e => e.DataRegistro).HasColumnName("dataregistro").HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.IdCliente).HasColumnName("idcliente");
            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");
            builder.Property(e => e.IdLocal).HasColumnName("idlocal");
            builder.Property(e => e.IdLote).HasColumnName("idlote");
            builder.Property(e => e.IdPlanejamento).HasColumnName("idplanejamento");

            builder.Property(e => e.QuantidadeAnimais).HasColumnName("qtdanimais");
            builder.Property(e => e.AjusteGramas).HasColumnName("ajustegramas");

            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");

            builder.Property(e => e.Status).HasColumnName("status").HasDefaultValueSql("1");

            builder.HasOne(d => d.Cliente)
                    .WithMany(c => c.LeiturasCocho)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("loteentradacliente");

            builder.HasOne(x => x.Local)
                .WithMany(x => x.LeiturasCocho)
                .HasForeignKey(x => x.IdLocal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("loteEntradaLocal");

            builder.HasOne(x => x.LoteEntrada)
                .WithMany(x => x.LeiturasCocho)
                .HasForeignKey(x => x.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("loteentradaatual");

            builder.HasOne(x => x.Planejamento)
                .WithMany(x => x.LeiturasCocho)
                .HasForeignKey(x => x.IdPlanejamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("loteentradaplanejamento");

        }
    }
}
