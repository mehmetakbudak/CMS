using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using CMS.Storage.Model;
using CMS.Service.Helper;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface ILookupService
    {
        Task<List<LookupModel>> ContactCategories();
        Task<List<LookupModel>> TaskCategories();
        Task<List<LookupModel>> TaskStatuses(int categoryId);
        Task<List<LookupModel>> Users();
        Task<List<LookupModel>> Menus();
        Task<List<LookupModel>> BlogCategories();
        List<LookupModel> MethodTypes();
        List<LookupModel> UserTypes();
    }

    public class LookupService : ILookupService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public LookupService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LookupModel>> ContactCategories()
        {
            return await _unitOfWork.Repository<ContactCategory>()
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
        }

        public async Task<List<LookupModel>> TaskCategories()
        {
            return await _unitOfWork.Repository<TaskCategory>()
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
        }

        public async Task<List<LookupModel>> TaskStatuses(int categoryId)
        {
            return await _unitOfWork.Repository<TaskStatusDmo>()
                .Where(x => !x.Deleted &&
                              x.IsActive &&
                              x.TaskCategoryId == categoryId)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
        }

        public async Task<List<LookupModel>> Users()
        {
            return await _unitOfWork.Repository<User>()
                .Where(x => !x.Deleted && x.IsActive && x.UserType != UserType.Member)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name + " " + x.Surname
                }).ToListAsync();
        }

        public async Task<List<LookupModel>> Menus()
        {
            var list = await _unitOfWork.Repository<Menu>()
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
            return list;
        }

        public async Task<List<LookupModel>> BlogCategories()
        {
            return await _unitOfWork.Repository<BlogCategory>()
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupModel()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
        }

        public List<LookupModel> MethodTypes()
        {
            var list = EnumHelper.GetEnumLookup((typeof(MethodType)));
            return list;
        }

        public List<LookupModel> UserTypes()
        {
            var list = EnumHelper.GetEnumLookup((typeof(UserType)));
            return list;
        }
    }
}
