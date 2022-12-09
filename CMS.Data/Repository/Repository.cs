using CMS.Storage.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

        public virtual async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task AddRange(List<T> entity)
        {
            await _dbSet.AddRangeAsync(entity);
        }

        public virtual async Task Delete(T entity)
        {
            entity = await FirstOrDefault(x => x.Id == entity.Id);

            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public virtual async Task Delete(int id)
        {
            var entity = await FirstOrDefault(x => x.Id == id);

            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public virtual IQueryable<T> Where()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query;
        }

        public virtual async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                return await query.FirstOrDefaultAsync(predicate);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> Any(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                return await query.AnyAsync(predicate);
            }

            return await query.AnyAsync();
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

        public void DeleteRange(List<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }
    }
}
