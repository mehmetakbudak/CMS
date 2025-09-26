using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using CMS.Storage.Dtos.Menu;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Business.Extensions;
using CMS.Storage.Dtos.Response;
using CMS.Business.Exceptions;

namespace CMS.Business.Services
{
    public interface IMenuService
    {
        IQueryable<MenuDto> Get();
        Task<MenuDto> GetById(int id);
        Task<List<MenubarDto>> GetFrontendMenu(int? parentId = null, List<MenubarDto> children = null);
        Task<List<TreeMenuDto>> GetFrontendTreeMenu(int? parentId = null, List<TreeMenuDto> children = null);
        Task<List<MenuItemListDto>> GetAdminMenu();
        Task<List<MenuItemTreeDto>> GetUserAdminMenu();
        Task<ServiceResult> Create(MenuDto dto);
        Task<ServiceResult> Update(MenuDto dto);
        Task<ServiceResult> Delete(int id);
    }

    public class MenuService(
            IUnitOfWork<CMSContext> unitOfWork,
            IHttpContextAccessor httpContextAccessor) : IMenuService
    {
        public async Task<List<MenuItemListDto>> GetAdminMenu()
        {
            return await unitOfWork.Repository<Menu>()
                .Where(x => x.Type == MenuType.Admin)
                .SelectMany(x => x.MenuItems)
                .Where(x => !x.Deleted && x.IsActive)
                .Select(x => new MenuItemListDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ParentId = x.ParentId,
                    Url = x.Url
                }).ToListAsync();
        }

        public async Task<List<MenuItemTreeDto>> GetUserAdminTreeMenu(List<MenuItemListDto> data = null, int? parentId = null, List<MenuItemTreeDto> children = null)
        {
            var menuItems = new List<MenuItemTreeDto>();

            var list = data.Where(x => x.ParentId == parentId)
                .Select(x => new MenuItemTreeDto
                {
                    Id = x.Id,
                    Label = x.Title,
                    Url = x.Url,
                }).ToList();

            menuItems.AddRange(list);

            foreach (var menuItem in list)
            {
                var items = await GetUserAdminTreeMenu(data, menuItem.Id, list);
                if (items != null && items.Count > 0)
                {
                    menuItem.Children = items;
                }
            }
            return menuItems;
        }

        public async Task<List<MenuItemTreeDto>> GetUserAdminMenu()
        {
            var list = new List<MenuItemTreeDto>();
            var allList = await GetAdminMenu();

            var loginUser = httpContextAccessor.HttpContext.User.Parse();

            if (loginUser.UserType == (int)UserType.SuperAdmin)
            {
                list = await GetUserAdminTreeMenu(allList);
            }
            else if (loginUser.UserType == (int)UserType.Admin)
            {
                var roleAccessRightIds = unitOfWork.Repository<UserRole>()
                    .Where(x => x.UserId == loginUser.UserId)
                    .Include(x => x.Role.RoleAccessRights)
                    .AsNoTracking()
                    .SelectMany(x => x.Role.RoleAccessRights)
                    .Select(x => x.AccessRightId).ToList();

                var menuItemIds = unitOfWork.Repository<MenuItemAccessRight>()
                    .Where(x => x.MenuItem.Menu.Type == MenuType.Admin && roleAccessRightIds.Contains(x.AccessRightId))
                    .AsNoTracking()
                    .Select(x => x.MenuItemId).ToList();

                var menuItemList = allList.Where(x => menuItemIds.Contains(x.Id)).ToList();

                var items = new List<MenuItemListDto>();

                foreach (var item in menuItemList)
                {
                    var menuItems = GetParentList(allList, item.Id);
                    foreach (var menuItem in menuItems)
                    {
                        if (!items.Any(x => x.Id == menuItem.Id))
                        {
                            items.Add(menuItem);
                        }
                    }
                }
                list = await GetUserAdminTreeMenu(items);                
                return list;
            }
            return list;
        }

        private List<MenuItemListDto> GetParentList(List<MenuItemListDto> list, int id)
        {
            var result = new List<MenuItemListDto>();
            var current = list.FirstOrDefault(x => x.Id == id);

            while (current != null)
            {
                result.Insert(0, current);
                if (current.ParentId is null)
                    break;
                current = list.FirstOrDefault(x => x.Id == current.ParentId);
            }
            return result;
        }

