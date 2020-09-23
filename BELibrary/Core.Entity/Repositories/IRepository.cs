using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BELibrary.Core.Entity.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(object id);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Put(TEntity entity, object id);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        void Del(object id, Guid userId);

        IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter);

        IEnumerable<TEntity> Include(params Expression<Func<TEntity, object>>[] includes);

        IEnumerable<TEntity> IncludeFilter(params Expression<Func<TEntity, object>>[] includes);
    }
}