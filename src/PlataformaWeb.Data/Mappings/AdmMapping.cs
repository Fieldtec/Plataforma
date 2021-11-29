using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class AdmMapping : IEntityTypeConfiguration<Adm>
    {
        public void Configure(EntityTypeBuilder<Adm> builder)
        {

            //builder.HasNoKey();

            builder.ToTable("adm");

            //builder.OwnsOne(c => c.Pessoa, e =>
            //{
            //    e.RegistrarPessoaMapping();

            //    e.Property(x => x.Tipo)
            //        .HasDefaultValueSql("1");
            //});

            builder.RegistrarPessoaMapping();

            builder.Property(x => x.Tipo)
                    .HasDefaultValueSql("1");

            //builder.Property(e => e.Id)
            //    .HasColumnName("id")
            //    .HasDefaultValueSql("nextval('pessoas_id_seq'::regclass)");            




        }
    }
}
