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
    public class TecnicoService : Service, ITecnicoService
    {
        private readonly ITecnicoRepositorio _tecnicoRepositorio;
        private readonly IPessoaRepositorio _pessoaRepositorio;
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IUsuarioClienteRepositorio _usuarioClienteRepositorio;

        public TecnicoService(INotificador notificador,
                             IUser appUser,
                             ITecnicoRepositorio tecnicoRepositorio,
                             IPessoaRepositorio pessoaRepositorio,
                             IClienteRepositorio clienteRepositorio,
                             IUsuarioClienteRepositorio usuarioClienteRepositorio) : base(notificador, appUser)
        {
            _tecnicoRepositorio = tecnicoRepositorio;
            _pessoaRepositorio = pessoaRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _usuarioClienteRepositorio = usuarioClienteRepositorio;
        }

        public async Task Adicionar(Tecnico entity)
        {
            if (ExecutarValidacao(new TecnicoValidation(Enums.TipoOperacao.Inclusao), entity))
            {
                if (!await ValidaUsuarioExistente(entity)) return;

                await _tecnicoRepositorio.Adicionar(entity);

                await _tecnicoRepositorio.UnitOfWork.Commit();
            }
        }

        public async Task Atualizar(Tecnico entity)
        {
            if (ExecutarValidacao(new TecnicoValidation(Enums.TipoOperacao.Atualizacao), entity))
            {
                if (!await ValidaUsuarioExistente(entity)) return;

                await _tecnicoRepositorio.Atualizar(entity);

                await _tecnicoRepositorio.UnitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<Tecnico>> Buscar(Expression<Func<Tecnico, bool>> predicate)
        {
            return await _tecnicoRepositorio.Buscar(predicate);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public async Task<Tecnico> ObterPorId(int id)
        {
            return await _tecnicoRepositorio.ObterPorId(id);
        }

        public async Task<List<Tecnico>> ObterTodos()
        {
            return await _tecnicoRepositorio.ObterTodos();
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id do Técnico é inválido");
                return;
            }

            var tecnico = await _tecnicoRepositorio.ObterPorId(id);

            if (tecnico is null)
            {
                Notificar("Técnico não encontrado na base de dados");
                return;
            }            

            await _tecnicoRepositorio.Remover(tecnico);

            await RemoverClientesAndUsuarios(tecnico.Id);

            await _tecnicoRepositorio.UnitOfWork.Commit();

        }

        private async Task<bool> ValidaUsuarioExistente(Tecnico entity)
        {
            bool existeUsuario = await _pessoaRepositorio.ExisteUsuario(entity.Usuario, entity.Id);

            if (existeUsuario)
            {
                Notificar($"Já existe uma pessoa cadastrada com o usuário informado");
                return false;
            }

            bool existeEmail = await _pessoaRepositorio.ExisteEmail(entity.Email, entity.Id);
            if (existeEmail)
            {
                Notificar($"Já existe uma pessoa cadastrada com o email informado.");
                return false;
            }

            return true;
        }

        private async Task RemoverClientesAndUsuarios(int tecnicoId)
        {
            var clientesVinculados = await _clienteRepositorio.Buscar(c => c.IdTecnico == tecnicoId && c.Status == Enums.Status.Ativado);
            foreach (var cliente in clientesVinculados)
            {
                await _clienteRepositorio.Remover(cliente);
                var usuariosVinculados = await _usuarioClienteRepositorio.Buscar(c => c.IdCliente == cliente.Id);
                foreach (var usuario in usuariosVinculados)
                {
                    await _usuarioClienteRepositorio.Remover(usuario);
                }
            }
        }
    }
}
