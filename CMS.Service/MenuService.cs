using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Dto;
using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Service
{
    public interface IMenuService
    {
        List<LookupModel> Lookup();

        List<MenuBarModel> GetFrontEndMenu();
    }

    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public MenuService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<MenuBarModel> GetFrontEndMenu()
        {
            var list = new List<MenuBarModel>();
            var menu = _unitOfWork.Repository<Menu>()
                .Where(x => x.Type == MenuType.FrontEnd && !x.Deleted && x.IsActive)
                .Include(x => x.MenuItems)
                .FirstOrDefault();

            var menuItems = menu.MenuItems
                .Where(x => x.ParentId == null && x.IsActive && !x.Deleted)
                .OrderBy(x => x.DisplayOrder).ToList();

            list = menuItems.Select(x => new MenuBarModel
            {
                Id = x.Id,
                Label = x.Title,
                To = x.Url,
                Items = GetSubMenuItems(x.Id, menu.MenuItems.ToList())
            }).ToList();

            return list;
        }

        private List<MenuBarModel> GetSubMenuItems(int parentId, List<MenuItems> menuItems)
        {
            List<MenuBarModel> list = null;
            var items = menuItems.Where(x => x.ParentId != null &&
                        x.ParentId == parentId)
                        .OrderBy(x => x.DisplayOrder).ToList();

            if (items.Any())
            {
                list = items.Select(x => new MenuBarModel
                {
                    Id = x.Id,
                    Label = x.Title,
                    To = x.Url,
                    Items = x.ParentId.HasValue ? GetSubMenuItems(x.Id, menuItems) : null
                }).ToList();
            }
            return list;
        }

        public List<LookupModel> Lookup()
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


    }
}
