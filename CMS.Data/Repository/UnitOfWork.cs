using CMS.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace CMS.Data.Repository
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable where TContext : DbContext
    {

        private readonly TContext _context;
        private bool _disposed;
        private string _errorMessage = string.Empty;
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

        public void Commit()
        {
            _objTran.Commit();
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

        public void Save()
        {
            _context.SaveChanges();
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
