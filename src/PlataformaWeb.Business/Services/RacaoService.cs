using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Enums;
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
    public class RacaoService : Service, IRacaoService
    {
        private readonly IRacaoRepositorio _racaoRepositorio;

        public RacaoService(INotificador notificador, 
                            IUser appUser, 
                            IRacaoRepositorio racaoRepositorio) : base(notificador, appUser)
        {
            _racaoRepositorio = racaoRepositorio;
        }

        public async Task Adicionar(Racao entity)
        {
            if (!CalcularValores(entity)) return;

            await _racaoRepositorio.Adicionar(entity);

            await _racaoRepositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(Racao entity)
        {

            if (!CalcularValores(entity)) return;

            await _racaoRepositorio.Atualizar(entity);

            await _racaoRepositorio.UnitOfWork.Commit();

        }

        public async Task<List<RacaoDTO>> Buscar(Expression<Func<Racao, bool>> predicate)
        {
            return await _racaoRepositorio.BuscarQuery(predicate);
        }

        public async Task<Racao> ObterPorId(int id)
        {
            return await _racaoRepositorio.ObterPorId(id);
        }

        public async Task<List<RacaoDTO>> ObterPaginacao(int? id = null)
        {
            return await _racaoRepositorio.ObterPaginacao();
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id da Ração é inválido");
                return;
            }

            var racao = await _racaoRepositorio.ObterPorId(id);

            if (racao is null)
            {
                Notificar("Ração não existe");
                return;
            }

            //Desativando Insumos
            racao.InsumosRacao.ForEach(x => x.Status = Status.Desativado);

            await _racaoRepositorio.Remover(racao);

            await _racaoRepositorio.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);            
        }

        private bool ValidaInsercaoAtualizacao(Racao model)
        {
            var isValid = base.ValidaInsercaoAtualizacaoCliente(model);

            foreach (var insumo in model.InsumosRacao)
            {
                isValid = base.ValidaInsercaoAtualizacaoCliente(insumo);
                if (!isValid) break;
            }

            return isValid;
        }

        public bool CalcularValores(Racao model)
        {
            if (!ValidaInsercaoAtualizacao(model)) return false;

            if (!ExecutarValidacao(new RacaoValidation(TipoOperacao.Inclusao, false), model)) return false;

            model.CalcularInformacoesInsumos();

            if (model.InsumosRacao.Count > 0)
                return ExecutarValidacao(new RacaoValidation(TipoOperacao.Inclusao, true), model);

            return true;
        }

        public async Task RemoverInsumo(Racao model, int idRacaoInsumo)
        {
            if (idRacaoInsumo == 0 || !model.InsumosRacao.Any(x => x.Id == idRacaoInsumo))
            {
                Notificar("Id do Insumo não foi informado");
                return;
            }

            var insumo = await _racaoRepositorio.ObterRacaoInsumo(idRacaoInsumo);

            if (insumo is null || insumo.IdRacao != model.Id)
            {
                Notificar("Insumo não existe para a Ração Atual");
                return;
            }

            //remove da lista
            model.InsumosRacao.RemoveAll(x => x.Id == insumo.Id);

            await _racaoRepositorio.RemoverInsumo(insumo);            

            if (!CalcularValores(model)) return;

            await _racaoRepositorio.Atualizar(model);

            await _racaoRepositorio.UnitOfWork.Commit();

        }
    }
}
