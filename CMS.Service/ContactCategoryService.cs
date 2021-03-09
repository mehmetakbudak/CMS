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
        private readonly IUnitOfWork<CMSContext> unitOfWork;
        public ContactCategoryService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<ContactCategory> GetAll()
        {
            return unitOfWork.Repository<ContactCategory>()
                           .GetAll(x => !x.Deleted);
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
