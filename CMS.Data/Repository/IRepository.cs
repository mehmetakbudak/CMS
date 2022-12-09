using CMS.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CMS.Data.Repository
{
    public interface IRepository<T> where T : BaseEntityModel
    {
        Task Add(T entity);

        Task AddRange(List<T> entity);

        IQueryable<T> Where();

        IQueryable<T> Where(Expression<Func<T, bool>> predicate = null);

        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate = null);

        Task<bool> Any(Expression<Func<T, bool>> predicate = null);

        void Update(T entity);

        Task Delete(T entity);

        void DeleteRange(List<T> entity);

        Task Delete(int id);
    }
}
