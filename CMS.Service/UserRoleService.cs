using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CMS.Service
{
    public interface IUserUserGroupService
    {
        IQueryable<UserRole> GetAll();
    }
    public class UserRoleService : IUserUserGroupService
    {
        private readonly IUnitOfWork<CMSContext> unitOfWork;
        public UserRoleService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IQueryable<UserRole> GetAll()
        {
            return this.unitOfWork
                .Repository<UserRole>()
                .GetAll(null, x => x
                .Include(o => o.User)
                .ThenInclude(o => o.UserAccessRights)
                .ThenInclude(o => o.AccessRight)
                .Include(o => o.Role)
                .ThenInclude(o => o.RoleAccessRights)
                .ThenInclude(o => o.AccessRight)
                .ThenInclude(o => o.AccessRightCategory));
        }
    }
}
