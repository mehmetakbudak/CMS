using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Entity;
using System.Linq;

namespace CMS.Service
{
    public interface IContactCategoryService
    {
        IQueryable<ContactCategory> GetAll();
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
            return _unitOfWork.Repository<ContactCategory>().Where(x => !x.Deleted);
        }
    }
}
