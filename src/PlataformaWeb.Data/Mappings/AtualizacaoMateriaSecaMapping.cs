using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class AtualizacaoMateriaSecaMapping : IEntityTypeConfiguration<AtualizacaoMateriaSeca>
    {
        public void Configure(EntityTypeBuilder<AtualizacaoMateriaSeca> builder)
        {
            builder.ToTable("atualizacaoms");

            builder.Property(e => e.Id).HasColumnName("id")
                   .HasDefaultValueSql("nextval('atualizacaoms_id_seq'::regclass)");

            builder.Property(e => e.DataAtualizacao).HasColumnName("dataatualizacao");
            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");
            builder.Property(e => e.IdUsuario).HasColumnName("idinsumo");
            builder.Property(e => e.MateriaSecaAnterior).HasColumnName("msanterior").HasColumnType("numeric(15,3)");
            builder.Property(e => e.MateriaSecaAtual).HasColumnName("msatual").HasColumnType("numeric(15,3)");

            builder.HasOne(d => d.Insumo)
                    .WithMany(c => c.AtualizacoesMateriaSeca)
                    .HasForeignKey(d => d.IdInsumo)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("atualizacaoms_fk");
        }
    }
}
