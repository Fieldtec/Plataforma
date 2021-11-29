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
    public class UsuarioClienteService : Service, IUsuarioClienteService
    {
        private readonly IUsuarioClienteRepositorio _usuarioClienteRepositorio;
        private readonly IPessoaRepositorio _pessoaRepositorio;

        public UsuarioClienteService(INotificador notificador,
                                     IUser appUser,
                                     IUsuarioClienteRepositorio usuarioClienteRepositorio, 
                                     IPessoaRepositorio pessoaRepositorio) : base(notificador, appUser)
        {
            _usuarioClienteRepositorio = usuarioClienteRepositorio;
            _pessoaRepositorio = pessoaRepositorio;
        }

        

        public async Task Adicionar(UsuarioCliente entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new UsuarioClienteValidation(TipoOperacao.Inclusao), entity)) return;

            if (!await ValidaUsuarioExistente(entity)) return;            

            await _usuarioClienteRepositorio.Adicionar(entity);

            await _usuarioClienteRepositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(UsuarioCliente entity)
        {
            if (!ValidaInsercaoAtualizacaoCliente(entity)) return;

            if (!ExecutarValidacao(new UsuarioClienteValidation(TipoOperacao.Atualizacao), entity)) return;

            if (!await ValidaUsuarioExistente(entity)) return;

            await _usuarioClienteRepositorio.Atualizar(entity);

            await _usuarioClienteRepositorio.UnitOfWork.Commit();

        }

        public async Task<IEnumerable<UsuarioCliente>> Buscar(Expression<Func<UsuarioCliente, bool>> predicate)
        {
            return await _usuarioClienteRepositorio.Buscar(predicate);
        }

        public async Task<UsuarioCliente> ObterPorId(int id)
        {
            return await _usuarioClienteRepositorio.ObterPorId(id);
        }

        public async Task<List<UsuarioClienteDTO>> ObterPaginacao(int? id = null)
        {
            return await _usuarioClienteRepositorio.ObterPaginacao(id);            
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id do Usuário é inválido");
                return;
            }

            var usuario = await _usuarioClienteRepositorio.ObterPorId(id);

            if (usuario is null)
            {
                Notificar("Usuário não existe");
                return;
            }

            await _usuarioClienteRepositorio.Remover(usuario);

            await _usuarioClienteRepositorio.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }


        private async Task<bool> ValidaUsuarioExistente(UsuarioCliente entity)
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
    }
}
