using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;

namespace PlataformaWeb.Data.Mappings
{
    public class PlanejamentoNutricionalMapping : IEntityTypeConfiguration<PlanejamentoNutricional>
    {
        public void Configure(EntityTypeBuilder<PlanejamentoNutricional> builder)
        {
            builder.ToTable("planejamentonutricional");

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

            builder.Property(e => e.Tipo).HasColumnName("tipo");

            builder.HasOne(d => d.Cliente)
                .WithMany(p => p.PlanejamentosNutricionais)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlanejamentoNutricional");
        }
    }

}
