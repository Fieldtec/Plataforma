using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Data.Context.Extensions;

namespace PlataformaWeb.Data
{
    public partial class PlataformaFieldContext : DbContext, IUnitOfWork
    {
        private readonly IUser _appUser;

        public PlataformaFieldContext(DbContextOptions<PlataformaFieldContext> options, IUser appUser)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
            _appUser = appUser;
        }

        public virtual DbSet<Adm> Adm { get; set; }
        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<FaseDoAno> FaseDoAno { get; set; }
        public virtual DbSet<FornecedorInsumo> FornecedorInsumo { get; set; }
        public virtual DbSet<InsumoAlimento> InsumoAlimento { get; set; }
        public virtual DbSet<PastoCurral> Pastocurral { get; set; }
        public virtual DbSet<Pessoa> Pessoas { get; set; }
        public virtual DbSet<PlanejamentoNutricional> PlanejamentosNutricionais { get; set; }
        public virtual DbSet<PlanejamentoValoresPasto> PlanejamentosValoresPasto { get; set; }
        public virtual DbSet<PlanejamentoValoresConfinamento> PlanejamentosValoresConfinamento { get; set; }
        public virtual DbSet<ProdutorParceiro> ProdutorParceiro { get; set; }
        public virtual DbSet<PropriedadeParceira> PropriedadeParceira { get; set; }
        public virtual DbSet<Raca> Raca { get; set; }
        public virtual DbSet<Racao> Racao { get; set; }
        public virtual DbSet<RacaoInsumo> RacaoInsumo { get; set; }
        public virtual DbSet<SuplementoMineral> SuplementoMineral { get; set; }
        public virtual DbSet<Tecnico> Tecnico { get; set; }
        public virtual DbSet<UsuarioCliente> UsuarioCliente { get; set; }
        public virtual DbSet<Animal> Animais { get; set; }
        public virtual DbSet<LoteEntrada> LotesEntradas { get; set; }
        public virtual DbSet<Frigorifico> Frigorificos { get; set; }
        public virtual DbSet<MotivoMovimentacao> MotivoMovimentacoes { get; set; }
        public virtual DbSet<CausaMorte> CausaMorte { get; set; }
        public virtual DbSet<MovimentacaoEntreLote> MovimentacoesEntreLote { get; set; }
        public virtual DbSet<NotaLeituraCocho> NotasLeiturasCocho { get; set; }
        public virtual DbSet<LogNotaLeituraCocho> LogsNotasLeiturasCocho { get; set; }
        public virtual DbSet<LeituraCocho> LeiturasCocho { get; set; }
        public virtual DbSet<FornecimentoConfinamento> FornecimentosConfinamento { get; set; }
        public virtual DbSet<ConfiguracaoFornecimentoPasto> ConfiguracaoFornecimentoPasto { get; set; }
        public virtual DbSet<PrevisaoFornecimentoPasto> PrevisaoFornecimentosPasto { get; set; }
        public virtual DbSet<MovimentacaoAnimal> MovimentacoesAnimal { get; set; }
        public virtual DbSet<FornecimentoPasto> FornecimentosPasto { get; set; }

        public static readonly ILoggerFactory PlataformaFieldLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(PlataformaFieldLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasPostgresExtension("adminpack");               

            modelBuilder.Ignore<PessoaBase>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlataformaFieldContext).Assembly);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public async Task<bool> Commit()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(entry => entry.Entity.GetType().GetProperty("DataRegistro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataRegistro").CurrentValue = DateTime.Now;
                    entry.Property("Status").CurrentValue = Status.Ativado;

                    if (entry.Entity.GetType().GetProperty("IdUsuario") != null)
                    {
                        entry.Property("IdUsuario").CurrentValue = _appUser.ObterId();
                    }

                    if (entry.Entity.GetType().GetProperty("IdUsuarioAlteracao") != null)
                    {
                        entry.Property("IdUsuarioAlteracao").IsModified = false;
                    }

                    if (entry.Entity.GetType().GetProperty("DataAlteracao") != null)
                    {
                        entry.Property("DataAlteracao").IsModified = false;
                    }
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataRegistro").IsModified = false;

                    if (entry.Entity.GetType().GetProperty("IdUsuario") != null)
                    {
                        entry.Property("IdUsuario").IsModified = false;
                    }

                    if (entry.Entity.GetType().GetProperty("IdUsuarioAlteracao") != null)
                    {
                        entry.Property("IdUsuarioAlteracao").CurrentValue = _appUser.ObterId();
                    }

                    if (entry.Entity.GetType().GetProperty("DataAlteracao") != null)
                    {
                        entry.Property("DataAlteracao").CurrentValue = DateTime.Now;
                    }
                }               

            }

            foreach (var entry in ChangeTracker.Entries()
                .Where(entry => entry.Entity.GetType().BaseType == typeof(PessoaBase)))
            {
                //Configurar as inserções dos campos de quem foi o Adm e as Datas de Inclusão a Alteração
                if (entry.Entity.GetType() == typeof(Tecnico))
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("IdAdm").CurrentValue = _appUser.ObterId();
                        entry.Property("IdAdmAlteracao").IsModified = false;
                        entry.Property("DataUltimaAlteracao").IsModified = false;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        entry.Property("IdAdm").IsModified = false;
                        entry.Property("IdAdmAlteracao").CurrentValue = _appUser.ObterId();
                        entry.Property("DataUltimaAlteracao").CurrentValue = DateTime.Now;
                    }
                }

                //Configurar para ignorar a Senha caso for uma Edição e se este campo for Vazio
                if (entry.State == EntityState.Modified && entry.Property("Senha").CurrentValue == null)
                {
                    entry.Property("Senha").IsModified = false;
                }

            }

            //Os campos abaixo das tabelas loteentrada e animal estão sendo ignorados pois existe uma rotina que o Thiago
            //irá rodar que irá preencher essas informações. A aplicação apenas deverá ler essas informações.
            //foreach (var entry in ChangeTracker.Entries()
            //    .Where(entry => entry.Entity.GetType().BaseType == typeof(LoteEntrada)))
            //{
            //    entry.Property("IdRacaoAtual").IsModified = false;
            //    entry.Property("PesoMedioProjetado").IsModified = false;
            //    entry.Property("DiaAtual").IsModified = false;
            //}

            foreach (var entry in ChangeTracker.Entries()
                .Where(entry => entry.Entity.GetType().BaseType == typeof(Animal)))
            {
                entry.Property("PesoProjetado").IsModified = false;
            }


            ChangeTracker.ApplyUpperCase();            

            return await base.SaveChangesAsync() > 0;
        }
    }
}
