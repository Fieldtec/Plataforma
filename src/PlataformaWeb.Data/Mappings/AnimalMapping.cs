using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaWeb.Data.Mappings
{
    public class AnimalMapping : IEntityTypeConfiguration<Animal>
    {
        public void Configure(EntityTypeBuilder<Animal> builder)
        {
            builder.ToTable("animal");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
               .HasColumnName("id")
               .HasDefaultValueSql("nextval('animal_id_seq'::regclass)");

            builder.Property(e => e.DataAlteracao).HasColumnName("dataalteracao");
            builder.Property(e => e.TipoEntrada).HasColumnName("tipoentrada");

            builder.Property(e => e.ValorCompra)
                .HasColumnName("valorcompra")
                .HasDefaultValue("0");

            builder.Property(e => e.IdadeEntrada).HasColumnName("idadeentrada");                

            builder.Property(e => e.PesoEntrada)
                .HasColumnName("pesoentrada")
                .HasColumnType("numeric(15,2)");

            builder.Property(e => e.DataEntrada)
                .HasColumnName("dataentrada")
                .HasColumnType("date");

            builder.Property(e => e.DataRegistro)
                .HasColumnName("dataregistro")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.IdCategoria).HasColumnName("idcategoria");
            builder.Property(e => e.IdRaca).HasColumnName("idraca");
            builder.Property(e => e.IdLote).HasColumnName("idlote");
            builder.Property(e => e.IdProdutorOrigem).HasColumnName("idprodutororigem");
            builder.Property(e => e.IdUsuario).HasColumnName("idusuario");
            builder.Property(e => e.IdCausaMorte).HasColumnName("idcausamorte");
            builder.Property(e => e.DataMorte).HasColumnName("datamorte");
            builder.Property(e => e.IdUsuarioAlteracao).HasColumnName("idusuarioalteracao");
            builder.Property(e => e.IdLoteSaida).HasColumnName("idlotesaida");

            builder.Property(e => e.DataSaida).HasColumnName("datasaida").HasColumnType("date");
            builder.Property(e => e.PesoSaida).HasColumnName("pesosaida").HasColumnType("numeric(15,2)");
            builder.Property(e => e.PesoProjetado).HasColumnName("pesoprojetado").HasColumnType("numeric(15,2)");

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("1");

            builder.HasOne(d => d.Categoria)
                .WithMany(p => p.Animais)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AnimalCategoria");

            builder.HasOne(d => d.Raca)
                .WithMany(p => p.Animais)
                .HasForeignKey(d => d.IdRaca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AnimalRaca");

            builder.HasOne(d => d.LoteEntrada)
                .WithMany(p => p.AnimaisLote)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AnimalloteEntrada");

            builder.HasOne(d => d.ProdutorPaceiro)
                .WithMany(p => p.Animais)
                .HasForeignKey(d => d.IdProdutorOrigem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produtorparceiro");

            builder.HasOne(d => d.CausaMorte)
                .WithMany(p => p.Animais)
                .HasForeignKey(x => x.IdCausaMorte)
                .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("animalcausamorte");

            builder.HasOne(d => d.LoteSaida)
                .WithMany(p => p.Animais)
                .HasForeignKey(x => x.IdLoteSaida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("animallotesaida");

        }
    }
}
