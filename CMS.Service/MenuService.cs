using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Consts;
using CMS.Model.Dto;
using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface IMenuService
    {

        List<MenuModel> GetFrontendMenu(int? parentId = null, List<MenuModel> children = null);

        List<TreeDataModel> GetFrontendTreeMenu(int? parentId = null, List<TreeDataModel> children = null);

        ServiceResult PostFrontendMenu(TreeDataModel model);

        ServiceResult PutFrontendMenu(TreeDataModel model);
        
        ServiceResult DeleteFrontendMenu(int id);
    }

    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;
        private readonly IMemoryCache _memoryCache;

        public MenuService(IUnitOfWork<CMSContext> unitOfWork,
            IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }       

        public List<MenuModel> GetFrontendMenu(int? parentId = null, List<MenuModel> children = null)
        {
            var menuItems = new List<MenuModel>();

            var list = _unitOfWork.Repository<MenuItem>()
                .Where(x => x.Menu.Type == MenuType.FrontEnd && !x.Deleted && x.IsActive && x.ParentId == parentId)
                .Include(x => x.Menu)
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new MenuModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Url = x.Url
                }).ToList();

            menuItems.AddRange(list);

            foreach (var menuItem in list)
            {
                var items = GetFrontendMenu(menuItem.Id, list);
                if (items != null && items.Count > 0)
                {
                    menuItem.Items = items;
                }
            }
            return menuItems;
        }

        public List<TreeDataModel> GetFrontendTreeMenu(int? parentId = null, List<TreeDataModel> children = null)
        {
            var menuItems = new List<TreeDataModel>();

            var list = _unitOfWork.Repository<MenuItem>()
                .Where(x => x.Menu.Type == MenuType.FrontEnd && !x.Deleted && x.IsActive && x.ParentId == parentId)
                .Include(x => x.Menu)
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new TreeDataModel
                {                     
                    Title = x.Title,
                    Url = x.Url,
                    DisplayOrder = x.DisplayOrder,
                    ParentId = x.ParentId,
                    IsActive = x.IsActive
                }).ToList();

            menuItems.AddRange(list);

            foreach (var menuItem in list)
            {
                var items = GetFrontendTreeMenu(menuItem.Id, list);
                if (items != null && items.Count > 0)
                {
                    menuItem.Children = items;
                }
            }
            return menuItems;
        }

        public ServiceResult PostFrontendMenu(TreeDataModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var menu = _unitOfWork.Repository<Menu>().FirstOrDefault(x => x.Type == MenuType.FrontEnd);

            var menuItem = new MenuItem()
            {
                Deleted = false,
                DisplayOrder = model.DisplayOrder,
                IsActive = model.IsActive,
                MenuId = menu.Id,
                ParentId = model.ParentId,
                Title = model.Title,
                Url = model.Url
            };
            _unitOfWork.Repository<MenuItem>().Add(menuItem);
            _unitOfWork.Save();
            _memoryCache.Remove("frontEndMenu");
            result.Message = AlertMessages.Post;
            return result;
        }

        public ServiceResult PutFrontendMenu(TreeDataModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var menuItem = _unitOfWork.Repository<MenuItem>()
                .Where(x => !x.Deleted && x.Id == model.Id && x.Menu.Type == MenuType.FrontEnd)
                .Include(x => x.Menu)
                .FirstOrDefault();

            if (menuItem == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }
            menuItem.Title = model.Title;
            menuItem.DisplayOrder = model.DisplayOrder;
            menuItem.Url = model.Url;
            menuItem.IsActive = model.IsActive;
            menuItem.ParentId = model.ParentId;
            _unitOfWork.Save();
            _memoryCache.Remove("frontEndMenu");
            result.Message = AlertMessages.Put;
            return result;
        }

        public ServiceResult DeleteFrontendMenu(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var menuItem = _unitOfWork.Repository<MenuItem>()
                .Where(x => !x.Deleted && x.Id == id && x.Menu.Type == MenuType.FrontEnd)
                .Include(x => x.Menu)
                .FirstOrDefault();

            if (menuItem == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }
            menuItem.Deleted = true;
            _unitOfWork.Save();
            _memoryCache.Remove("frontEndMenu");
            result.Message = AlertMessages.Delete;

            return result;
        }
    }
}
