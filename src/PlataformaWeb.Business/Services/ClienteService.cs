using Microsoft.Extensions.Options;
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
    public class ClienteService : Service, IClienteService
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IPessoaRepositorio _pessoaRepositorio;
        private readonly IUsuarioClienteRepositorio _usuarioClienteRepositorio;
        private readonly ITecnicoRepositorio _tecnicoRepositorio;
        private readonly List<NotasLeituraCochoSettings> _notasLeituraCochoSettings;

        public ClienteService(INotificador notificador,
                              IUser appUser,
                              IClienteRepositorio clienteRepositorio,
                              IPessoaRepositorio pessoaRepositorio,
                              IUsuarioClienteRepositorio usuarioClienteRepositorio,
                              ITecnicoRepositorio tecnicoRepositorio, 
                              IOptions<List<NotasLeituraCochoSettings>> notasLeituraCochoSettings) : base(notificador, appUser)
        {
            _clienteRepositorio = clienteRepositorio;
            _pessoaRepositorio = pessoaRepositorio;
            _usuarioClienteRepositorio = usuarioClienteRepositorio;
            _tecnicoRepositorio = tecnicoRepositorio;
            _notasLeituraCochoSettings = notasLeituraCochoSettings.Value;
        }


        private List<NotaLeituraCocho> ObterNotasLeituraCocho()
        {
            List<NotaLeituraCocho> notas = new List<NotaLeituraCocho>();

            foreach (var notaSettings in _notasLeituraCochoSettings)
            {
                notas.Add(new NotaLeituraCocho
                {
                    AjustePorcentagem = notaSettings.AjustePorcentagem,
                    Nome = notaSettings.Nome,
                    Status = Status.Ativado
                });
            }

            return notas;
        }

        public async Task Adicionar(Cliente entity)
        {
            if (!ValidarAcesso(entity)) return;

            if (!ExecutarValidacao(new ClienteValidation(TipoOperacao.Inclusao), entity)) return;

            if (!await ValidaUsuarioExistente(entity)) return;

            if (entity.Status == Status.Ativado) if (!await ValidaNumeroLicencas()) return;

            entity.NotasLeituraCocho = ObterNotasLeituraCocho();

            await _clienteRepositorio.Adicionar(entity);           

            await _clienteRepositorio.UnitOfWork.Commit();
        }

        public async Task Atualizar(Cliente entity)
        {
            if (!ValidarAcesso(entity)) return;

            if (!ExecutarValidacao(new ClienteValidation(TipoOperacao.Atualizacao), entity)) return;

            if (!await ValidaUsuarioExistente(entity)) return;            

            await _clienteRepositorio.Atualizar(entity);

            await _clienteRepositorio.UnitOfWork.Commit();

        }

        public async Task<IEnumerable<Cliente>> Buscar(Expression<Func<Cliente, bool>> predicate)
        {
            return await _clienteRepositorio.Buscar(predicate);
        }

        public async Task<Cliente> ObterPorId(int id)
        {
            return await _clienteRepositorio.ObterPorId(id);
        }

        public async Task<List<ClienteDTO>> ObterPaginacao(int? id = null)
        {
            return await _clienteRepositorio.ObterPaginacao();
        }

        public async Task Remover(int id)
        {
            if (id <= 0)
            {
                Notificar("Id do Cliente é inválido");
                return;
            }

            var cliente = await _clienteRepositorio.ObterPorId(id);

            if (cliente is null)
            {
                Notificar("Cliente não existe");
                return;
            }

            if (cliente.IdTecnico != AppUser.ObterId())
            {
                Notificar("Cliente não pertence ao técnico");
                return;
            }

            await _clienteRepositorio.Remover(cliente);

            //depois, criar uma lógica no Dao de UsuarioCliente para remover via query
            await RemoverUsuarios(cliente.Id);

            //adicionar lógica para remover todos os vínculos relacionados ao cliente.

            await _clienteRepositorio.UnitOfWork.Commit();
        }
        
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public async Task<int> ObterLicencasDisponiveis()
        {
            return await _tecnicoRepositorio.BuscaLicencasDisponiveis(AppUser.ObterId());
        }

        private async Task<bool> ValidaUsuarioExistente(Cliente entity)
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

        private async Task<bool> ValidaNumeroLicencas()
        {
            int quantidadeLicensas = await ObterLicencasDisponiveis();

            if (quantidadeLicensas > 0) return true;

            Notificar("Técnico não possui Licenças Disponíveis.");

            return false;
        }

        private async Task RemoverUsuarios(int clienteId)
        {
            var usuariosVinculados = await _usuarioClienteRepositorio.Buscar(c => c.IdCliente == clienteId && c.Status == Enums.Status.Ativado);

            foreach (var usuario in usuariosVinculados)
            {
                await _usuarioClienteRepositorio.Remover(usuario);
            }
        }

        private bool ValidarAcesso(Cliente entity)
        {
            if (!AppUser.EhTecnico())
            {
                Notificar("Apenas usuários com o Perfil de Técnico podem cadastrar Clientes");
                return false;
            }

            entity.IdTecnico = AppUser.ObterId();

            return true;
        }
        
    }
}
