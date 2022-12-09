using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Model;
using CMS.Service.Helper;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Service
{
    public interface ILookupService
    {
        List<LookupModel> ContactCategories();
        List<LookupModel> TodoCategories();
        List<LookupModel> TodoStatuses(int categoryId);
        List<LookupModel> Users();
        List<LookupModel> Menus();
        List<LookupModel> BlogCategories();
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

        public List<LookupModel> ContactCategories()
        {
            return _unitOfWork.Repository<ContactCategory>()
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
        }

        public List<LookupModel> TodoCategories()
        {
            return _unitOfWork.Repository<TodoCategory>()
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
        }

        public List<LookupModel> TodoStatuses(int categoryId)
        {
            return _unitOfWork.Repository<TodoStatus>()
                .Where(x => !x.Deleted &&
                              x.IsActive &&
                              x.TodoCategoryId == categoryId)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
        }

        public List<LookupModel> Users()
        {
            return _unitOfWork.Repository<User>()
                .Where(x => !x.Deleted && x.IsActive && x.UserType != UserType.Member)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name + " " + x.Surname
                }).ToList();
        }

        public List<LookupModel> Menus()
        {
            var list = _unitOfWork.Repository<Menu>()
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            return list;
        }

        public List<LookupModel> BlogCategories()
        {
            return _unitOfWork.Repository<BlogCategory>()
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupModel()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
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
