using CMS.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace CMS.Data.Repository
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable where TContext : DbContext
    {

        private readonly TContext _context;
        private bool _disposed;
        private IDbContextTransaction _objTran;

        public TContext Context
        {
            get { return _context; }
        }

        public UnitOfWork(TContext context)
        {
            // dispose edildiği için  NEW TContext() kullanılabilir;
            _context = context;
        }

        public void CreateTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void Rollback()
        {
            _objTran.Rollback();
            _objTran.Dispose();
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        public void Commit()
        {
            _objTran.Commit();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    _context.Dispose();
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<T> Repository<T>() where T : BaseEntityModel
        {
            return new Repository<T>(_context);
        }


    }
}
