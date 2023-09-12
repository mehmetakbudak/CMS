using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Service.Exceptions;
using CMS.Storage.Consts;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using CMS.Storage.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IMenuItemService
    {       
        Task<MenuItemModel> GetById(int id);
        Task<ServiceResult> Post(MenuItemModel model);
        Task<ServiceResult> Put(MenuItemModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class MenuItemService : IMenuItemService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public MenuItemService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }        

        public async Task<MenuItemModel> GetById(int id)
        {
            var menuItem = await _unitOfWork.Repository<MenuItem>()
                .Where(x => x.Id == id && !x.Deleted)
                .Include(x => x.Menu)
                .Include(x => x.MenuItemAccessRights)
                .FirstOrDefaultAsync();

            if (menuItem == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            var accessRightIds = new List<int>();

            if (menuItem.Menu.Type == MenuType.Admin)
            {
                accessRightIds = menuItem.MenuItemAccessRights
                    .Select(x => x.AccessRightId).ToList();
            }

            var model = new MenuItemModel
            {
                Id = menuItem.Id,
                Title = menuItem.Title,
                Url = menuItem.Url,
                ParentId = menuItem.ParentId.HasValue ? new List<int> { menuItem.ParentId.Value } : null,
                IsActive = menuItem.IsActive,
                MenuType = menuItem.Menu.Type,
                DisplayOrder = menuItem.DisplayOrder,
                AccessRightIds = accessRightIds
            };
            return model;
        }

        public async Task<ServiceResult> Post(MenuItemModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Post };

            if (!string.IsNullOrEmpty(model.Url))
            {
                var isExist = await _unitOfWork.Repository<MenuItem>()
                    .Any(x => !x.Deleted && x.Url == model.Url && x.Menu.Type == model.MenuType);

                if (isExist)
                {
                    throw new FoundException("Daha önce aynı url'den kaydedilmiş.");
                }
            }

            if (model.MenuType == MenuType.FrontEnd)
            {
                int menuId = 0;
                var menu = await _unitOfWork.Repository<Menu>()
                    .FirstOrDefault(x => x.Type == MenuType.FrontEnd);

                if (menu == null)
                {
                    var newMenu = new Menu
                    {
                        Deleted = false,
                        IsActive = true,
                        IsDeletable = false,
                        Name = "Ön Arayüz Menü",
                        Type = MenuType.FrontEnd,
                    };
                    await _unitOfWork.Repository<Menu>().Add(newMenu);
                    await _unitOfWork.Save();
                    menuId = newMenu.Id;
                }
                else
                {
                    menuId = menu.Id;
                }

                await _unitOfWork.Repository<MenuItem>().Add(new MenuItem
                {
                    Deleted = false,
                    DisplayOrder = model.DisplayOrder,
                    IsActive = model.IsActive,
                    ParentId = model.ParentId != null ? model.ParentId.First() : null,
                    Title = model.Title,
                    MenuId = menuId,
                    Url = string.IsNullOrEmpty(model.Url) ? null : model.Url

                });
                await _unitOfWork.Save();
            }
            else if (model.MenuType == MenuType.Admin)
            {
                int menuId = 0;
                var menu = await _unitOfWork.Repository<Menu>()
                    .FirstOrDefault(x => x.Type == MenuType.Admin);

                if (menu == null)
                {
                    var newMenu = new Menu
                    {
                        Deleted = false,
                        IsActive = true,
                        IsDeletable = false,
                        Name = "Admin Menü",
                        Type = MenuType.Admin
                    };
                    await _unitOfWork.Repository<Menu>().Add(newMenu);
                    await _unitOfWork.Save();
                    menuId = newMenu.Id;
                }
                else
                {
                    menuId = menu.Id;
                }

                var menuItem = new MenuItem
                {
                    Deleted = false,
                    DisplayOrder = model.DisplayOrder,
                    IsActive = model.IsActive,
                    ParentId = model.ParentId != null ? model.ParentId.First() : null,
                    Title = model.Title,
                    MenuId = menuId,
                    Url = model.Url
                };
                await _unitOfWork.Repository<MenuItem>().Add(menuItem);
                await _unitOfWork.Save();

                if (model.AccessRightIds != null && model.AccessRightIds.Count > 0)
                {
                    foreach (var accessRightId in model.AccessRightIds)
                    {
                        await _unitOfWork.Repository<MenuItemAccessRight>().Add(
                            new MenuItemAccessRight
                            {
                                AccessRightId = accessRightId,
                                MenuItemId = menuItem.Id
                            });
                    }
                    await _unitOfWork.Save();
                }
            }
            return result;
        }

        public async Task<ServiceResult> Put(MenuItemModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Put };

            if (!string.IsNullOrEmpty(model.Url))
            {
                var isExist = await _unitOfWork.Repository<MenuItem>()
                    .Any(x => x.Id != model.Id && !x.Deleted && x.Url == model.Url && x.Menu.Type == model.MenuType);

                if (isExist)
                {
                    throw new FoundException("Daha önce aynı url'den kaydedilmiş.");
                }
            }

            var menuItem = await _unitOfWork.Repository<MenuItem>()
                .Where(x => x.Id == model.Id && !x.Deleted)
                .Include(x => x.MenuItemAccessRights)
                .FirstOrDefaultAsync();

            if (menuItem == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            menuItem.Title = model.Title;
            menuItem.Url = string.IsNullOrEmpty(model.Url) ? null : model.Url;
            menuItem.ParentId = model.ParentId != null ? model.ParentId.First() : null;
            menuItem.IsActive = model.IsActive;
            menuItem.DisplayOrder = model.DisplayOrder;

            var addingList = model.AccessRightIds
                            .Where(x => !menuItem.MenuItemAccessRights.Select(x => x.AccessRightId).Contains(x)).ToList();

            if (addingList != null && addingList != null)
            {
                foreach (var accessRightId in addingList)
                {
                    await _unitOfWork.Repository<MenuItemAccessRight>().Add(
                            new MenuItemAccessRight
                            {
                                AccessRightId = accessRightId,
                                MenuItemId = menuItem.Id
                            });
                }
            }

            var deletingList = menuItem.MenuItemAccessRights.Where(x => !model.AccessRightIds.Contains(x.AccessRightId)).ToList();

            if (deletingList != null && deletingList.Any())
            {
                foreach (var menuItemAccessRight in deletingList)
                {
                    await _unitOfWork.Repository<MenuItemAccessRight>().Delete(menuItemAccessRight);
                }
            }

            await _unitOfWork.Save();

            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Delete };

            var menuItem = await _unitOfWork.Repository<MenuItem>()
                .FirstOrDefault(x => x.Id == id && !x.Deleted);

            if (menuItem == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            menuItem.Deleted = true;
            await _unitOfWork.Save();

            return result;
        }
    }
}