        public async Task<List<MenubarDto>> GetFrontendMenu(int? parentId = null, List<MenubarDto> children = null)
        {
            var menuItems = new List<MenubarDto>();

            var list = await unitOfWork.Repository<MenuItem>()
                .Where(x => x.Menu.Type == MenuType.FrontEnd && !x.Deleted && x.IsActive && x.ParentId == parentId)
                .Include(x => x.Menu)
                .AsNoTracking()
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new MenubarDto
                {
                    Id = x.Id,
                    Label = x.Title,
                    Route = x.Url,
                    DisplayOrder = x.DisplayOrder,
                    ParentId = x.ParentId,
                    IsActive = x.IsActive
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

        public async Task<List<TreeMenuDto>> GetFrontendTreeMenu(int? parentId = null, List<TreeMenuDto> children = null)
        {
            var menuItems = new List<TreeMenuDto>();

            var list = await unitOfWork.Repository<MenuItem>()
                .Where(x => x.Menu.Type == MenuType.FrontEnd && !x.Deleted && x.IsActive && x.ParentId == parentId)
                .Include(x => x.Menu)
                .AsNoTracking()
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new TreeMenuDto
                {
                    Id = x.Id,
                    Label = x.Title,
                    Route = x.Url,
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

        public IQueryable<MenuDto> Get()
        {
            return unitOfWork.Repository<Menu>()
                .Where(x => !x.Deleted)
                .AsNoTracking()
                .Select(x => new MenuDto
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    IsDeletable = x.IsDeletable,
                    Name = x.Name,
                    Type = x.Type
                }).AsQueryable();
        }

        public Task<MenuDto> GetById(int id)
        {
            var result = unitOfWork.Repository<Menu>()
                  .Where(x => !x.Deleted && x.Id == id)
                  .AsNoTracking()
                  .Select(x => new MenuDto
                  {
                      Id = x.Id,
                      IsActive = x.IsActive,
                      IsDeletable = x.IsDeletable,
                      Name = x.Name,
                      Type = x.Type
                  }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<ServiceResult> Create(MenuDto dto)
        {
            if (dto.Type != MenuType.Custom)
            {
                var isExist = await unitOfWork.Repository<Menu>()
                    .Any(x => !x.Deleted && x.Type == dto.Type);

                if (isExist)
                    throw new FoundException("Menu.AlreadyExistMenuType");
            }
            var menu = new Menu
            {
                Name = dto.Name,
                Type = dto.Type,
                IsActive = dto.IsActive,
                IsDeletable = dto.Type != MenuType.Custom ? false : true,
                Deleted = false
            };
            await unitOfWork.Repository<Menu>().Add(menu);
            await unitOfWork.Save();

            return ServiceResult.Success(200, menu);
        }

        public async Task<ServiceResult> Update(MenuDto dto)
        {
            if (dto.Type != MenuType.Custom)
            {
                var isExist = await unitOfWork.Repository<Menu>()
                    .Any(x => !x.Deleted && x.Id != dto.Id && x.Type == dto.Type);

                if (isExist)
                    throw new FoundException("Menu.AlreadyExistMenuType");
            }
            if (dto.Type != MenuType.Custom)
                throw new BadRequestException("Menu.NotEditable");

            var menu = await unitOfWork.Repository<Menu>()
               .FirstOrDefault(x => !x.Deleted && x.Id == dto.Id);

            if (menu is null)
                throw new NotFoundException("Menu.NotFound");

            menu.Name = dto.Name;
            menu.IsActive = dto.IsActive;

            unitOfWork.Repository<Menu>().Update(menu);
            await unitOfWork.Save();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var menu = await unitOfWork.Repository<Menu>()
                .FirstOrDefault(x => !x.Deleted && x.Id == id);

            if (menu is null)
                throw new NotFoundException("Menu.NotFound");

            if (!menu.IsDeletable)
                throw new BadRequestException("Menu.NotDeletable");

            menu.Deleted = true;

            unitOfWork.Repository<Menu>().Update(menu);
            await unitOfWork.Save();

            return ServiceResult.Success();
        }
    }
}