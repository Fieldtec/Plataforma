using FluentValidation;
using FluentValidation.Results;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Models;
using PlataformaWeb.Business.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Services
{

    public interface IBaseService<TEntity, TDto> : IDisposable where TEntity : Entity
    {
        Task Adicionar(TEntity entity);
        Task<TEntity> ObterPorId(int id);
        Task<List<TDto>> ObterPaginacao(int? id = null);
        Task Atualizar(TEntity entity);
        Task Remover(int id);
    }

    public interface IBaseService<TEntity> : IDisposable where TEntity : Entity
    {
        Task Adicionar(TEntity entity);
        Task<TEntity> ObterPorId(int id);
        Task<List<TEntity>> ObterTodos();
        Task Atualizar(TEntity entity);
        Task Remover(int id);
        Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);
    }

    public abstract class Service
    {
        private readonly INotificador _notificador;
        protected readonly IUser AppUser;

        protected Service(INotificador notificador, IUser appUser)
        {
            _notificador = notificador;
            AppUser = appUser;
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }

        protected bool TemNotificacao()
        {
            return _notificador.TemNotificacao();
        }

        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            Notificar(validator);

            return false;
        }

        protected virtual bool ValidaInsercaoAtualizacaoCliente<T>(T model) where T : class
        {
            if (model is null)
            {
                Notificar("O Cliente não atende ao registro informado");
                return false;
            }

            if (AppUser.EhAdmin())
            {
                Notificar("Perfil ADM não pode inserir informações do Cliente");
                return false;
            }

            if (model.GetType().GetProperty("IdCliente") == null)
            {
                Notificar("Não é possível fazer a validação do Cliente");
                return false;
            }

            if (AppUser.ObterIdCliente() == 0)
            {
                Notificar("É necessário informar o Cliente");
                return false;
            }

            model.GetType().GetProperty("IdCliente").SetValue(model, AppUser.ObterIdCliente());

            return true;
        }

        protected StringContent ObterConteudo(object dado)
        {
            return new StringContent(
                JsonSerializer.Serialize(dado),
                Encoding.UTF8,
                "application/json");
        }

        protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false), options);
        }

        protected T DeserializarObjetoResponse<T>(string responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(responseMessage, options);
        }

    }
}
