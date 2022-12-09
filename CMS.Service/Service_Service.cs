using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IService_Service
    {
        IQueryable<Storage.Entity.Service> GetAll();
        Task<List<ServiceModel>> GetAllActive();
        Task<Storage.Entity.Service> GetByUrl(string url);
    }

    public class Service_Service : IService_Service
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public Service_Service(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Storage.Entity.Service> GetAll()
        {
            var list = _unitOfWork.Repository<Storage.Entity.Service>()
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.Id).AsQueryable();

            return list;
        }

        public async Task<List<ServiceModel>> GetAllActive()
        {
            return await GetAll()
                .Where(x => x.IsActive)
                .Select(x => new ServiceModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageUrl = x.ImageUrl,
                    Url = x.Url,
                    Content = x.Content
                }).ToListAsync();
        }

        public async Task<Storage.Entity.Service> GetByUrl(string url)
        {
            return await _unitOfWork.Repository<Storage.Entity.Service>()
                .FirstOrDefault(x => !x.Deleted && x.Url == url);
        }
    }
}
