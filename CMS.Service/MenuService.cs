using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Consts;
using CMS.Storage.Dto;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using CMS.Storage.Model;
using CMS.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IMenuService
    {

        Task<List<MenuModel>> GetFrontendMenu(int? parentId = null, List<MenuModel> children = null);
        Task<List<TreeDataModel>> GetFrontendTreeMenu(int? parentId = null, List<TreeDataModel> children = null);
        Task<ServiceResult> PostFrontendMenu(TreeDataModel model);
        Task<ServiceResult> PutFrontendMenu(TreeDataModel model);
        Task<ServiceResult> DeleteFrontendMenu(int id);
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

        public async Task<List<MenuModel>> GetFrontendMenu(int? parentId = null, List<MenuModel> children = null)
        {
            var menuItems = new List<MenuModel>();

            var list = await _unitOfWork.Repository<MenuItem>()
                .Where(x => x.Menu.Type == MenuType.FrontEnd && !x.Deleted && x.IsActive && x.ParentId == parentId)
                .Include(x => x.Menu)
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new MenuModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Url = x.Url
                }).ToListAsync();

            menuItems.AddRange(list);

            foreach (var menuItem in list)
            {
                var items = await GetFrontendMenu(menuItem.Id, list);
                if (items != null && items.Count > 0)
                {
                    menuItem.Items = items;
                }
            }
            return menuItems;
        }

        public async Task<List<TreeDataModel>> GetFrontendTreeMenu(int? parentId = null, List<TreeDataModel> children = null)
        {
            var menuItems = new List<TreeDataModel>();

            var list = await _unitOfWork.Repository<MenuItem>()
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
                }).ToListAsync();

            menuItems.AddRange(list);

            foreach (var menuItem in list)
            {
                var items = await GetFrontendTreeMenu(menuItem.Id, list);
                if (items != null && items.Count > 0)
                {
                    menuItem.Children = items;
                }
            }
            return menuItems;
        }

        public async Task<ServiceResult> PostFrontendMenu(TreeDataModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var menu = await _unitOfWork.Repository<Menu>()
                .FirstOrDefault(x => x.Type == MenuType.FrontEnd);

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

            await _unitOfWork.Repository<MenuItem>().Add(menuItem);

            await _unitOfWork.Save();

            _memoryCache.Remove("frontEndMenu");

            result.Message = AlertMessages.Post;

            return result;
        }

        public async Task<ServiceResult> PutFrontendMenu(TreeDataModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var menuItem = await _unitOfWork.Repository<MenuItem>()
                .Where(x => !x.Deleted && x.Id == model.Id && x.Menu.Type == MenuType.FrontEnd)
                .Include(x => x.Menu)
                .FirstOrDefaultAsync();

            if (menuItem == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            menuItem.Title = model.Title;
            menuItem.DisplayOrder = model.DisplayOrder;
            menuItem.Url = model.Url;
            menuItem.IsActive = model.IsActive;
            menuItem.ParentId = model.ParentId;
            
            await _unitOfWork.Save();
            
            _memoryCache.Remove("frontEndMenu");
            
            result.Message = AlertMessages.Put;
            
            return result;
        }

        public async Task<ServiceResult> DeleteFrontendMenu(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var menuItem = await _unitOfWork.Repository<MenuItem>()
                .Where(x => !x.Deleted && x.Id == id && x.Menu.Type == MenuType.FrontEnd)
                .Include(x => x.Menu)
                .FirstOrDefaultAsync();

            if (menuItem == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            menuItem.Deleted = true;
            
            await _unitOfWork.Save();
            
            _memoryCache.Remove("frontEndMenu");
            
            result.Message = AlertMessages.Delete;

            return result;
        }
    }
}
