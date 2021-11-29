using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class ClienteMapping : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {

            builder.ToTable("cliente");

            builder.HasIndex(e => e.IdTecnico)
                    .HasName("fki_ClienteTecnico");

            builder.Property(e => e.AreaHectare)
                .HasColumnName("areahectare")
                .HasColumnType("numeric(15,2)");

            builder.Property(e => e.Cidade)
                .IsRequired()
                .HasColumnName("cidade")
                .HasMaxLength(100);

            builder.Property(e => e.CpfCnpj)
                .IsRequired()
                .HasColumnName("cpf_cnpj")
                .HasMaxLength(20);

            builder.Property(e => e.DataValidadeLicenca)
                .HasColumnName("datavalidadelicenca")
                .HasColumnType("date");


            //builder.Property(e => e.Id)
            //    .HasColumnName("id")
            //    .HasDefaultValueSql("nextval('pessoas_id_seq'::regclass)");

            builder.Property(e => e.IdTecnico).HasColumnName("idtecnico");

            builder.Property(e => e.NomePropriedade)
                .IsRequired()
                .HasColumnName("nomepropriedade")
                .HasMaxLength(100);

            builder.Property(e => e.QtdeAnimais).HasColumnName("qtdeanimais");

            builder.Property(e => e.Uf)
                .IsRequired()
                .HasColumnName("uf")
                .HasMaxLength(2)
                .IsFixedLength();

            builder.RegistrarPessoaMapping();

            builder.Property(e => e.Tipo)
                .HasDefaultValueSql("3");


            builder.HasOne(d => d.Tecnico)
                   .WithMany(p => p.Cliente)
                   .HasForeignKey(d => d.IdTecnico)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("ClienteTecnico");




        }
    }
}
