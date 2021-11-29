using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class TecnicoMapping : IEntityTypeConfiguration<Tecnico>
    {
        public void Configure(EntityTypeBuilder<Tecnico> builder)
        {
            //builder.HasNoKey();

            builder.ToTable("tecnico");

            builder.HasIndex(e => e.IdAdm)
                   .HasName("fki_TecnicoAdm");

            builder.Property(e => e.DataAquisicao)
                .HasColumnName("dataaquisicao")
                .HasColumnType("date");

           
            builder.Property(e => e.DataUltimaAlteracao).HasColumnName("dataultimaalteracao");

            //builder.Property(e => e.Id)
            //    .HasColumnName("id")
            //    .HasDefaultValueSql("nextval('pessoas_id_seq'::regclass)");


            builder.Property(e => e.IdAdm).HasColumnName("idadm");

            builder.Property(e => e.IdAdmAlteracao).HasColumnName("idadmalteracao");

            builder.Property(e => e.QtdeLicenca).HasColumnName("qtdelicenca");
              
            builder.RegistrarPessoaMapping();

            builder.Property(e => e.Tipo)
                .HasDefaultValueSql("2");

            builder.HasOne(d => d.Adm)
                   .WithMany(p => p.Tecnico)
                   .HasForeignKey(d => d.IdAdm)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("TecnicoAdm");

        }
    }
}
