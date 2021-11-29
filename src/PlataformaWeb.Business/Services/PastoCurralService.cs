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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class PastoCurralService : Service, IPastoCurralService
    {

        private readonly IPastoCurralRepositorio _pastoCurralRepositorio;

        public PastoCurralService(INotificador notificador, 
                                  IUser appUser, 
                                  IPastoCurralRepositorio pastoCurralRepositorio) : base(notificador, appUser)
        {
            _pastoCurralRepositorio = pastoCurralRepositorio;
        }

        public async Task Adicionar(PastoCurral entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new PastoCurralValidation(TipoOperacao.Inclusao), entity)) return;

            if (!await ValidaNomePastoCurral(entity)) return;

            await _pastoCurralRepositorio.Adicionar(entity);

            await _pastoCurralRepositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(PastoCurral entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new PastoCurralValidation(TipoOperacao.Atualizacao), entity)) return;

            if (!await ValidaNomePastoCurral(entity)) return;

            await _pastoCurralRepositorio.Atualizar(entity);

            await _pastoCurralRepositorio.UnitOfWork.Commit();

        }

        public async Task<List<PastoCurralDTO>> Buscar(Expression<Func<PastoCurral, bool>> predicate)
        {
            return await _pastoCurralRepositorio.BuscarQuery(predicate);
        }

        public async Task<PastoCurral> ObterPorId(int id)
        {
            return await _pastoCurralRepositorio.ObterPorId(id);
        }

        public async Task<List<PastoCurralDTO>> ObterPaginacao(int? id = null)
        {
            return await _pastoCurralRepositorio.ObterPaginacao(id);
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id do Pasto/Curral é inválido");
                return;
            }

            var pastoCurral = await _pastoCurralRepositorio.ObterPorId(id);

            if (pastoCurral is null)
            {
                Notificar("Pasto/Curral não existe");
                return;
            }

            if (await _pastoCurralRepositorio.ExisteLoteAtivo(pastoCurral.Id))
            {
                Notificar("Pasto/Curral existe um Lote de Entrada Ativo. Não é possível fazer a exclusão.");
                return;
            }

            await _pastoCurralRepositorio.Remover(pastoCurral);

            await _pastoCurralRepositorio.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        private async Task<bool> ValidaNomePastoCurral(PastoCurral model)
        {
            var where = PredicateBuilder.True<PastoCurral>().And(x => x.Nome.Trim() == model.Nome.Trim() && x.Tipo == model.Tipo);
            if (model.Id != 0)
                where = where.And(x => x.Id != model.Id);

            var pastoCurral = await _pastoCurralRepositorio.BuscarQuery(where);

            if (pastoCurral.Count > 0)
            {
                Notificar($"O nome de local {model.Nome} já existe para o tipo de lugar {model.Tipo.ObterDescricao()}");
                return false;
            }

            return true;
        }

        public async Task<List<LocalLoteDTO>> BuscarLocaisAtivos()
        {
            return await _pastoCurralRepositorio.BuscarLocaisAtivos();
        }
    }
}
