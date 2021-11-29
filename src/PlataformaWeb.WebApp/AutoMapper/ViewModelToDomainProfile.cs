using AutoMapper;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Models.Cadastro;
using PlataformaWeb.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.AutoMapper
{
    public class ViewModelToDomainProfile : Profile
    {
        public ViewModelToDomainProfile()
        {
            CreateMap<UsuarioLoginViewModel, UsuarioLoginDTO>();

            CreateMap<TecnicoViewModel, Tecnico>()
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => TipoPessoa.Tecnico));

            CreateMap<ClienteViewModel, Cliente>()
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => TipoPessoa.Cliente));

            CreateMap<UsuarioClienteViewModel, UsuarioCliente>()
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => TipoPessoa.UsuarioCliente));

            CreateMap<PastoCurralViewModel, PastoCurral>();
            CreateMap<PastoCurralConsultaViewModel, PastoCurral>();

            CreateMap<CategoriaViewModel, Categoria>();
            CreateMap<RacaViewModel, Raca>();
            CreateMap<PropriedadeParceiraViewModel, PropriedadeParceira>();
            CreateMap<ProdutorParceiroViewModel, ProdutorParceiro>();
            CreateMap<FaseDoAnoViewModel, FaseDoAno>();
            CreateMap<FornecedorInsumoViewModel, FornecedorInsumo>();

            CreateMap<InsumoAlimentoViewModel, InsumoAlimento>();
            CreateMap<InsumoAlimentoConsultaViewModel, InsumoAlimento>()
                .ConstructUsing(c =>
                new InsumoAlimento
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    MateriaSeca = c.MateriaSeca ?? 0,
                    ValorKg = c.ValorKg ?? 0
                });

            CreateMap<SuplementoMineralViewModel, SuplementoMineral>();
            CreateMap<SuplementoMineralConsultaViewModel, SuplementoMineral>()
                .ConstructUsing(c =>
                new SuplementoMineral
                {
                    Id = c.Id,
                    Nome = c.Nome
                });

            CreateMap<RacaoViewModel, Racao>();
            CreateMap<RacaoInsumosViewModel, RacaoInsumo>()
                .ForMember(dest => dest.IdInsumoAlimento, opt => opt.MapFrom(src => src.InsumoAlimento != null ? src.InsumoAlimento.Id : 0));
            CreateMap<RacaoConsultaViewModel, Racao>();                

            CreateMap<PlanejamentoNutricionalViewModel, PlanejamentoNutricional>();
            CreateMap<PlanejamentoNutricionalConsultaViewModel, PlanejamentoNutricional>();

            CreateMap<PlanejamentoValoresConfinamentoViewModel, PlanejamentoValoresConfinamento>()
                .ForMember(dest => dest.IdRacao, opt => opt.MapFrom(src => src.Racao != null ? src.Racao.Id : 0));

            CreateMap<PlanejamentoValoresPastoViewModel, PlanejamentoValoresPasto>()
                .ForMember(dest => dest.IdSuplemento, opt => opt.MapFrom(src => src.SuplementoMineral != null ? src.SuplementoMineral.Id : 0))
                .ForMember(dest => dest.IdCategoria, opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Id : 0))
                .ForMember(dest => dest.IdFase, opt => opt.MapFrom(src => src.FaseDoAno != null ? src.FaseDoAno.Id : 0));

            CreateMap<LoteAnimalViewModel, LoteAnimalCadastro>();
            CreateMap<LoteAnimalDeletarViewModel, LoteAnimalCadastro>();
            CreateMap<LoteEntradaViewModel, LoteEntrada>();

            CreateMap<CausaMorteViewModel, CausaMorte>();
            CreateMap<MotivoMovimentacaoViewModel, MotivoMovimentacao>();
            CreateMap<FrigorificoViewModel, Frigorifico>();

            CreateMap<MovimentacaoEntreLoteViewModel, MovimentacaoEntreLote>()
                .ForMember(dest => dest.IdLocalDestino, opt => opt.MapFrom(src => src.LocalDestino != null ? src.LocalDestino.Id : 0))
                .ForMember(dest => dest.IdLocalOrigem, opt => opt.MapFrom(src => src.LocalOrigem != null ? src.LocalOrigem.Id : 0))
                .ForMember(dest => dest.IdMotivo, opt => opt.MapFrom(src => src.Motivo != null ? src.Motivo.Id : 0))
                .ForMember(dest => dest.IdLoteEntrada, opt => opt.MapFrom(src => src.LoteEntrada != null ? src.LoteEntrada.Id : 0))
                .ForMember(dest => dest.QuantidadeAnimais, opt => opt.MapFrom(src => src.LoteEntrada != null ? src.LoteEntrada.QuantidadeAnimais : 0));

            CreateMap<MovimentacaoAnimalViewModel, MovimentacaoAnimalCadastro>();
            CreateMap<MorteAnimalViewModel, MorteAnimalCadastro>();

            CreateMap<LoteSaidaViewModel, LoteSaida>()
                .ForMember(dest => dest.IdFrigorificoDestino, opt => opt.MapFrom(src => src.FrigorificoDestino != null ? (Nullable<int>)src.FrigorificoDestino.Id : null))
                .ForMember(dest => dest.IdProdutorDestino, opt => opt.MapFrom(src => src.ProdutorDestino != null ? (Nullable<int>)src.ProdutorDestino.Id : null));

            CreateMap<SaidaAnimalCadastroViewModel, SaidaAnimalCadastro>();
            CreateMap<SaidaAnimalLoteViewModel, SaidaAnimalLote>();

            CreateMap<NotaLeituraCochoViewModel, NotaLeituraCocho>();

            CreateMap<GerenciarPlanejamentoViewModel, GerenciarPlanejamentoDTO>();
            CreateMap<GerenciarPlanejamentoLoteViewModel, GerenciarPlanejamentoLoteDTO>();

            CreateMap<FornecimentoPastoViewModel, FornecimentoPasto>();

        }
    }
}
