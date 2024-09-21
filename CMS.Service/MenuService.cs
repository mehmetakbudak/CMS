using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Service.Helper;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using CMS.Storage.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IMenuService
    {
        Task<List<MenuItemGetModel>> GetFrontendMenu();
        Task<List<MenuItemGetModel>> GetAdminMenu();
        Task<List<MenuItemGetModel>> GetUserAdminMenu();
        Task<List<MenuItemTreeModel>> GetUserAdminTreeMenu(List<MenuItemGetModel> data = null, int? parentId = null, List<MenuItemTreeModel> children = null);
        Task<List<TreeDataModel>> GetFrontendTreeMenu(int? parentId = null, List<TreeDataModel> children = null);
    }

    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MenuService(
            IUnitOfWork<CMSContext> unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<MenuItemGetModel>> GetAdminMenu()
        {
            return await _unitOfWork.Repository<Menu>()
                .Where(x => x.Type == MenuType.Admin)
                .SelectMany(x => x.MenuItems)
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new MenuItemGetModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    ParentId = x.ParentId,
                    Url = x.Url
                }).ToListAsync();
        }

        public async Task<List<MenuItemTreeModel>> GetUserAdminTreeMenu(List<MenuItemGetModel> data = null, int? parentId = null, List<MenuItemTreeModel> children = null)
        {
            var menuItems = new List<MenuItemTreeModel>();

            var allList = await GetUserAdminMenu();

            var list = allList.Where(x => x.ParentId == parentId)
                .Select(x => new MenuItemTreeModel
                {
                    Id = x.Id,
                    Label = x.Title,
                    Url = x.Url,
                }).ToList();

            menuItems.AddRange(list);

            foreach (var menuItem in list)
            {
                var items = await GetUserAdminTreeMenu(allList, menuItem.Id, list);
                if (items != null && items.Count > 0)
                {
                    menuItem.Children = items;
                }
            }
            return menuItems;
        }

        public async Task<List<MenuItemGetModel>> GetUserAdminMenu()
        {
            var list = await GetAdminMenu();

            var loginUser = _httpContextAccessor.HttpContext.User.Parse();

            if (loginUser.UserType == (int)UserType.Admin)
            {
                var roleAccessRightIds = _unitOfWork.Repository<UserRole>()
                    .Where(x => x.UserId == loginUser.UserId)
                    .Include(x => x.Role.RoleAccessRights)
                    .SelectMany(x => x.Role.RoleAccessRights)
                    .Select(x => x.AccessRightId).ToList();

                var menuItemIds = _unitOfWork.Repository<MenuItemAccessRight>()
                    .Where(x => x.MenuItem.Menu.Type == MenuType.Admin && roleAccessRightIds.Contains(x.AccessRightId))
                    .Select(x => x.MenuItemId).ToList();

                var menuItemsWithAccessRights = _unitOfWork.Repository<MenuItemAccessRight>()
                    .Where(x => x.MenuItem.Menu.Type == MenuType.Admin)
                    .Select(x => x.MenuItemId).ToList();

                var accessRightMenuItems = list.Where(x => menuItemIds.Contains(x.Id)).ToList();
                var withoutAccessRightMenuItems = list.Where(x => !menuItemsWithAccessRights.Contains(x.Id)).ToList();

                var adminList = new List<MenuItemGetModel>();
                adminList.AddRange(accessRightMenuItems);
                adminList.AddRange(withoutAccessRightMenuItems);

                var mainMenuItemList = adminList.Where(x => string.IsNullOrEmpty(x.Url)).ToList();

                if (mainMenuItemList.Count > 0)
                {
                    foreach (var menuItem in mainMenuItemList)
                    {
                        var isCheck = adminList.Any(x => x.ParentId == menuItem.Id);

                        if (!isCheck)
                        {
                            adminList.Remove(menuItem);
                        }
                    }
                }

                return adminList;
            }

            return list;
        }

        public async Task<List<MenuItemGetModel>> GetFrontendMenu()
        {
            return await _unitOfWork.Repository<Menu>()
                .Where(x => x.Type == MenuType.FrontEnd)
                .SelectMany(x => x.MenuItems)
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new MenuItemGetModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    ParentId = x.ParentId,
                    Url = x.Url
                }).ToListAsync();
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
                    Id = x.Id,
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


    }
}
