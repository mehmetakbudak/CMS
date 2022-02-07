using CMS.Model.Entity;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CMS.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : BaseEntityModel
    {
        void Add(TEntity entity);

        void AddRange(List<TEntity> entity);
        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        TEntity Find(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void Delete(int id);
    }
}
