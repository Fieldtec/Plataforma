using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;

namespace PlataformaWeb.Data.Mappings
{
    public class MovimentacaoAnimalMapping : IEntityTypeConfiguration<MovimentacaoAnimal>
    {
        public void Configure(EntityTypeBuilder<MovimentacaoAnimal> builder)
        {
            builder.ToTable("movimentacaoanimal");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");
            builder.Property(e => e.DataMovimentacao).HasColumnName("datamov").HasColumnType("date");
            builder.Property(e => e.DataRegistro).HasColumnName("dataregistro").HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.IdLoteDestino).HasColumnName("idlotedestino");
            builder.Property(e => e.IdLoteOrigem).HasColumnName("idloteorigem");
            builder.Property(e => e.IdLocalOrigem).HasColumnName("idlocalorigem");
            builder.Property(e => e.IdLocalDestino).HasColumnName("idlocaldestino");
            builder.Property(e => e.IdMotivo).HasColumnName("idmotivo");
            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");
            builder.Property(e => e.IdCliente).HasColumnName("idcliente");
            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");
            builder.Property(e => e.IdAnimal).HasColumnName("idanimal");

            builder.Property(e => e.Status).HasColumnName("status").HasDefaultValueSql("1");

            builder.HasOne(d => d.Cliente)
                .WithMany(p => p.MovimentacoesAnimal)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movimentacaoanimalcliente");

            builder.HasOne(d => d.LocalDestino)
                .WithMany(p => p.MovimentacoesAnimalDestino)
                .HasForeignKey(d => d.IdLocalDestino)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movimentacaoanimallocaldestino");

            builder.HasOne(d => d.LocalOrigem)
                .WithMany(p => p.MovimentacoesAnimalOrigem)
                .HasForeignKey(d => d.IdLocalOrigem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movimentacaoanimallocalorigem");

            builder.HasOne(d => d.LoteDestino)
                .WithMany(p => p.MovimentacoesAnimalDestino)
                .HasForeignKey(d => d.IdLoteDestino)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movimentacaoanimallotedestino");

            builder.HasOne(d => d.LoteOrigem)
                .WithMany(p => p.MovimentacoesAnimalOrigem)
                .HasForeignKey(d => d.IdLoteOrigem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movimentacaoanimalloteorigem");

            builder.HasOne(d => d.Motivo)
                .WithMany(p => p.MovimentacoesAnimal)
                .HasForeignKey(d => d.IdMotivo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movimentacaoanimalmotivo");

            builder.HasOne(d => d.Animal)
                .WithMany(p => p.Movimentacoes)
                .HasForeignKey(d => d.IdAnimal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movimentacaoanimalid");
        }
    }


}
