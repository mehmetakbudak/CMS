using CMS.Data.Context;
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
        List<MenuModel> GetFrontEndMenu();

    }

    public class MenuService : IMenuService
    {
        private readonly CMSContext context;

        public MenuService(CMSContext context)
        {
            this.context = context;
        }

        public List<MenuModel> GetFrontEndMenu()
        {
            var list = new List<MenuModel>();
            var menu = context.Menus
                .Include(x => x.MenuItems)
                .FirstOrDefault(x => x.Type == MenuType.FrontEnd &&
                !x.Deleted && x.IsActive);

            var menuItems = menu.MenuItems
                .Where(x => x.ParentId == null && x.IsActive && !x.Deleted)
                .OrderBy(x => x.Order).ToList();

            list = menuItems.Select(x => new MenuModel
            {
                Id = x.Id,
                Label = x.Label,
                To = x.To,
                Items = GetSubMenuItems(x.Id, menu.MenuItems)
            }).ToList();

            return list;
        }

        private List<MenuModel> GetSubMenuItems(int parentId, List<MenuItems> menuItems)
        {
            List<MenuModel> list = null;
            var items = menuItems.Where(x => x.ParentId != null &&
                        x.ParentId == parentId)
                        .OrderBy(x => x.Order).ToList();

            if (items.Any())
            {
                list = items.Select(x => new MenuModel
                {
                    Id = x.Id,
                    Label = x.Label,
                    To = x.To,
                    Items = x.ParentId.HasValue ? GetSubMenuItems(x.Id, menuItems) : null
                }).ToList();
            }
            return list;
        }

        public List<LookupModel> Lookup()
        {
            var list = context.Menus
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
