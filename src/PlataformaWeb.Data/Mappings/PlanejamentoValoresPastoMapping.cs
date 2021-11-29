using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;

namespace PlataformaWeb.Data.Mappings
{
    public class PlanejamentoValoresPastoMapping : IEntityTypeConfiguration<PlanejamentoValoresPasto>
    {
        public void Configure(EntityTypeBuilder<PlanejamentoValoresPasto> builder)
        {
            builder.ToTable("planejamentovalorespasto");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.GmdEsperado)
                .HasColumnName("gmdesperado")
                .HasColumnType("numeric(18,3)");

            builder.Property(e => e.IdCategoria).HasColumnName("idcategoria");

            builder.Property(e => e.IdFase).HasColumnName("idfase");

            builder.Property(e => e.IdPlanejamento).HasColumnName("idplanejamento");

            builder.Property(e => e.IdSuplemento).HasColumnName("idsuplemento");

            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");

            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");

            builder.Property(e => e.ImspvEsperado)
                .HasColumnName("imspvesperado")
                .HasColumnType("numeric(18,3)");

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("1");

            builder.HasOne(d => d.Categoria)
                .WithMany(p => p.PlanejamentoValoresPasto)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlanejamentoValoresPastoIdCategoria");

            builder.HasOne(d => d.FaseDoAno)
                .WithMany(p => p.PlanejamentoValoresPasto)
                .HasForeignKey(d => d.IdFase)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlanejamentoValoresPastoIdFase");

            builder.HasOne(d => d.PlanejamentoNutricional)
                .WithMany(p => p.PlanejamentoValoresPasto)
                .HasForeignKey(d => d.IdPlanejamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlanejamentoValoresPastoIdPlan");

            builder.HasOne(d => d.SuplementoMineral)
                .WithMany(p => p.PlanejamentoValoresPasto)
                .HasForeignKey(d => d.IdSuplemento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlanejamentoValoresPastoIdSuplemento");
        }
    }

}
