using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;

namespace PlataformaWeb.Data.Mappings
{
    public class FornecimentoConfinamentoMapping : IEntityTypeConfiguration<FornecimentoConfinamento>
    {
        public void Configure(EntityTypeBuilder<FornecimentoConfinamento> builder)
        {
            builder.ToTable("fornecimentoconfinamento");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id).HasColumnName("id")
                   .HasDefaultValueSql("nextval('fornecimentoconfinamento_id_seq'::regclass)");

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");
            builder.Property(e => e.DataFornecimento).HasColumnName("datafornecimento");
            builder.Property(e => e.DataRegistro).HasColumnName("dataregistro").HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.IdCliente).HasColumnName("idcliente");
            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");
            builder.Property(e => e.IdRacao).HasColumnName("idracao");
            builder.Property(e => e.IdCurral).HasColumnName("idcurral");
            builder.Property(e => e.IdLote).HasColumnName("idlote");

            builder.Property(e => e.QuantidadeAnimais).HasColumnName("qtdeanimais");
            builder.Property(e => e.KgPrevisto).HasColumnName("kgprevisto");
            builder.Property(e => e.KgRealizado).HasColumnName("kgrealizado");
            builder.Property(e => e.MateriaSecaRacao).HasColumnName("msracao");
            builder.Property(e => e.Ajuste).HasColumnName("ajuste");

            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");

            builder.Property(e => e.Status).HasColumnName("status").HasDefaultValueSql("1");

            builder.HasOne(d => d.Lote)
                .WithMany(l => l.FornecimentosConfinamento)
                .HasForeignKey(l => l.IdLote);

            builder.HasOne(d => d.Cliente)
                    .WithMany(c => c.FornecimentosConfinamento)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("forncliente");

            builder.HasOne(x => x.Curral)
                .WithMany(x => x.FornecimentosConfinamento)
                .HasForeignKey(x => x.IdCurral)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fonrcurral");

            builder.HasOne(x => x.Racao)
                .WithMany(x => x.FornecimentosConfinamento)
                .HasForeignKey(x => x.IdRacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fornracao");
        }
    }


}
