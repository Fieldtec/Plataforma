using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Business.Interfaces.Repositorios
{
    public interface IRepositorio<TEntity> : IDisposable where TEntity : Entity
    {
        IDbConnection DbConnection { get; }
        IUnitOfWork UnitOfWork { get; }
        Task Adicionar(TEntity entity);
        Task<TEntity> ObterPorId(int id);
        Task<List<TEntity>> ObterTodos();
        Task Atualizar(TEntity entity);
        Task Remover(TEntity id);
        Task<List<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);        
        Expression<Func<TEntity, bool>> ObterWhere();
        bool ExisteNoChangeTracker<TEntityChange>(TEntityChange entity) where TEntityChange : Entity;
    }
}
