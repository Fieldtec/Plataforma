using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class InsumoAlimentoMapping : IEntityTypeConfiguration<InsumoAlimento>
    {
        public void Configure(EntityTypeBuilder<InsumoAlimento> builder)
        {
            builder.ToTable("insumosalimento");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");
            builder.Property(e => e.DataAtualizacaoMateriaSeca).HasColumnName("dataatualizacaoms");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.EstoqueMinimoDias)
                .HasColumnName("estoqueminimodias")
                .HasColumnType("numeric(15,2)");

            builder.Property(e => e.EstoqueMinimoKg)
                .HasColumnName("estoqueminimokg")
                .HasColumnType("numeric(15,2)");

            //builder.Property(e => e.IdCliente).HasColumnName("idcliente");

            builder.Property(e => e.IdFornecedor).HasColumnName("idfornecedor");

            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");

            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");

            builder.Property(e => e.MateriaSeca)
                .HasColumnName("materiaseca")
                .HasColumnType("numeric(15,2)");

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasColumnName("nome")
                .HasMaxLength(100);

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("1");

            builder.Property(e => e.ValorKg)
                .HasColumnName("valorkg")
                .HasColumnType("numeric(15,2)");

            builder.HasOne(d => d.FornecedorInsumo)
                .WithMany(p => p.InsumosAlimentos)
                .HasForeignKey(d => d.IdFornecedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkInsumosAlimentoForn");
        }
    }
}
