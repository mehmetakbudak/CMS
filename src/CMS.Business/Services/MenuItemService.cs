using CMS.Business.Exceptions;
using CMS.Business.Helper;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.Menu;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IMenuItemService
    {
        Task<MenuItemDto> GetById(int id);
        Task<List<TreeMenuDto>> GetMenuItemsByMenuId(int menuId, int? parentId = null, List<TreeMenuDto> children = null);
        Task<ServiceResult> Create(MenuItemDto dto);
        Task<ServiceResult> Update(MenuItemDto dto);
        Task<ServiceResult> Delete(int id);
    }

    public class MenuItemService(IUnitOfWork<CMSContext> unitOfWork) : IMenuItemService
    {
        public async Task<MenuItemDto> GetById(int id)
        {
            var menuItem = await unitOfWork.Repository<MenuItem>()
                .Where(x => x.Id == id && !x.Deleted)
                .Include(x => x.MenuItemAccessRights)
                .Include(x => x.Menu)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (menuItem is null)
                throw new NotFoundException();

            var accessRightIds = new List<int>();

            if (menuItem.Menu.Type == MenuType.Admin)
            {
                accessRightIds = menuItem.MenuItemAccessRights
                    .Select(x => x.AccessRightId).ToList();
            }

            var model = new MenuItemDto
            {
                Id = menuItem.Id,
                Title = menuItem.Title,
                Url = menuItem.Url,
                ParentId = menuItem.ParentId,
                IsActive = menuItem.IsActive,
                MenuId = menuItem.MenuId,
                DisplayOrder = menuItem.DisplayOrder,
                AccessRightIds = accessRightIds
            };
            return model;
        }

        public async Task<List<TreeMenuDto>> GetMenuItemsByMenuId(int menuId, int? parentId = null, List<TreeMenuDto> children = null)
        {
            var menuItems = new List<TreeMenuDto>();

            var list = await unitOfWork.Repository<MenuItem>()
                          .Where(x => x.MenuId == menuId && !x.Deleted && x.IsActive && x.ParentId == parentId)
                          .Include(x => x.Menu)
                          .AsNoTracking()
                          .OrderBy(x => x.DisplayOrder)
                          .Select(x => new TreeMenuDto
                          {
                              Id = x.Id,
                              Label = x.Title,
                              Route = x.Url,
                              DisplayOrder = x.DisplayOrder,
                              ParentId = parentId,
                              IsActive = x.IsActive
                          }).ToListAsync();

            menuItems.AddRange(list);

            foreach (var menuItem in list)
            {
                var items = await GetMenuItemsByMenuId(menuId, menuItem.Id, list);
                if (items != null && items.Count > 0)
                {
                    menuItem.Children = items;
                }
            }
            return menuItems;
        }

        public async Task<ServiceResult> Create(MenuItemDto dto)
        {            
            var menu = await unitOfWork.Repository<Menu>()
                .FirstOrDefault(x => x.Id == dto.MenuId && !x.Deleted);

            if (menu is null)
                throw new NotFoundException("Menu.Notfound");

            var strategy = unitOfWork.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                try
                {
                    await unitOfWork.CreateTransaction();

                    var menuItem = new MenuItem
                    {
                        Deleted = false,
                        DisplayOrder = dto.DisplayOrder,
                        IsActive = dto.IsActive,
                        ParentId = dto.ParentId,
                        Title = dto.Title,
                        MenuId = dto.MenuId,
                        Url = dto.Url
                    };
                    await unitOfWork.Repository<MenuItem>().Add(menuItem);
                    await unitOfWork.Save();

                    if (menu.Type == MenuType.Admin &&
                        dto.AccessRightIds != null &&
                        dto.AccessRightIds.Count > 0)
                    {
                        foreach (var accessRightId in dto.AccessRightIds)
                        {
                            await unitOfWork.Repository<MenuItemAccessRight>().Add(new()
                            {
                                AccessRightId = accessRightId,
                                MenuItemId = menuItem.Id,
                            });
                        }
                        await unitOfWork.Save();
                    }
                    await unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    await unitOfWork.Rollback();
                    throw new Exception(ex.Message);
                }
            });
            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Update(MenuItemDto dto)
        {
            var menuItem = await unitOfWork.Repository<MenuItem>()
                .Where(x => x.Id == dto.Id && !x.Deleted)
                .Include(x => x.MenuItemAccessRights)
                .Include(x => x.Menu)
                .FirstOrDefaultAsync();

            if (menuItem is null)
                throw new NotFoundException("Menu.Notfound");

            menuItem.Title = dto.Title;
            menuItem.Url = string.IsNullOrEmpty(dto.Url) ? null : dto.Url;
            menuItem.ParentId = dto.ParentId;
            menuItem.IsActive = dto.IsActive;
            menuItem.DisplayOrder = dto.DisplayOrder;

            if (menuItem.Menu.Type == MenuType.Admin)
            {
                if (dto.AccessRightIds is null)
                    dto.AccessRightIds = new List<int>();

                var accessRightIds = menuItem.MenuItemAccessRights.Select(x => x.AccessRightId).ToList();

                var addingList = dto.AccessRightIds.Where(x => !accessRightIds.Contains(x)).ToList();

                if (addingList != null && addingList != null)
                {
                    foreach (var accessRightId in addingList)
                    {
                        await unitOfWork.Repository<MenuItemAccessRight>().Add(new()
                        {
                            AccessRightId = accessRightId,
                            MenuItemId = menuItem.Id
                        });
                    }
                }
                var deletingList = accessRightIds.Where(x => !dto.AccessRightIds.Contains(x)).ToList();

                if (deletingList != null && deletingList.Any())
                {
                    foreach (var accessRightId in deletingList)
                    {
                        var menuItemAccessRight = menuItem.MenuItemAccessRights.FirstOrDefault(x => x.AccessRightId == accessRightId && x.MenuItemId == menuItem.Id);

                        if (menuItemAccessRight != null)
                            await unitOfWork.Repository<MenuItemAccessRight>().Delete(menuItemAccessRight);
                    }
                }
            }
            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var menuItem = await unitOfWork.Repository<MenuItem>()
                .FirstOrDefault(x => x.Id == id && !x.Deleted);

            if (menuItem is null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            menuItem.Deleted = true;
            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }
    }
}
