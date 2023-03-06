using CMS.Storage.Entity;
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

        public IExecutionStrategy CreateExecutionStrategy()
        {
            return _context.Database.CreateExecutionStrategy();
        }

        public async Task CreateTransaction()
        {
            if (_objTran == null)
            {
                _objTran = await _context.Database.BeginTransactionAsync();
            }
        }

        public async Task Rollback()
        {
            await _objTran.RollbackAsync();
            await _objTran.DisposeAsync();
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task Commit()
        {
            await _objTran.CommitAsync();
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
