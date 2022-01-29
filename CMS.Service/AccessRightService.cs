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
    public interface IAccessRightService
    {
        AccessRightModel Get();
        List<TreeMenuModel> GetUserMenu();
    }
    public class AccessRightService : IAccessRightService
    {
        private readonly IUserAccessRightService userAccessRightService;
        private readonly CMSContext context;

        public AccessRightService(
            IUserAccessRightService userAccessRightService,
            CMSContext context)
        {
            this.userAccessRightService = userAccessRightService;
            this.context = context;
        }

        public AccessRightModel Get()
        {
            var model = new AccessRightModel();
            var accessRights = context.AccessRights.Where(x => !x.Deleted).ToList();

            if (accessRights != null && accessRights.Any())
            {
                var operationAccessRights = accessRights
                    .Where(x => x.Type == AccessRightType.Operation && x.ParentId == null).ToList();
                if (operationAccessRights != null && operationAccessRights.Any())
                {
                    model.OperationAccessRights = operationAccessRights.Select(x => new TreeModel
                    {
                        Key = x.Id,
                        Label = x.Name,
                        Children = GetSubAccessRights(x.Id, accessRights)
                    }).ToList();
                }

                var menuAccessRights = accessRights.Where(x => x.Type == AccessRightType.Menu && x.ParentId == null).ToList();
                if (menuAccessRights != null || menuAccessRights.Any())
                {
                    model.MenuAccessRights = menuAccessRights.Select(x => new TreeModel
                    {
                        Key = x.Id,
                        Label = x.Name,
                        Children = GetSubAccessRights(x.Id, accessRights)
                    }).ToList();
                }
            }
            return model;
        }

        private List<TreeModel> GetSubAccessRights(int parentId, List<AccessRight> menuItems)
        {

            List<TreeModel> list = null;
            var items = menuItems.Where(x => x.ParentId != null &&
                        x.ParentId == parentId)
                        .OrderBy(x => x.DisplayOrder).ToList();

            if (items != null && items.Any())
            {
                list = items.Select(x => new TreeModel
                {
                    Key = x.Id,
                    Label = x.Name,
                    Children = x.ParentId.HasValue ? GetSubAccessRights(x.Id, menuItems) : null
                }).ToList();
            }
            return list;
        }

        public List<TreeMenuModel> GetUserMenu()
        {
            var list = new List<TreeMenuModel>();

            var user = context.Users.FirstOrDefault(x => x.Id == AuthTokenContent.Current.UserId);
            List<AccessRight> accessRights = new List<AccessRight>();

            if (user.UserType == UserType.SuperAdmin)
            {
                accessRights = context.AccessRights
                    .Where(x => x.Type == AccessRightType.Menu).ToList();
            }
            else
            {
                var userAccessRights = context.UserAccessRights
                    .Include(x => x.AccessRight)
                    .Where(x => x.UserId == user.Id).ToList();

                if (userAccessRights != null && userAccessRights.Any())
                {
                    accessRights = userAccessRights.Select(x => x.AccessRight).ToList();
                }
            }

            if (accessRights.Any())
            {
                var menuItems = accessRights
                        .Where(x => x.ParentId == null &&
                        x.IsActive && !x.Deleted)
                        .OrderBy(x => x.DisplayOrder).ToList();

                list = menuItems.Select(x => new TreeMenuModel
                {
                    Key = x.Id,
                    Label = x.Name,
                    To = x.Endpoint,
                    Children = GetSubMenuForBackEnd(x.Id, accessRights)
                }).ToList();
            }
            return list;
        }

        private List<TreeMenuModel> GetSubMenuForBackEnd(int parentId, List<AccessRight> menuItems)
        {
            List<TreeMenuModel> menus = new List<TreeMenuModel>();

            var list = new List<MenuModel>();
            var items = menuItems.Where(x => x.ParentId != null &&
                        x.ParentId == parentId)
                        .OrderBy(x => x.DisplayOrder).ToList();

            return items.Select(x => new TreeMenuModel
            {
                Key = x.Id,
                Label = x.Name,
                To = x.Endpoint,
                Children = x.ParentId.HasValue ? GetSubMenuForBackEnd(x.Id, menuItems) : null
            }).ToList();
        }
    }
}