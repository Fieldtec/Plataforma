using AutoMapper;
using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Models.Cadastro;
using PlataformaWeb.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaWeb.WebApp.AutoMapper
{
    public class DomainToViewModelProfile : Profile
    {
        public DomainToViewModelProfile()
        {
            CreateMap<Tecnico, TecnicoViewModel>();
            CreateMap<Cliente, ClienteViewModel>();
            CreateMap<UsuarioCliente, UsuarioClienteViewModel>();

            CreateMap<PastoCurral, PastoCurralViewModel>();
            CreateMap<PastoCurral, PastoCurralConsultaViewModel>();

            CreateMap<Categoria, CategoriaViewModel>();
            CreateMap<Raca, RacaViewModel>();
            CreateMap<PropriedadeParceira, PropriedadeParceiraViewModel>();
            CreateMap<ProdutorParceiro, ProdutorParceiroViewModel>();
            CreateMap<FaseDoAno, FaseDoAnoViewModel>();
            CreateMap<FornecedorInsumo, FornecedorInsumoViewModel>();
            
            CreateMap<InsumoAlimento, InsumoAlimentoViewModel>();
            CreateMap<InsumoAlimento, InsumoAlimentoConsultaViewModel>()
                .ForMember(dest => dest.NomeFornecedorInsumo, opt => opt.MapFrom(src => src.FornecedorInsumo.Nome));

            CreateMap<SuplementoMineral, SuplementoMineralViewModel>();
            CreateMap<SuplementoMineral, SuplementoMineralConsultaViewModel>()
                .ForMember(dest => dest.NomeFornecedorInsumo, opt => opt.MapFrom(src => src.FornecedorInsumo.Nome)); 

            CreateMap<Racao, RacaoViewModel>();
            CreateMap<RacaoInsumo, RacaoInsumosViewModel>();
            CreateMap<Racao, RacaoConsultaViewModel>();

            CreateMap<PlanejamentoNutricional, PlanejamentoNutricionalViewModel>();
            CreateMap<PlanejamentoNutricional, PlanejamentoNutricionalConsultaViewModel>();

            CreateMap<PlanejamentoValoresConfinamento, PlanejamentoValoresConfinamentoViewModel>();
            CreateMap<PlanejamentoValoresPasto, PlanejamentoValoresPastoViewModel>();

            CreateMap<LoteAnimalCadastro, LoteAnimalViewModel>();
            CreateMap<LoteAnimalCadastro, LoteAnimalDeletarViewModel>();
            CreateMap<MovimentacaoLoteEntrada, MovimentacaoLoteEntradaViewModel>();
            CreateMap<LoteEntrada, LoteEntradaViewModel>()
                .ForMember(dest => dest.QuantidadeAnimais, opt => opt.MapFrom(src => src.AnimaisLote.Count)); 

            CreateMap<CausaMorte, CausaMorteViewModel>();
            CreateMap<MotivoMovimentacao, MotivoMovimentacaoViewModel>();
            CreateMap<Frigorifico, FrigorificoViewModel>();

            CreateMap<MovimentacaoEntreLote, MovimentacaoEntreLoteViewModel>();
            CreateMap<MovimentacaoEntreLote, MovimentacaoEntreLoteDelecaoViewModel>()                
                .ForMember(dest => dest.Motivo, opt => opt.MapFrom(src => src.Motivo.Nome))
                .ForMember(dest => dest.LocalDestino, opt => opt.MapFrom(src => src.LocalDestino.Nome))
                .ForMember(dest => dest.LocalOrigem, opt => opt.MapFrom(src => src.LocalOrigem.Nome))
                .ForMember(dest => dest.DataLote, opt => opt.MapFrom(src => src.LoteEntrada.DataEntrada))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.LocalDestino.Tipo));

            CreateMap<MovimentacaoAnimalCadastro, MovimentacaoAnimalViewModel>();
            CreateMap<MorteAnimalCadastro, MorteAnimalViewModel>();

            CreateMap<LoteSaida, LoteSaidaViewModel>();

            CreateMap<SaidaAnimalCadastro, SaidaAnimalCadastroViewModel>();
            CreateMap<SaidaAnimalLote, SaidaAnimalLoteViewModel>();

            CreateMap<NotaLeituraCocho, NotaLeituraCochoViewModel>();

            CreateMap<GerenciarPlanejamentoDTO, GerenciarPlanejamentoViewModel>();
            CreateMap<GerenciarPlanejamentoLoteDTO, GerenciarPlanejamentoLoteViewModel>();

            CreateMap<ConfiguracaoFornecimentoPasto, ConfiguracaoFornecimentoPastoViewModel>()
                .ForMember(dest => dest.Segunda, opt => opt.MapFrom(src => src.Segunda == Status.Ativado))
                .ForMember(dest => dest.Terca, opt => opt.MapFrom(src => src.Terca == Status.Ativado))
                .ForMember(dest => dest.Quarta, opt => opt.MapFrom(src => src.Quarta == Status.Ativado))
                .ForMember(dest => dest.Quinta, opt => opt.MapFrom(src => src.Quinta == Status.Ativado))
                .ForMember(dest => dest.Sexta, opt => opt.MapFrom(src => src.Sexta == Status.Ativado))
                .ForMember(dest => dest.Sabado, opt => opt.MapFrom(src => src.Sabado == Status.Ativado))
                .ForMember(dest => dest.Domingo, opt => opt.MapFrom(src => src.Domingo == Status.Ativado));

        }
    }
}
