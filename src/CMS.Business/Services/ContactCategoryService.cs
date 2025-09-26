using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Entity;
using System.Linq;

namespace CMS.Business.Services
{
    public interface IContactCategoryService
    {
        IQueryable<ContactCategory> GetAll();
    }

    public class ContactCategoryService(IUnitOfWork<CMSContext> unitOfWork) : IContactCategoryService
    {
        public IQueryable<ContactCategory> GetAll()
        {
            return unitOfWork.Repository<ContactCategory>().Where(x => !x.Deleted);
        }
    }
}
