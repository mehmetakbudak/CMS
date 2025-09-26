using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using CMS.Business.Helper;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CMS.Storage.Dtos.Lookup;

namespace CMS.Business.Services
{
    public interface ILookupService
    {
        Task<List<LookupDto>> ContactCategories();
        Task<List<LookupDto>> TaskCategories();
        Task<List<LookupDto>> TaskStatuses(int categoryId);
        Task<List<LookupDto>> Users();
        Task<List<LookupDto>> Menus();
        Task<List<LookupDto>> BlogCategories();
        List<LookupDto> MethodTypes();
        List<LookupDto> UserTypes();
        List<LookupDto> UserStatuses();
        List<LookupDto> WorkTypes();
        Task<List<LookupDto>> Roles();
        Task<List<LookupDto>> JobLocations();
        Task<List<LookupDto>> Tenants();
        Task<List<string>> Tags();
        List<LookupDto> CommentStatuses();
        List<LookupDto> MenuTypes();
    }

    public class LookupService(IUnitOfWork<CMSContext> unitOfWork) : ILookupService
    {
        public async Task<List<LookupDto>> ContactCategories()
        {
            return await unitOfWork.Repository<ContactCategory>()
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
        }

        public async Task<List<LookupDto>> TaskCategories()
        {
            return await unitOfWork.Repository<TaskCategory>()
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
        }

        public async Task<List<LookupDto>> TaskStatuses(int categoryId)
        {
            return await unitOfWork.Repository<Storage.Entity.TaskStatus>()
                .Where(x => !x.Deleted &&
                              x.IsActive &&
                              x.TaskCategoryId == categoryId)
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new LookupDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
        }

        public async Task<List<LookupDto>> Users()
        {
            return await unitOfWork.Repository<User>()
                .Where(x => !x.Deleted && x.IsActive && x.UserType != UserType.Member)
                .Select(x => new LookupDto
                {
                    Id = x.Id,
                    Name = x.Name + " " + x.Surname
                }).ToListAsync();
        }

        public async Task<List<LookupDto>> Menus()
        {
            var list = await unitOfWork.Repository<Menu>()
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
            return list;
        }

        public async Task<List<LookupDto>> BlogCategories()
        {
            return await unitOfWork.Repository<BlogCategory>()
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupDto()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
        }

        public List<LookupDto> MethodTypes()
        {
            var list = EnumHelper.GetEnumLookup(typeof(MethodType));
            return list;
        }

        public List<LookupDto> UserTypes()
        {
            var list = EnumHelper.GetEnumLookup(typeof(UserType));
            return list;
        }

        public List<LookupDto> UserStatuses()
        {
            var list = EnumHelper.GetEnumLookup(typeof(UserStatus));
            return list;
        }

        public List<LookupDto> WorkTypes()
        {
            var list = EnumHelper.GetEnumLookup(typeof(WorkType));
            return list;
        }

        public async Task<List<LookupDto>> Roles()
        {
            var list = await unitOfWork.Repository<Role>()
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
            return list;
        }

        public async Task<List<LookupDto>> JobLocations()
        {
            var list = await unitOfWork.Repository<JobLocation>()
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
            return list;
        }

        public async Task<List<string>> Tags()
        {
            var list = await unitOfWork.Repository<Tag>()
                            .Where()
                            .Select(x => x.Name).ToListAsync();
            return list;
        }

        public async Task<List<LookupDto>> Tenants()
        {
            var list = await unitOfWork.Repository<Tenant>()
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
            return list;
        }

        public List<LookupDto> CommentStatuses()
        {
            var list = EnumHelper.GetEnumLookup(typeof(CommentStatus));
            return list;
        }

        public List<LookupDto> MenuTypes()
        {
            var list = EnumHelper.GetEnumLookup(typeof(MenuType));
            return list;
        }
    }
}
