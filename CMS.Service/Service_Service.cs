using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Model;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Service
{
    public interface IService_Service
    {
        IQueryable<Model.Entity.Service> GetAll();
        List<ServiceModel> GetAllActive();
        Model.Entity.Service GetByUrl(string url);
    }

    public class Service_Service : IService_Service
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public Service_Service(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Model.Entity.Service> GetAll()
        {
            var list = _unitOfWork.Repository<Model.Entity.Service>()
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.Id).AsQueryable();

            return list;
        }

        public List<ServiceModel> GetAllActive()
        {
            return GetAll().Where(x => x.IsActive)
                .Select(x => new ServiceModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageUrl = x.ImageUrl,
                    Url = x.Url,
                    Content = x.Content
                }).ToList();
        }

        public Model.Entity.Service GetByUrl(string url)
        {
            return _unitOfWork.Repository<Model.Entity.Service>()
                .FirstOrDefault(x => !x.Deleted && x.Url == url);
        }
    }
}
