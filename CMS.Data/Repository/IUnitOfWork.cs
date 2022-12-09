using CMS.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CMS.Data.Repository
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        TContext Context { get; }
        IRepository<T> Repository<T>() where T : BaseEntityModel;
        void CreateTransaction();
        void Commit();
        void Rollback();
        Task<int> Save();
    }
}
