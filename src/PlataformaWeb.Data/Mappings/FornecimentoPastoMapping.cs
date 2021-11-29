using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;

namespace PlataformaWeb.Data.Mappings
{
    public class FornecimentoPastoMapping : IEntityTypeConfiguration<FornecimentoPasto>
    {
        public void Configure(EntityTypeBuilder<FornecimentoPasto> builder)
        {
            builder.ToTable("fornecimentopasto");

            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");
            builder.Property(e => e.DataRealizado).HasColumnName("datarealizado");
            builder.Property(e => e.DataRegistro).HasColumnName("dataregistro").HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(e => e.IdPasto).HasColumnName("idpasto");
            builder.Property(e => e.QuantidadeAnimais).HasColumnName("qtdanimais");
            builder.Property(e => e.IdLote).HasColumnName("idlote");
            builder.Property(e => e.IdSuplemento).HasColumnName("idsuplemento");
            builder.Property(e => e.IdCliente).HasColumnName("idcliente");
            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");
            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");
            builder.Property(e => e.PrevisaoKg).HasColumnName("previsaokg");
            builder.Property(e => e.PrevisaoSaco).HasColumnName("previsaosaco");
            builder.Property(e => e.RealizadoKg).HasColumnName("realizadokg");
            builder.Property(e => e.RealizadoSaco).HasColumnName("realizadosaco");
            builder.Property(e => e.Origem).HasColumnName("origem");
            builder.Property(e => e.Destino).HasColumnName("destino");
            builder.Property(e => e.Status).HasColumnName("status").HasDefaultValueSql("1");

            builder.HasOne(d => d.Cliente)
                .WithMany(p => p.FornecimentosPasto)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fornecimentopasto_cliente");

            builder.HasOne(d => d.Lote)
                .WithMany(d => d.FornecimentosPasto)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fornecimentopasto_lote");

            builder.HasOne(d => d.Pasto)
                .WithMany(d => d.FornecimentosPasto)
                .HasForeignKey(d => d.IdPasto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fornecimentopasto_pasto");

            builder.HasOne(d => d.Suplemento)
             .WithMany(d => d.FornecimentosPasto)
             .HasForeignKey(d => d.IdSuplemento)
             .OnDelete(DeleteBehavior.ClientSetNull)
             .HasConstraintName("fornecimentopasto_suple");

        }
    }
}
