using CMS.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace CMS.Data.Repository
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        //bakılacak
        TContext Context { get; }
        IRepository<T> Repository<T>() where T : BaseModel;
        void CreateTransaction();
        void Commit();
        void Rollback();
        void Save();
    }
}
