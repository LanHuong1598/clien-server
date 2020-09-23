using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using BELibrary.Core.Entity.Repositories;
using BELibrary.DbContext;
using BELibrary.Utils;
using Z.EntityFramework.Plus;

namespace BELibrary.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly PatientManagementDbContext Context;

        public Repository(PatientManagementDbContext context)
        {
            Context = context;
        }

        public TEntity Get(object id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            var entities = Context.Set<TEntity>().ToList();
            return entities;
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Put(TEntity entity, object id)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            var exist = Context.Set<TEntity>().Find(id);
            if (exist != null)
            {
                Context.Entry(exist).CurrentValues.SetValues(entity);
            }
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public IEnumerable<TEntity> Include(params Expression<Func<TEntity, object>>[] includes)
        {
            IDbSet<TEntity> dbSet = Context.Set<TEntity>();

            IEnumerable<TEntity> query = null;
            foreach (var include in includes)
            {
                query = dbSet.Include(include);
            }

            return query ?? dbSet;
        }

        public IEnumerable<TEntity> IncludeFilter(params Expression<Func<TEntity, object>>[] includes)
        {
            IDbSet<TEntity> dbSet = Context.Set<TEntity>();

            IEnumerable<TEntity> query = null;
            foreach (var include in includes)
            {
                query = dbSet.IncludeFilter(include);
            }

            return query ?? dbSet;
        }

        public IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>().Where(filter);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>().FirstOrDefault(filter);
        }

        public IEnumerable<TEntity> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return Context.Database.SqlQuery<TEntity>(query, parameters);
        }

        public void Del(object id, Guid userId)
        {
            var entity = Context.Set<TEntity>().Find(id);
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            Context.SaveChanges();
        }
    }
}