using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Models.Validations;
using PlataformaWeb.Business.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class PlanejamentoNutricionalService : Service, IPlanejamentoNutricionalService
    {
        private readonly IPlanejamentoNutricionalRepositorio _planejamentoNutricionalRepositorio;
        private readonly ILoteEntradaRepositorio _loteEntradaRepositorio;
        public PlanejamentoNutricionalService(INotificador notificador,
                                              IUser appUser,
                                              IPlanejamentoNutricionalRepositorio planejamentoNutricionalRepositorio, 
                                              ILoteEntradaRepositorio loteEntradaRepositorio) : base(notificador, appUser)
        {
            _planejamentoNutricionalRepositorio = planejamentoNutricionalRepositorio;
            _loteEntradaRepositorio = loteEntradaRepositorio;
        }

        public async Task Adicionar(PlanejamentoNutricional entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new PlanejamentoNutricionalValidation(TipoOperacao.Inclusao), entity)) return;

            if (!ValidaValoresPlanejamento(entity)) return;

            await _planejamentoNutricionalRepositorio.Adicionar(TransforaObjetoParaSalvar(entity));

            await _planejamentoNutricionalRepositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(PlanejamentoNutricional entity)
        {

            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new PlanejamentoNutricionalValidation(TipoOperacao.Atualizacao), entity)) return;

            if (!ValidaValoresPlanejamento(entity)) return;

            await _planejamentoNutricionalRepositorio.Atualizar(TransforaObjetoParaSalvar(entity));

            await _planejamentoNutricionalRepositorio.UnitOfWork.Commit();

        }

        private PlanejamentoNutricional TransforaObjetoParaSalvar(PlanejamentoNutricional entity)
        {
            PlanejamentoNutricional novoObjeto = new PlanejamentoNutricional();
            novoObjeto.Id = entity.Id;
            novoObjeto.IdCliente = entity.IdCliente;
            novoObjeto.Nome = entity.Nome;
            novoObjeto.Tipo = entity.Tipo;
            novoObjeto.Status = entity.Status;

            foreach (var item in entity.PlanejamentoValoresPasto)
            {
                novoObjeto.PlanejamentoValoresPasto.Add(new PlanejamentoValoresPasto
                {
                    Id = item.Id,
                    GmdEsperado = item.GmdEsperado,
                    IdCategoria = item.IdCategoria,
                    IdFase = item.IdFase,
                    IdPlanejamento = item.IdPlanejamento,
                    IdSuplemento = item.IdSuplemento,
                    ImspvEsperado = item.ImspvEsperado,
                    Status = item.Status
                });
            }

            foreach (var item in entity.PlanejamentoValoresConfinamento)
            {
                novoObjeto.PlanejamentoValoresConfinamento.Add(new PlanejamentoValoresConfinamento
                {
                    Id = item.Id,
                    GmdEsperado = item.GmdEsperado,
                    ImspvEsperado = item.ImspvEsperado,
                    IdPlanejamento = item.IdPlanejamento,
                    Status = item.Status,
                    DiaFim = item.DiaFim,
                    DiaInicio = item.DiaInicio,
                    IdRacao = item.IdRacao
                });
            }

            return novoObjeto;
        }

        public async Task<List<PlanejamentoNutricionalDTO>> Buscar(Expression<Func<PlanejamentoNutricional, bool>> predicate)
        {
            return await _planejamentoNutricionalRepositorio.BuscarQuery(predicate);
        }

        public async Task<PlanejamentoNutricional> ObterPorId(int id)
        {
            return await _planejamentoNutricionalRepositorio.ObterPorId(id);
        }

        public async Task<List<PlanejamentoNutricionalDTO>> ObterPaginacao(int? id = null)
        {
            return await _planejamentoNutricionalRepositorio.ObterPaginacao();
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id do Planejamento é inválido");
                return;
            }

            var planejamentoNutricional = await _planejamentoNutricionalRepositorio.ObterPorId(id);

            if (planejamentoNutricional is null)
            {
                Notificar("Planejamento não existe");
                return;
            }

            planejamentoNutricional.PlanejamentoValoresConfinamento.ForEach(x => x.Status = Status.Desativado);
            planejamentoNutricional.PlanejamentoValoresPasto.ForEach(x => x.Status = Status.Desativado);

            await _planejamentoNutricionalRepositorio.Remover(TransforaObjetoParaSalvar(planejamentoNutricional));

            await _planejamentoNutricionalRepositorio.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        private bool ValidaValoresPlanejamento(PlanejamentoNutricional entity)
        {
            if (entity.Tipo == TipoPlanejamentoNutricional.Confinamento)
            {
                return ValidaValoresConfinamento(entity.PlanejamentoValoresConfinamento);
            } 
            else if (entity.Tipo == TipoPlanejamentoNutricional.Pasto)
            {
                return ValidaValoresPasto(entity.PlanejamentoValoresPasto);
            }

            return true;
        }

        private bool ValidaValoresPasto(List<PlanejamentoValoresPasto> valoresPasto)
        {
            if (valoresPasto.GroupBy(x => new { x.IdFase, x.IdCategoria }).Any(x => x.Count() > 1))
            {
                Notificar("Fase do Ano e Categoria não podem se repetir para o mesmo Planejamento");
            }

            return !TemNotificacao();
        }

        private bool ValidaValoresConfinamento(List<PlanejamentoValoresConfinamento> valoresConfinamento)
        {
            int diaFinalAnterior = 0;
            int idRacao = 0;
            foreach (var valorConfinamento in valoresConfinamento)
            {
                if (diaFinalAnterior == 0)
                {
                    //se dia final anterior = 0, está validando o primeiro registro, então o Dia Início obrigatóriamente precisa ser 1
                    if (valorConfinamento.DiaInicio != 1)
                    {
                        Notificar("O primeiro Dia do confinamento precisa ser 1");
                        break;
                    }
                }
                else if (diaFinalAnterior + 1 != valorConfinamento.DiaInicio) //obrigatóriamente o dia inicial do registro atual precisa ser sequência do dia final do registro anterior 
                {

                    Notificar($"O Dia Início({valorConfinamento.DiaInicio}) do Confinamento precisa ser sequência do dia final do confinamento anterior({diaFinalAnterior})");
                    break;
                }

                if (idRacao == valorConfinamento.IdRacao)
                {
                    Notificar("Ração não pode ser cadastrado em sequência");
                    break;
                }

                idRacao = valorConfinamento.IdRacao;
                diaFinalAnterior = valorConfinamento.DiaFim;
            }

            return !TemNotificacao();
        }

        public async Task<List<GerenciarPlanejamentoLoteDTO>> BuscarLotesGerenciamento(int idPlanejamento)
        {
            return await _planejamentoNutricionalRepositorio.BuscarLotesGerenciamento(idPlanejamento);
        }

        public async Task AlterarPlanejamentosNosLotes(List<GerenciarPlanejamentoDTO> planejamentosGerenciamento)
        {
            foreach (var gerenciamento in planejamentosGerenciamento)
            {
                var planejamentoDestino = await _planejamentoNutricionalRepositorio.ObterPorId(gerenciamento.IdPlanejamentoDestino);

                if (planejamentoDestino is null)
                {
                    Notificar($"Planejamento Nutricional Destino {gerenciamento.IdPlanejamentoDestino} não encontrado");
                    break;
                }

                var planejamentoOrigem = await _planejamentoNutricionalRepositorio.ObterPorId(gerenciamento.Lote.IdPlanejamento);

                if (planejamentoOrigem is null)
                {
                    Notificar($"Planejamento Nutricional Origem {gerenciamento.Lote.IdPlanejamento} não encontrado");
                    break;
                }

                if (planejamentoOrigem.Tipo != planejamentoDestino.Tipo)
                {
                    Notificar($"Tipo do Planejamento Origem({planejamentoOrigem.Tipo.ObterDescricao()}) é diferente do Tipo do Planejamento Destino({planejamentoDestino.Tipo.ObterDescricao()})");
                    break;
                }

                var loteEntrada = await _loteEntradaRepositorio.ObterPorIdSimplicado(gerenciamento.Lote.IdLote);

                if (loteEntrada is null)
                {
                    Notificar("Lote Entrada não encontrado no banco de dados");
                    break;
                }

                if (loteEntrada.IdPlanejamento != planejamentoOrigem.Id)
                {
                    Notificar($"Planejamento de Origem {planejamentoOrigem.Nome} não pertence ao Lote selecionado");
                    break;
                }

                loteEntrada.IdPlanejamento = planejamentoDestino.Id;

                await _loteEntradaRepositorio.Atualizar(loteEntrada);

            }

            if (TemNotificacao()) return;

            await _planejamentoNutricionalRepositorio.UnitOfWork.Commit();
        }
    }
}
