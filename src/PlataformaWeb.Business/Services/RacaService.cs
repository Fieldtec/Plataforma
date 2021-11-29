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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class RacaService : Service, IRacaService
    {
        private readonly IRacaRepositorio _racaRepositorio;

        public RacaService(INotificador notificador, 
                           IUser appUser, 
                           IRacaRepositorio racaRepositorio) : base(notificador, appUser)
        {
            _racaRepositorio = racaRepositorio;
        }

        public async Task Adicionar(Raca entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new RacaValidation(TipoOperacao.Inclusao), entity)) return;

            await _racaRepositorio.Adicionar(entity);

            await _racaRepositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(Raca entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new RacaValidation(TipoOperacao.Atualizacao), entity)) return;

            await _racaRepositorio.Atualizar(entity);

            await _racaRepositorio.UnitOfWork.Commit();

        }

        public async Task<List<RacaDTO>> Buscar(Expression<Func<Raca, bool>> predicate)
        {
            return await _racaRepositorio.BuscarQuery(predicate);
        }

        public async Task<Raca> ObterPorId(int id)
        {
            return await _racaRepositorio.ObterPorId(id);
        }

        public async Task<List<RacaDTO>> ObterPaginacao(int? id = null)
        {
            return await _racaRepositorio.ObterPaginacao(id);
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id da Raça é inválido");
                return;
            }

            var Raca = await _racaRepositorio.ObterPorId(id);

            if (Raca is null)
            {
                Notificar("Raça não existe");
                return;
            }

            await _racaRepositorio.Remover(Raca);

            await _racaRepositorio.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
