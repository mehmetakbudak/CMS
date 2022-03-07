using CMS.Model.Entity;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CMS.Data.Repository
{
    public interface IRepository<T> where T : BaseEntityModel
    {
        void Add(T entity);

        void AddRange(List<T> entity);

        IQueryable<T> Where();

        IQueryable<T> Where(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        T FirstOrDefault(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        bool Any(Expression<Func<T, bool>> predicate = null);

        void Update(T entity);

        void Delete(T entity);

        void DeleteRange(List<T> entity);

        void Delete(int id);
    }
}
