using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Models.Validations;
using PlataformaWeb.Business.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class NotaLeituraCochoService : Service, INotaLeituraCochoService
    {
        private readonly INotaLeituraCochoRepositorio _repositorio;

        public NotaLeituraCochoService(INotificador notificador, 
            IUser appUser, 
            INotaLeituraCochoRepositorio repositorio) : base(notificador, appUser)
        {
            _repositorio = repositorio;
        }

        public async Task Atualizar(List<NotaLeituraCocho> notas)
        {
            if (!ValidaNotasIguais(notas)) return;

            foreach (var nota in notas)
            {
                if (!ValidaInsercaoAtualizacaoCliente(nota)) return;

                if (!ExecutarValidacao(new NotaLeituraCochoValidation(), nota)) return;

                await _repositorio.Atualizar(nota);
            }

            await _repositorio.AdicionarLog(notas);

            await _repositorio.UnitOfWork.Commit();
        }

        public bool ValidaNotasIguais(List<NotaLeituraCocho> notas)
        {
            if (notas.GroupBy(x => x.AjustePorcentagem).Any(x => x.Count() > 1))
            {
                Notificar("Não é permitido Ajustes % de notas iguais");
                return false;
            }

            return true;
        }

        public async Task<List<NotaLeituraCocho>> ObterTodos()
        {
            return await _repositorio.ObterTodos();
        }

        public async Task Adicionar(NotaLeituraCocho nota)
        {
            if (!ValidaInsercaoAtualizacaoCliente(nota)) return;

            if (!ExecutarValidacao(new NotaLeituraCochoValidation(), nota)) return;

            await _repositorio.Adicionar(nota);

            await _repositorio.UnitOfWork.Commit();
        }

        public async Task Remover(int id)
        {
            var model = await _repositorio.ObterPorId(id);

            if (model is null)
            {
                Notificar("Nota não encontrada no banco de dados");
                return;
            }

            await _repositorio.Remover(model);

            await _repositorio.UnitOfWork.Commit();
        }
    }
}
