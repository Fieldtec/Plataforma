using PlataformaWeb.Business.DTO;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Interfaces.Services;
using PlataformaWeb.Business.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly IAutenticacaoRepositorio _autenticacaoRepositorio;
        private readonly IUsuarioClienteRepositorio _usuarioClienteRepositorio;
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IFuncoesRepositorio _funcoesRepositorio;

        public AutenticacaoService(INotificador notificador,
                                   IUser appUser,
                                   IAutenticacaoRepositorio autenticacaoRepositorio,
                                   IUsuarioClienteRepositorio usuarioClienteRepositorio,
                                   IClienteRepositorio clienteRepositorio, 
                                   IFuncoesRepositorio funcoesRepositorio) : base(notificador, appUser)
        {
            _autenticacaoRepositorio = autenticacaoRepositorio;
            _usuarioClienteRepositorio = usuarioClienteRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _funcoesRepositorio = funcoesRepositorio;
        }

        public async Task<UsuarioResponseDTO> Login(UsuarioLoginDTO login)
        {
            var usuarioResponse = await _autenticacaoRepositorio.Login(login);

            if (usuarioResponse is null)
            {
                Notificar("Credenciais Inválidas");
                return null;
            }

            //Caso o Login for Tipo Cliente ou Usuário do Cliente, fazer algumas validações no momento do login
            if (usuarioResponse.Tipo == TipoPessoa.Cliente || usuarioResponse.Tipo == TipoPessoa.UsuarioCliente)
            {
                //Obtém o Id do Usuário para conferir se a Data de Validade da Licença expirou.               
                int idCliente = usuarioResponse.Id;

                if (usuarioResponse.Tipo == TipoPessoa.UsuarioCliente)
                {
                    //Caso o tipo da pessoa for Usuário do Cliente, valida o código do cliente no banco de dados.
                    if (!await ValidarClienteDoUsuario(usuarioResponse)) return null;

                    //Caso a validção funcione a variável idCliente recebe o Id do Cliente do Usuário
                    idCliente = usuarioResponse.IdCliente;
                }

                //Valida se a Data de Validade da Licença está válida
                if (!await ValidarDataValidadeLicenca(idCliente)) return null;

                await _funcoesRepositorio.AtualizaGmd(idCliente);
                await _funcoesRepositorio.AtualizaLote(idCliente, 0, DateTime.Now);
            }


            return usuarioResponse;
        }

        public async Task<List<Claim>> ObterClaims(UsuarioResponseDTO usuarioLogado)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, usuarioLogado.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, usuarioLogado.Email));
            claims.Add(new Claim(ClaimTypes.Name, usuarioLogado.Nome));
            claims.Add(new Claim(ClaimTypes.Surname, usuarioLogado.Usuario));

            //Role do Usuário
            claims.Add(new Claim("role", usuarioLogado.Tipo.ObterDescricao()));

            if (usuarioLogado.Tipo == TipoPessoa.Cliente || usuarioLogado.Tipo == TipoPessoa.UsuarioCliente) {

                int idCliente = usuarioLogado.Tipo == TipoPessoa.Cliente ? usuarioLogado.Id : usuarioLogado.IdCliente;

                claims.Add(new Claim(CustomClaims.UsuarioCliente, idCliente.ToString()));
                //claims.Add(new Claim(CustomClaims.UsuarioCliente, usuarioLogado.Id.ToString()));            
                //claims.Add(new Claim(CustomClaims.UsuarioCliente, usuarioLogado.IdCliente.ToString()));

                var cliente = await _clienteRepositorio.ObterPorId(idCliente);

                claims.Add(new Claim(CustomClaims.NomePropriedade, cliente.NomePropriedade));
                claims.Add(new Claim(CustomClaims.NomeProprietario, cliente.Nome));

            }

            return claims;
        }

        private async Task<bool> ValidarDataValidadeLicenca(int idCliente)
        {
            var validaLicensa = await _clienteRepositorio.ObterDataValidadeLicenca(idCliente);

            if (validaLicensa.HasValue && DateTime.Compare(validaLicensa.Value, DateTime.Now.Date) < 0)
            {
                Notificar($"Licença expirou em {validaLicensa.Value.ToShortDateString()}");
                return false;
            }

            return true;
        }

        private async Task<bool> ValidarClienteDoUsuario(UsuarioResponseDTO usuario)
        {
            usuario.IdCliente = await _usuarioClienteRepositorio.ObterIdCliente(usuario.Id);
            if (usuario.IdCliente == 0)
            {
                Notificar("Credenciais Inválidas");
                return false;
            }

            return true;
        }

        public async Task<List<Claim>> SelecionarCliente(ClienteDTO cliente)
        {
            var claims = AppUser.ObterClaims().ToList();

            if (claims.Any(x => x.Type == CustomClaims.UsuarioCliente))
            {
                claims = claims
                            .Where(x => x.Type != CustomClaims.UsuarioCliente && x.Type != CustomClaims.NomePropriedade 
                                    && x.Type != CustomClaims.NomeProprietario).ToList();
            }

            claims.Add(new Claim(CustomClaims.UsuarioCliente, cliente.Id.ToString()));
            claims.Add(new Claim(CustomClaims.NomePropriedade, cliente.Propriedade));
            claims.Add(new Claim(CustomClaims.NomeProprietario, cliente.Nome));

            await _funcoesRepositorio.AtualizaGmd(cliente.Id);
            await _funcoesRepositorio.AtualizaLote(cliente.Id, 0, DateTime.Now);

            return await Task.FromResult(claims);
        }
    }
}
