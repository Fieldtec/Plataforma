using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;

namespace PlataformaWeb.Data.Mappings
{
    public class PrevisaoFornecimentoPastoMapping : IEntityTypeConfiguration<PrevisaoFornecimentoPasto>
    {
        public void Configure(EntityTypeBuilder<PrevisaoFornecimentoPasto> builder)
        {
            builder.ToTable("previsaofornecimentopasto");

            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");
            builder.Property(e => e.DataPrevisao).HasColumnName("dataprev");
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
            builder.Property(e => e.Status).HasColumnName("status").HasDefaultValueSql("1");

            builder.HasOne(d => d.Cliente)
                .WithMany(p => p.PrevisoesFornecimentoPasto)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("previsaofornecimentopasto_cliente");

            builder.HasOne(d => d.Lote)
                .WithMany(d => d.PrevisaoFornecimentosPasto)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("previsaofornecimentopasto_lote");

            builder.HasOne(d => d.Pasto)
                .WithMany(d => d.PrevisoesFornecimentoPasto)
                .HasForeignKey(d => d.IdPasto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("previsaofornecimentopasto_pasto");

            builder.HasOne(d => d.Suplemento)
             .WithMany(d => d.PrevisaoFornecimentosPasto)
             .HasForeignKey(d => d.IdSuplemento)
             .OnDelete(DeleteBehavior.ClientSetNull)
             .HasConstraintName("previsaofornecimentopasto_suple");

        }
    }
}
