using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CMS.Service
{
    public interface IUserAccessRightService
    {
        IQueryable<UserAccessRight> GetAll();
    }
    public class UserAccessRightService : IUserAccessRightService
    {
        private readonly IUnitOfWork<CMSContext> unitOfWork;
        public UserAccessRightService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IQueryable<UserAccessRight> GetAll()
        {
            return unitOfWork.Repository<UserAccessRight>()
                .GetAll(null, x=> x
                .Include(o=>o.AccessRight)
                .Include(o=>o.User));
        }
    }
}
