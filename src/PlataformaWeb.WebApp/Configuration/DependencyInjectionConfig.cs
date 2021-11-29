using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Notificacoes;
using PlataformaWeb.Business.Services;
using PlataformaWeb.Data;
using PlataformaWeb.Data.Repositorio;
using PlataformaWeb.WebApp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<PlataformaFieldContext>();            

            //Configurações
            services.AddScoped<INotificador, Notificador>();            
            services.AddScoped<IUser, AppUser>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Autenticação
            services.AddScoped<IAutenticacaoService, AutenticacaoService>();
            services.AddScoped<IAutenticacaoRepositorio, AutenticacaoRepositorio>();

            //Pessoa
            services.AddScoped<IPessoaRepositorio, PessoaRepositorio>();

            //Técnico
            services.AddScoped<ITecnicoService, TecnicoService>();
            services.AddScoped<ITecnicoRepositorio, TecnicoRepositorio>();

            //Cliente
            services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            services.AddScoped<IClienteService, ClienteService>();

            //Usuario Cliente
            services.AddScoped<IUsuarioClienteRepositorio, UsuarioClienteRepositorio>();
            services.AddScoped<IUsuarioClienteService, UsuarioClienteService>();

            //Pasto/Curral
            services.AddScoped<IPastoCurralRepositorio, PastoCurralRepositorio>();
            services.AddScoped<IPastoCurralService, PastoCurralService>();

            //Categorias
            services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
            services.AddScoped<ICategoriaService, CategoriaService>();

            //Raças
            services.AddScoped<IRacaRepositorio, RacaRepositorio>();
            services.AddScoped<IRacaService, RacaService>();

            //Propriedade Parceira
            services.AddScoped<IPropriedadeParceiraRepositorio, PropriedadeParceiraRepositorio>();
            services.AddScoped<IPropriedadeParceiraService, PropriedadeParceiraService>();

            //Produtor Parceiro
            services.AddScoped<IProdutorParceiroRepositorio, ProdutorParceiroRepositorio>();
            services.AddScoped<IProdutorParceiroService, ProdutorParceiroService>();

            //Fase Do Ano
            services.AddScoped<IFaseDoAnoService, FaseDoAnoService>();
            services.AddScoped<IFaseDoAnoRepositorio, FaseDoAnoRepositorio>();

            //Fornecedor de Insumos
            services.AddScoped<IFornecedorInsumoService, FornecedorInsumoService>();
            services.AddScoped<IFornecedorInsumoRepositorio, FornecedorInsumoRepositorio>();

            //Insumo Alimentos
            services.AddScoped<IInsumoAlimentoService, InsumoAlimentoService>();
            services.AddScoped<IInsumoAlimentoRepositorio, InsumoAlimentoRepositorio>();

            //Suplemento Mineral
            services.AddScoped<ISuplementoMineralService, SuplementoMineralService>();
            services.AddScoped<ISuplementoMineralRepositorio, SuplementoMineralRepositorio>();

            //Ração & Ração Insumo
            services.AddScoped<IRacaoService, RacaoService>();
            services.AddScoped<IRacaoRepositorio, RacaoRepositorio>();

            //Planejamento Nutricional
            services.AddScoped<IPlanejamentoNutricionalRepositorio, PlanejamentoNutricionalRepositorio>();
            services.AddScoped<IPlanejamentoNutricionalService, PlanejamentoNutricionalService>();

            //Lote/Animais
            services.AddScoped<ILoteEntradaRepositorio, LoteEntradaRepositorio>();
            services.AddScoped<ILoteEntradaService, LoteEntradaService>();

            //Causa Morte
            services.AddScoped<ICausaMorteRepositorio, CausaMorteRepositorio>();
            services.AddScoped<ICausaMorteService, CausaMorteService>();

            //Motivo Movimentação
            services.AddScoped<IMotivoMovimentacaoRepositorio, MotivoMovimentacaoRepositorio>();
            services.AddScoped<IMotivoMovimentacaoService, MotivoMovimentacaoService>();

            //Frigorífico
            services.AddScoped<IFrigorificoRepositorio, FrigorificoRepositorio>();
            services.AddScoped<IFrigorificoService, FrigorificoService>();

            //Movimentação entre lotes
            services.AddScoped<IMovimentacaoEntreLoteRepositorio, MovimentacaoEntreLoteRepositorio>();
            services.AddScoped<IMovimentacaoEntreLoteService, MovimentacaoEntreLoteService>();

            //Movimentação Animal
            services.AddScoped<IMovimentacaoAnimalRepositorio, MovimentacaoAnimalRepositorio>();
            services.AddScoped<IMovimentacaoAnimalService, MovimentacaoAnimalService>();

            //Lote de Saída
            services.AddScoped<ILoteSaidaService, LoteSaidaService>();
            services.AddScoped<ILoteSaidaRepositorio, LoteSaidaRepositorio>();

            //Notas de Leitura Cocho
            services.AddScoped<INotaLeituraCochoService, NotaLeituraCochoService>();
            services.AddScoped<INotaLeituraCochoRepositorio, NotaLeituraCochoRepositorio>();

            //Leitura de Cocho
            services.AddScoped<ILeituraCochoService, LeituraCochoService>();
            services.AddScoped<ILeituraCochoRepositorio, LeituraCochoRepositorio>();

            //Fornecimento Confinamento
            services.AddScoped<IFornecimentoConfinamentoRepositorio, FornecimentoConfinamentoRepositorio>();
            services.AddScoped<IFornecimentoConfinamentoService, FornecimentoConfinamentoService>();

            //Funções Gerais
            services.AddScoped<IFuncoesRepositorio, FuncoesRepositorio>();

            //Fornecimento Pasto
            services.AddScoped<IConfiguracaoFornecimentoPastoRepositorio, ConfiguracaoFornecimentoPastoRepositorio>();
            services.AddScoped<IPrevisaoFornecimentoPastoService, PrevisaoFornecimentoPastoService>();
            services.AddScoped<IPrevisaoFornecimentoPastoRepositorio, PrevisaoFornecimentoPastoRepositorio>();
            services.AddScoped<IFornecimentoPastoService, FornecimentoPastoService>();
            services.AddScoped<IFornecimentoPastoRepositorio, FornecimentoPastoRepositorio>();

            //APi de Relatório
            services.AddHttpClient<IRelatorioService, RelatorioService>();

            return services;
        }
    }
}
