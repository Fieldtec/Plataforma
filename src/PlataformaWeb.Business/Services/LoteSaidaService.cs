using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Models.Cadastro;
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
    public class LoteSaidaService : Service, ILoteSaidaService
    {
        private readonly ILoteSaidaRepositorio _loteSaidaRepositorio;
        private readonly IPastoCurralRepositorio _pastoCurralRepositorio;
        private readonly ILoteEntradaRepositorio _loteEntradaRepositorio;
        private readonly IFuncoesRepositorio _funcoesRepositorio;

        public LoteSaidaService(INotificador notificador,
                                IUser appUser,
                                ILoteSaidaRepositorio loteSaidaRepositorio,
                                IPastoCurralRepositorio pastoCurralRepositorio,
                                ILoteEntradaRepositorio loteEntradaRepositorio, 
                                IFuncoesRepositorio funcoesRepositorio) : base(notificador, appUser)
        {
            _loteSaidaRepositorio = loteSaidaRepositorio;
            _pastoCurralRepositorio = pastoCurralRepositorio;
            _loteEntradaRepositorio = loteEntradaRepositorio;
            _funcoesRepositorio = funcoesRepositorio;
        }

        public async Task Adicionar(LoteSaida entity)
        {

            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            //Novo embarque deve começar com 0
            entity.QuantidadeAnimaEmbarcado = 0;

            //obtém próximo número
            entity.NumeroLote = await _loteSaidaRepositorio.ObterProximoNumeroLote();

            if (!ExecutarValidacao(new LoteSaidaValidation(Enums.TipoOperacao.Inclusao), entity)) return;

            var newEntity = new LoteSaida(entity);

            await _loteSaidaRepositorio.Adicionar(newEntity);

            await _loteSaidaRepositorio.UnitOfWork.Commit();

            entity.Id = newEntity.Id;
        }

        public async Task Atualizar(LoteSaida entity)
        {
            var loteSaida = await _loteSaidaRepositorio.ObterPorId(entity.Id);
            bool alterouDataEmbarque = false;
            if (loteSaida is null)
            {
                Notificar("Lote de Saída não encontrado no banco de dados");
                return;
            }

            loteSaida.IdFrigorificoDestino = entity.IdFrigorificoDestino;
            loteSaida.IdProdutorDestino = entity.IdProdutorDestino;
            loteSaida.TipoSaida = entity.TipoSaida;
            loteSaida.QuantidadeAnimalPrevisto = entity.QuantidadeAnimalPrevisto;

            if (loteSaida.QuantidadeAnimaEmbarcado > 0 && loteSaida.DataEmbarque.Date.CompareTo(entity.DataEmbarque.Date) != 0)
                alterouDataEmbarque = true;

            loteSaida.DataEmbarque = entity.DataEmbarque;

            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new LoteSaidaValidation(Enums.TipoOperacao.Atualizacao), loteSaida)) return;

            if (alterouDataEmbarque)
            {
                var animaisLoteSaida = await _loteSaidaRepositorio.ObterAnimaisNoLote(loteSaida.Id);
                animaisLoteSaida.ForEach(x => x.DataSaida = loteSaida.DataEmbarque);

                await _loteEntradaRepositorio.AtualizarAnimais(animaisLoteSaida);
            }

            var newEntity = new LoteSaida(loteSaida);

            await _loteSaidaRepositorio.Atualizar(newEntity);

            await _loteSaidaRepositorio.UnitOfWork.Commit();
        }

        public async Task<List<LoteSaidaDTO>> Buscar(Expression<Func<LoteSaida, bool>> predicate)
        {
            return await _loteSaidaRepositorio.BuscarQuery(predicate);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public async Task<List<LoteSaidaDTO>> ObterPaginacao(int? id = null)
        {
            return await _loteSaidaRepositorio.ObterPaginacao();
        }

        public async Task<LoteSaida> ObterPorId(int id)
        {
            return await _loteSaidaRepositorio.ObterPorId(id);
        }

        public async Task Remover(int id)
        {
            var loteSaida = await _loteSaidaRepositorio.ObterPorId(id);

            if (loteSaida is null)
            {
                Notificar("Lote de Saída não encontrado no banco de dados");
                return;
            }

            if (loteSaida.QuantidadeAnimaEmbarcado > 0)
            {
                Notificar($"Lote de Saída possui {loteSaida.QuantidadeAnimaEmbarcado} animais embarcados. Não é permitido a exclusão!");
                return;
            }

            await _loteSaidaRepositorio.Remover(loteSaida);

            await _loteSaidaRepositorio.UnitOfWork.Commit();

        }

        #region ...::: Saída de Animal :::...

        public async Task AtualizarSaida(SaidaAnimalCadastro saida)
        {
            if (!ExecutarValidacao(new SaidaAnimalCadastroValidation(), saida)) return;

            var loteSaida = await ObterLoteSaida(saida.Id);
            if (loteSaida is null) return;

            var animaisNoLoteSaida = await _loteSaidaRepositorio.ObterAnimaisNoLote(loteSaida.Id);

            if (animaisNoLoteSaida.Count == 0)
            {
                Notificar("Não existem animais vinculados no Lote de Saída. Operação não pode ser feita.");
                return;
            }

            foreach (var itemSaida in saida.Lotes)
            {
                var local = await ObterLocal(itemSaida.Local.Id);
                if (local is null) break;

                var loteEntrada = await ObterLoteEntrada(itemSaida.Lote.Id);
                if (loteEntrada is null) break;

                if (!ValidaLotePertenceLocal(loteEntrada, local)) break;

                var animaisLoteEntrada = animaisNoLoteSaida.Where(x => x.IdLote == loteEntrada.Id).ToList();

                if (animaisLoteEntrada.Count == 0)
                {
                    Notificar($"Não existem animais no lote de saída pertencentes ao Local {local.Nome}");
                    break;
                }

                animaisLoteEntrada.ForEach(x => x.PesoSaida = itemSaida.PesoMedio);
            }

            if (TemNotificacao()) return;

            await _loteEntradaRepositorio.AtualizarAnimais(animaisNoLoteSaida);

            await _loteSaidaRepositorio.UnitOfWork.Commit();

            await _funcoesRepositorio.AtualizaGmd(AppUser.ObterIdCliente());

            await _funcoesRepositorio.AtualizaLote(AppUser.ObterIdCliente(), 0, saida.DataEmbarque);
        }

        public async Task ExcluirSaida(SaidaAnimalCadastro saida)
        {
            int totalAnimaisEmbarcados = 0;
            var loteSaida = await ObterLoteSaida(saida.Id);
            if (loteSaida is null) return;

            var animaisNoLoteSaida = await _loteSaidaRepositorio.ObterAnimaisNoLote(loteSaida.Id);

            if (animaisNoLoteSaida.Count == 0)
            {
                Notificar("Não existem animais vinculados no Lote de Saída. Operação não pode ser feita");
                return;
            }

            foreach (var itemSaida in saida.Lotes)
            {
                var local = await ObterLocal(itemSaida.Local.Id);
                if (local is null) break;

                var loteEntrada = await ObterLoteEntrada(itemSaida.Lote.Id);
                if (loteEntrada is null) break;

                if (!ValidaLotePertenceLocal(loteEntrada, local)) break;

                int totalAnimaisLoteEntrada = await _loteSaidaRepositorio.ObterQuantidadeAnimaisNoLoteEntradaSaida(loteEntrada.Id, loteSaida.Id);

                if (totalAnimaisLoteEntrada == 0)
                {
                    Notificar("Não foi encontrado animais vinculados para o lote de entrada e saída para serem retornados");
                    break;
                }

                totalAnimaisEmbarcados += totalAnimaisLoteEntrada;

                local.IncluirAnimais(totalAnimaisLoteEntrada);

                await _pastoCurralRepositorio.Atualizar(local);

            }

            if (TemNotificacao()) return;

            if (animaisNoLoteSaida.Count != totalAnimaisEmbarcados)
            {
                Notificar($"Total de Animais registrados no Lote de Saída({loteSaida.Animais.Count}) diverge do Total de Animais embarcados nos lotes({totalAnimaisEmbarcados}).");
                return;
            }

            foreach (var animal in animaisNoLoteSaida)
                animal.CancelarSaida();

            await _loteEntradaRepositorio.AtualizarAnimais(animaisNoLoteSaida);

            loteSaida.QuantidadeAnimaEmbarcado = 0;

            await _loteSaidaRepositorio.Atualizar(new LoteSaida(loteSaida));

            await _loteSaidaRepositorio.UnitOfWork.Commit();

            await _funcoesRepositorio.AtualizaGmd(AppUser.ObterIdCliente());

            await _funcoesRepositorio.AtualizaLote(AppUser.ObterIdCliente(), 0, saida.DataEmbarque);

        }

        public async Task LancarSaida(SaidaAnimalCadastro saida)
        {
            if (!ExecutarValidacao(new SaidaAnimalCadastroValidation(), saida)) return;

            var loteSaida = await ObterLoteSaida(saida.Id);

            if (loteSaida is null) return;

            foreach (var itemSaida in saida.Lotes)
            {
                var local = await ObterLocal(itemSaida.Local.Id);
                if (local is null) break;

                var loteEntrada = await ObterLoteEntrada(itemSaida.Lote.Id);
                if (loteEntrada is null) break;

                if (!ValidaLotePertenceLocal(loteEntrada, local)) break;

                if (!ValidaQuantidadeEmbarcado(loteEntrada, local, itemSaida)) break;

                var animaisSaidaLote = loteEntrada.RealizarSaidaAnimais(loteSaida, itemSaida.QuantidadeEmbarcado, itemSaida.PesoMedio);

                await _loteEntradaRepositorio.AtualizarAnimais(animaisSaidaLote);

                local.RetirarAnimais(animaisSaidaLote.Count);

                await _pastoCurralRepositorio.Atualizar(local);

            }

            if (TemNotificacao()) return;

            loteSaida.QuantidadeAnimaEmbarcado = saida.Lotes.Sum(x => x.QuantidadeEmbarcado);

            await _loteSaidaRepositorio.Atualizar(new LoteSaida(loteSaida));

            await _loteSaidaRepositorio.UnitOfWork.Commit();

            await _funcoesRepositorio.AtualizaGmd(AppUser.ObterIdCliente());

            await _funcoesRepositorio.AtualizaLote(AppUser.ObterIdCliente(), 0, saida.DataEmbarque);
        }

        public async Task<SaidaAnimalCadastro> ObterSaidaAnimal(int id)
        {
            return await _loteSaidaRepositorio.ObterSaidaAnimal(id);
        }

        private async Task<LoteSaida> ObterLoteSaida(int id)
        {
            var loteSaida = await _loteSaidaRepositorio.ObterPorId(id);

            if (loteSaida is null)
                Notificar("Lote de Saída não encontrada");

            return loteSaida;
        }

        private async Task<PastoCurral> ObterLocal(int id)
        {
            var local = await _pastoCurralRepositorio.ObterPorId(id);

            if (local is null)
                Notificar("Local não encontrado no banco de dados");

            return local;
        }

        private async Task<LoteEntrada> ObterLoteEntrada(int id)
        {
            var lote = await _loteEntradaRepositorio.ObterPorId(id);
            if (lote is null)
                Notificar("Lote de entrada não encontrado no banco de dados");

            return lote;
        }

        private bool ValidaLotePertenceLocal(LoteEntrada loteEntrada, PastoCurral local)
        {
            if (local.Id != loteEntrada.IdLocal)
            {
                Notificar("Lote de Entrada informado não corresponde ao Local informado");
                return false;
            }

            return true;
        }

        private bool ValidaQuantidadeEmbarcado(LoteEntrada loteEntrada, PastoCurral local, SaidaAnimalLote itemSaida)
        {
            if (loteEntrada.AnimaisLote.Count < itemSaida.QuantidadeEmbarcado)
            {
                Notificar($"Quantidade no lote({loteEntrada.AnimaisLote.Count}) do local {local.Nome} é inferior à Quantidade embarcada informada({itemSaida.QuantidadeEmbarcado}).");
                return false;
            }

            return true;
        }

        


        #endregion

    }
}
