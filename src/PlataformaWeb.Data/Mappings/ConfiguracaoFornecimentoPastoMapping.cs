using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;

namespace PlataformaWeb.Data.Mappings
{
    public class ConfiguracaoFornecimentoPastoMapping : IEntityTypeConfiguration<ConfiguracaoFornecimentoPasto>
    {
        public void Configure(EntityTypeBuilder<ConfiguracaoFornecimentoPasto> builder)
        {
            builder.ToTable("configuracaofornecimentopasto");

            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");
            builder.Property(e => e.Segunda).HasColumnName("segunda").HasDefaultValueSql("0");
            builder.Property(e => e.Terca).HasColumnName("terca").HasDefaultValueSql("0");
            builder.Property(e => e.Quarta).HasColumnName("quarta").HasDefaultValueSql("0");
            builder.Property(e => e.Quinta).HasColumnName("quinta").HasDefaultValueSql("0");
            builder.Property(e => e.Sexta).HasColumnName("sexta").HasDefaultValueSql("0");
            builder.Property(e => e.Sabado).HasColumnName("sabado").HasDefaultValueSql("0");
            builder.Property(e => e.Domingo).HasColumnName("domingo").HasDefaultValueSql("0");            
            builder.Property(e => e.DataRegistro).HasColumnName("dataregistro").HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(e => e.IdCliente).HasColumnName("idcliente");
            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");
            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");           
            builder.Property(e => e.Status).HasColumnName("status").HasDefaultValueSql("1");

            builder.HasOne(d => d.Cliente)
                .WithMany(p => p.ConfiguracaoFornecimentoPasto)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fornecimentopasto_cliente");
        }
    }
}
