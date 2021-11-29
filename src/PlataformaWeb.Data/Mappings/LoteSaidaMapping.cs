using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;

namespace PlataformaWeb.Data.Mappings
{
    public class LoteSaidaMapping : IEntityTypeConfiguration<LoteSaida>
    {
        public void Configure(EntityTypeBuilder<LoteSaida> builder)
        {
            builder.ToTable("lotesaida");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");

            builder.Property(e => e.DataEmbarque)
                .HasColumnName("dataembarque")
                .HasColumnType("date");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.IdProdutorDestino).HasColumnName("idprodutordestino");
            builder.Property(e => e.IdFrigorificoDestino).HasColumnName("idfrigorificodestino");
            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");
            builder.Property(e => e.IdCliente).HasColumnName("idcliente");

            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");

            builder.Property(e => e.TipoSaida).HasColumnName("tiposaida");
            builder.Property(e => e.QuantidadeAnimaEmbarcado).HasColumnName("qtdanimalembarcado");
            builder.Property(e => e.QuantidadeAnimalPrevisto).HasColumnName("qtdanimalprevista");

            builder.Property(e => e.NumeroLote).HasColumnName("numerolote");

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("1");

            builder.HasOne(d => d.Cliente)
                .WithMany(p => p.LotesDeSaida)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lotesaidacliente");

            builder.HasOne(d => d.FrigorificoDestino)
                .WithMany(p => p.LotesSaida)
                .HasForeignKey(d => d.IdFrigorificoDestino)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lotesaidafrigorifico");

            builder.HasOne(d => d.ProdutorDestino)
                .WithMany(p => p.LotesSaida)
                .HasForeignKey(d => d.IdProdutorDestino)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lotesaidaprodutorparceiro");


        }
    }

}
