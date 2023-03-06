using CMS.Storage.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace CMS.Data.Repository
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        TContext Context { get; }
        IRepository<T> Repository<T>() where T : BaseEntityModel;
        IExecutionStrategy CreateExecutionStrategy();
        Task CreateTransaction();
        Task Commit();
        Task Rollback();
        Task<int> Save();
    }
}
