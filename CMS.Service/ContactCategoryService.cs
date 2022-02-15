using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Model;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Service
{
    public interface IContactCategoryService
    {
        IQueryable<ContactCategory> GetAll();
        List<LookupModel> Lookup();
    }

    public class ContactCategoryService : IContactCategoryService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public ContactCategoryService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<ContactCategory> GetAll()
        {
            return _unitOfWork.Repository<ContactCategory>()
                .Where(x => !x.Deleted);
        }

        public List<LookupModel> Lookup()
        {
            return GetAll().Where(x => x.IsActive)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
        }
    }
}
