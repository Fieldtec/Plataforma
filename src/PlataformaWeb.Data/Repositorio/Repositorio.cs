using Microsoft.EntityFrameworkCore;
using PlataformaWeb.Business.Enums;
using PlataformaWeb.Business.Extensions;
using PlataformaWeb.Business.Interfaces;
using PlataformaWeb.Business.Interfaces.Repositorios;
using PlataformaWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaWeb.Data.Repositorio
{
    public class Repositorio<TEntity> : IRepositorio<TEntity> where TEntity : Entity, new()
    {
        protected PlataformaFieldContext Context { get; set; }
        protected DbSet<TEntity> DbSet { get; set; }
        protected IUser AppUser { get; set; }

        public Repositorio(PlataformaFieldContext context, IUser appUser)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
            AppUser = appUser;
        }

        public IDbConnection DbConnection => Context.Database.GetDbConnection();

        public IUnitOfWork UnitOfWork => Context;

        public virtual async Task Adicionar(TEntity entity)
        {
            await Task.FromResult(DbSet.Add(entity));
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            await Task.FromResult(DbSet.Update(entity));
        }

        public virtual async Task<List<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate.And(x => x.Status == Status.Ativado)).ToListAsync();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public virtual async Task<TEntity> ObterPorId(int id)
        {
            return await DbSet.SingleOrDefaultAsync(x => x.Id == id && x.Status == Status.Ativado);
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await DbSet.Where(x => x.Status == Status.Ativado).ToListAsync();
        }

        public virtual Expression<Func<TEntity, bool>> ObterWhere()
        {
            var where = PredicateBuilder.True<TEntity>().And(x => x.Status == Status.Ativado);            

            return where;
        }

        public virtual async Task Remover(TEntity entity)
        {
            entity.Status = Status.Desativado;
            await Task.FromResult(DbSet.Update(entity));
        }

        public bool ExisteNoChangeTracker<TEntityChange>(TEntityChange entity) where TEntityChange : Entity
        {
            if (Context.ChangeTracker.Entries()
                        .Any(x => x.Entity.GetType() == entity.GetType() 
                                    && x.Property("Id").CurrentValue.ToString() == entity.Id.ToString()))
                return true;

            return false;
        }
    }
}
