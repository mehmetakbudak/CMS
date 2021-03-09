using System;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Dto;
using CMS.Model.Entity;
using CMS.Model.Enum;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMS.Service
{
    public interface IAccessRightService
    {
        IQueryable<AccessRightModel> GetAll(AccessRightCategoryType menuType);
        List<AccessRightModel> GetFrontEndMenu();
        List<AccessRightModel> GetBackEndMenuByUserId(int userId, int userType);
        List<AccessRight> GetAccessRightsByUserId(int userId, bool isShowMenu);
    }
    public class AccessRightService : IAccessRightService
    {
        private readonly IUserAccessRightService userAccessRightService;
        private readonly IUserUserGroupService userUserGroupService;
        private readonly IUnitOfWork<CMSContext> unitOfWork;

        public AccessRightService(
            IUserAccessRightService userAccessRightService,
            IUserUserGroupService userUserGroupService,
            IUnitOfWork<CMSContext> unitOfWork)
        {
            this.userAccessRightService = userAccessRightService;
            this.userUserGroupService = userUserGroupService;
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<AccessRightModel> GetAll(AccessRightCategoryType menuType)
        {
            List<AccessRightModel> list = new List<AccessRightModel>();

            var accessRights = unitOfWork
                .Repository<AccessRight>()
                .GetAll(x => x.ParentId == null,
                x => x.Include(o => o.AccessRightCategory))
                .Where(x => !x.Deleted &&
                x.AccessRightCategory.Type == menuType).ToList();

            foreach (var x in accessRights)
            {
                AccessRightModel item = new AccessRightModel();
                item.Id = x.Id;
                item.Title = x.LinkName;
                item.Url = x.LinkUrl;
                var subMenus = GetAccessRightSubItems(x.Id, menuType);
                if (subMenus.Any())
                    item.Items = subMenus;
                list.Add(item);
            }
            return list.AsQueryable();
        }

        public List<AccessRightModel> GetAccessRightSubItems(int? parentId, AccessRightCategoryType menuType)
        {
            List<AccessRightModel> list = new List<AccessRightModel>();

            var accessRights = unitOfWork.Repository<AccessRight>()
                .GetAll(x => !x.Deleted &&
                x.AccessRightCategory.Type == menuType &&
                x.ParentId == parentId &&
                x.IsShowMenu)
                .OrderBy(x => x.Order).ToList();

            foreach (var x in accessRights)
            {
                AccessRightModel item = new AccessRightModel();
                item.Id = x.Id;
                item.Title = x.LinkName;
                item.Url = x.LinkUrl;
                var subMenus = GetAccessRightSubItems(x.Id, menuType);
                if (subMenus.Any())
                    item.Items = subMenus;
                list.Add(item);
            }
            return list;
        }

        public List<AccessRight> GetAccessRightsByUserId(int userId, bool isShowMenu)
        {
            List<AccessRight> accessRights = new List<AccessRight>();
            try
            {
                var userAccessRights = userAccessRightService.GetAll().Where(x =>
                    x.UserId == userId &&
                    !x.AccessRight.Deleted &&
                    x.AccessRight.IsShowMenu == isShowMenu)
                    .Select(x => x.AccessRight).ToList();

                if (userAccessRights.Any())
                    accessRights.AddRange(userAccessRights);

                var userRole = userUserGroupService.GetAll()
                    .Where(x => x.UserId == userId).ToList();

                foreach (var item in userRole)
                {
                    var accessRightList = item.Role.RoleAccessRights.Where(x =>
                        x.AccessRight.IsShowMenu == isShowMenu &&
                        !x.AccessRight.Deleted)
                        .Select(x => x.AccessRight).ToList();

                    foreach (var accessRight in accessRightList)
                    {
                        var checkAccessRight = accessRights.Any(x => x.Id == accessRight.Id);
                        if (!checkAccessRight)
                            accessRights.Add(accessRight);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return accessRights;
        }

        public List<AccessRightModel> GetFrontEndMenu()
        {
            List<AccessRightModel> menus = new List<AccessRightModel>();
            try
            {
                var list = unitOfWork.Repository<AccessRight>()
                .GetAll(x => !x.Deleted &&
                x.AccessRightCategory.Type == AccessRightCategoryType.Frontend &&
                x.ParentId == null &&
                x.IsShowMenu,
                x => x.Include(o => o.AccessRightCategory))
                .OrderBy(x => x.Order).ToList();

                foreach (var x in list)
                {
                    AccessRightModel menu = new AccessRightModel();
                    menu.Id = x.Id;
                    menu.Title = x.LinkName;
                    menu.Url = x.LinkUrl;
                    var subMenus = GetSubMenuForFrontEnd(x.Id);
                    if (subMenus.Any())
                        menu.Items = subMenus;
                    menus.Add(menu);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return menus;
        }

        public List<AccessRightModel> GetSubMenuForFrontEnd(int? parentId)
        {
            List<AccessRightModel> menus = new List<AccessRightModel>();
            try
            {
                var list = unitOfWork.Repository<AccessRight>()
                    .GetAll(
                    x => !x.Deleted &&
                    x.AccessRightCategory.Type == AccessRightCategoryType.Frontend &&
                    x.ParentId == parentId
                    && x.IsShowMenu,
                    x => x.Include(o => o.AccessRightCategory))
                    .OrderBy(x => x.Order).ToList();

                foreach (var x in list)
                {
                    AccessRightModel menu = new AccessRightModel();
                    menu.Id = x.Id;
                    menu.Title = x.LinkName;
                    menu.Url = x.LinkUrl;
                    var subMenus = GetSubMenuForFrontEnd(x.Id);
                    if (subMenus.Any())
                        menu.Items = subMenus;
                    menus.Add(menu);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return menus;
        }

        public List<AccessRightModel> GetSubMenuForBackEnd(List<AccessRight> list, int? parentId)
        {
            List<AccessRightModel> menus = new List<AccessRightModel>();
            try
            {
                var accessRights = list.Where(x => !x.Deleted &&
                  x.AccessRightCategory.Type == AccessRightCategoryType.Admin &&
                  x.ParentId == parentId &&
                  x.IsShowMenu)
                    .OrderBy(x => x.Order).ToList();

                foreach (var x in accessRights)
                {
                    AccessRightModel menu = new AccessRightModel();
                    menu.Id = x.Id;
                    menu.Title = x.LinkName;
                    menu.Url = x.LinkUrl;
                    var subMenus = GetSubMenuForBackEnd(list, x.Id);
                    if (subMenus.Any())
                        menu.Items = subMenus;
                    menus.Add(menu);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return menus;
        }

        public List<AccessRightModel> GetBackEndMenuByUserId(int userId, int userType)
        {
            List<AccessRightModel> menus = new List<AccessRightModel>();
            try
            {
                var type = (UserType)userType;

                if (type == UserType.SuperAdmin)
                {
                    return GetAll(AccessRightCategoryType.Admin).ToList();
                }

                var list = GetAccessRightsByUserId(userId, true);

                var accessRights = list.Where(x => x.ParentId == null).OrderBy(x => x.Order).ToList();

                foreach (var x in accessRights)
                {
                    AccessRightModel menu = new AccessRightModel();
                    menu.Id = x.Id;
                    menu.Title = x.LinkName;
                    menu.Url = x.LinkUrl;
                    var subMenus = GetSubMenuForBackEnd(list, x.Id);
                    if (subMenus.Any())
                        menu.Items = subMenus;
                    menus.Add(menu);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return menus;
        }
    }
}
