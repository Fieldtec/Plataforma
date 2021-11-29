using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class RacaoInsumoMapping : IEntityTypeConfiguration<RacaoInsumo>
    {
        public void Configure(EntityTypeBuilder<RacaoInsumo> entity)
        {
            entity.ToTable("racaoinsumos");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");

            entity.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.IdCliente).HasColumnName("idcliente");

            entity.Property(e => e.IdInsumoAlimento).HasColumnName("idinsumoalimento");

            entity.Property(e => e.IdRacao).HasColumnName("idracao");

            entity.Property(e => e.IdUsuario).HasColumnName("idusuario");

            entity.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");

            entity.Property(e => e.InclusaoMateriaNatural)
                .HasColumnName("inclusaomaterianatural")
                .HasColumnType("numeric(15,3)");

            entity.Property(e => e.InclusaoMateriaSeca)
                .HasColumnName("inclusaomateriaseca")
                .HasColumnType("numeric(15,3)");

            entity.Property(e => e.KgMateriaSeca)
                .HasColumnName("kgmateriaseca")
                .HasColumnType("numeric(15,3)");

            entity.Property(e => e.KgMateriaNatural)
               .HasColumnName("kgmaterianatural")
               .HasColumnType("numeric(15,3)");

            entity.Property(e => e.PercentualMateriaSeca)
                .HasColumnName("percentmateriaseca")
                .HasColumnType("numeric(15,3)");

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("1");

            entity.Property(e => e.ValorInclusao)
                .HasColumnName("valorinclusao")
                .HasColumnType("numeric(15,3)");

            entity.Property(e => e.ValorKg)
                .HasColumnName("valorkg")
                .HasColumnType("numeric(15,3)");

            entity.HasOne(d => d.Cliente)
                .WithMany(p => p.InsumosRacoes)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("racaoinsumos_fk");

            entity.HasOne(d => d.InsumoAlimento)
                .WithMany(p => p.InsumosRacoes)
                .HasForeignKey(d => d.IdInsumoAlimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("racaoinsumos_fk_2");

            entity.HasOne(d => d.Racao)
                .WithMany(p => p.InsumosRacao)
                .HasForeignKey(d => d.IdRacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("racaoinsumos_fk_1");
        }
    }
}
