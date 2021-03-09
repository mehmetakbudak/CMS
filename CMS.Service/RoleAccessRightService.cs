using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CMS.Service
{
    public interface IUserGroupAccessRightService
    {
        IQueryable<RoleAccessRight> GetAll();
    }

    public class RoleAccessRightService : IUserGroupAccessRightService
    {
        private readonly IUnitOfWork<CMSContext> unitOfWork;
        public RoleAccessRightService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<RoleAccessRight> GetAll()
        {
            return unitOfWork.Repository<RoleAccessRight>()
                .GetAll(null, x => x
                .Include(o => o.AccessRight)
                .Include(o => o.Role));

        }
    }
}
