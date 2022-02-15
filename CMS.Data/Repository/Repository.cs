using CMS.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CMS.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntityModel
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        private bool _isDisposed;

        public Repository(IUnitOfWork<DbContext> unitOfWork) : this(unitOfWork.Context) { }

        public Repository(DbContext context)
        {
            _isDisposed = false;
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void AddRange(List<T> entity)
        {
            _dbSet.AddRange(entity);
        }

        public virtual void Delete(T entity)
        {
            entity = FirstOrDefault(x => x.Id == entity.Id);

            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public virtual void Delete(int id)
        {
            var entity = FirstOrDefault(x => x.Id == id);

            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public virtual IQueryable<T> Where()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query;
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                return query.FirstOrDefault(predicate);
            }

            return query.FirstOrDefault();
        }

        public bool Any(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                return query.Any(predicate);
            }

            return query.Any();
        }

        public virtual void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity is null");
            }

            _dbSet.Update(entity);
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
            _isDisposed = true;
        }
    }
}
