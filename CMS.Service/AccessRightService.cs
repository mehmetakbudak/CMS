using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Consts;
using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using CMS.Service.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface IAccessRightService
    {
        AccessRightGetModel Get();

        AccessRightModel Get(int id);

        List<TreeMenuModel> GetUserMenu();

        ServiceResult PostMenu(AccessRightModel model);

        ServiceResult PutMenu(AccessRightModel model);

        ServiceResult PostOperation(AccessRightModel model);

        ServiceResult PutOperation(AccessRightModel model);

        ServiceResult Delete(int id);
    }

    public class AccessRightService : IAccessRightService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;
        private readonly IMemoryCache _memoryCache;


        public AccessRightService(
            IUnitOfWork<CMSContext> unitOfWork,
            IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }

        public AccessRightGetModel Get()
        {
            var model = new AccessRightGetModel();

            var accessRights = _unitOfWork.Repository<AccessRight>().Where(x => !x.Deleted).ToList();

            if (accessRights != null && accessRights.Any())
            {
                var operationAccessRights = accessRights
                    .Where(x => x.Type == AccessRightType.Operation && x.ParentId == null).ToList();

                if (operationAccessRights != null && operationAccessRights.Any())
                {
                    model.OperationAccessRights = operationAccessRights.Select(x => new TreeModel
                    {
                        Title = x.Name,
                        Items = GetSubAccessRights(x.Id, accessRights)
                    }).ToList();
                }

                var menuAccessRights = accessRights
                    .Where(x => x.Type == AccessRightType.Menu && x.ParentId == null).ToList();

                if (menuAccessRights != null || menuAccessRights.Any())
                {
                    model.MenuAccessRights = menuAccessRights.Select(x => new TreeModel
                    {
                        Title = x.Name,
                        Items = GetSubAccessRights(x.Id, accessRights)
                    }).ToList();
                }
            }
            return model;
        }

        public AccessRightModel Get(int id)
        {
            var accessRight = _unitOfWork.Repository<AccessRight>()
                .Where(x => x.Id == id && !x.Deleted)
                .Include(x => x.AccessRightEndpoints)
                .FirstOrDefault();

            if (accessRight == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }
            var model = new AccessRightModel()
            {
                DisplayOrder = accessRight.DisplayOrder,
                IsActive = accessRight.IsActive,
                Id = id,
                IsShowMenu = accessRight.IsShowMenu,
                Name = accessRight.Name,
                ParentId = accessRight.ParentId
            };
            if (accessRight.AccessRightEndpoints != null && accessRight.AccessRightEndpoints.Count > 0)
            {
                var accessRightEndpoint = accessRight.AccessRightEndpoints.FirstOrDefault();
                model.Method = accessRightEndpoint.Method;
                model.Endpoint = accessRightEndpoint.Endpoint;
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
                    Title = x.Name,
                    Items = x.ParentId.HasValue ? GetSubAccessRights(x.Id, menuItems) : null
                }).ToList();
            }
            return list;
        }

        public List<TreeMenuModel> GetUserMenu()
        {
            var list = new List<TreeMenuModel>();
            var accessRights = new List<AccessRight>();

            var user = _unitOfWork.Repository<User>().FirstOrDefault(x => x.Id == AuthTokenContent.Current.UserId);

            if (user.UserType == UserType.SuperAdmin)
            {
                accessRights = _unitOfWork.Repository<AccessRight>()
                    .Where(x => x.Type == AccessRightType.Menu)
                    .Include(x => x.AccessRightEndpoints).ToList();
            }
            else
            {
                var userAccessRights = _unitOfWork.Repository<UserAccessRight>()
                    .Where(x => x.UserId == user.Id)
                    .Include(x => x.AccessRight)
                    .ThenInclude(x => x.AccessRightEndpoints).ToList();

                if (userAccessRights != null && userAccessRights.Any())
                {
                    accessRights = userAccessRights.Select(x => x.AccessRight).ToList();
                }
            }

            if (accessRights.Any())
            {
                list = GetAdminMenu(accessRights);
            }
            return list;
        }

        private List<TreeMenuModel> GetAdminMenu(List<AccessRight> accessRights, int? parentId = null, List<TreeMenuModel> children = null)
        {
            var list = new List<TreeMenuModel>();

            var menuItems = accessRights
                        .Where(x => x.IsActive && !x.Deleted && x.ParentId == parentId)
                        .OrderBy(x => x.DisplayOrder)
                        .Select(x => new TreeMenuModel()
                        {
                            Id = x.Id,
                            Title = x.Name,
                            To = x.AccessRightEndpoints.Select(x => x.Endpoint).FirstOrDefault()
                        }).ToList();

            list.AddRange(menuItems);

            foreach (var menuItem in menuItems)
            {
                var items = GetAdminMenu(accessRights, menuItem.Id, menuItem.Items);
                if (items != null && items.Count > 0)
                {
                    menuItem.Items = items;
                }
            }
            return list;
        }

        public ServiceResult PostMenu(AccessRightModel model)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var accessRight = new AccessRight()
            {
                Deleted = false,
                DisplayOrder = model.DisplayOrder,
                ParentId = model.ParentId,
                IsActive = model.IsActive,
                IsShowMenu = model.IsShowMenu,
                Name = model.Name,
                Type = AccessRightType.Menu
            };

            if (!string.IsNullOrEmpty(model.Endpoint))
            {
                var accessRightEndpoint = new AccessRightEndpoint()
                {
                    AccessRight = accessRight,
                    Endpoint = model.Endpoint
                };
                _unitOfWork.Repository<AccessRightEndpoint>().Add(accessRightEndpoint);
            }
            else
            {
                _unitOfWork.Repository<AccessRight>().Add(accessRight);
            }
            _unitOfWork.Save();
            result.Message = AlertMessages.Post;
            ClearUserCache();

            return result;
        }

        public ServiceResult PutMenu(AccessRightModel model)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var accessRight = _unitOfWork.Repository<AccessRight>()
               .Where(x => x.Id == model.Id && !x.Deleted && x.Type == AccessRightType.Menu)
               .Include(x => x.AccessRightEndpoints)
               .FirstOrDefault();

            if (accessRight == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            accessRight.ParentId = model.ParentId;
            accessRight.IsActive = model.IsActive;
            accessRight.IsShowMenu = model.IsShowMenu;
            accessRight.Name = model.Name;
            accessRight.DisplayOrder = model.DisplayOrder;

            if (!string.IsNullOrEmpty(model.Endpoint))
            {
                if (accessRight.AccessRightEndpoints != null && accessRight.AccessRightEndpoints.Count > 0)
                {
                    var accessRightEndpoint = accessRight.AccessRightEndpoints.FirstOrDefault();
                    accessRightEndpoint.Endpoint = model.Endpoint;
                }
                else
                {
                    var accessRightEndpoint = new AccessRightEndpoint()
                    {
                        AccessRightId = accessRight.Id,
                        Endpoint = model.Endpoint
                    };
                    _unitOfWork.Repository<AccessRightEndpoint>().Add(accessRightEndpoint);
                }
            }
            else
            {
                if (accessRight.AccessRightEndpoints != null && accessRight.AccessRightEndpoints.Count > 0)
                {
                    var accessRightEndpoint = accessRight.AccessRightEndpoints.FirstOrDefault();
                    _unitOfWork.Repository<AccessRightEndpoint>().Delete(accessRightEndpoint);
                }
            }
            _unitOfWork.Save();
            result.Message = AlertMessages.Put;
            ClearUserCache();

            return result;
        }

        public ServiceResult Delete(int id)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var accessRight = _unitOfWork.Repository<AccessRight>()
               .Where(x => x.Id == id && !x.Deleted)
               .Include(x => x.AccessRightEndpoints)
               .FirstOrDefault();

            if (accessRight == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            accessRight.Deleted = true;

            if (accessRight.AccessRightEndpoints != null && accessRight.AccessRightEndpoints.Count > 0)
            {
                var accessRightEndpoint = accessRight.AccessRightEndpoints.FirstOrDefault();
                _unitOfWork.Repository<AccessRightEndpoint>().Delete(accessRightEndpoint);
            }
            _unitOfWork.Save();
            result.Message = AlertMessages.Delete;
            ClearUserCache();

            return result;
        }

        public void ClearUserCache()
        {
            var userIds = _unitOfWork.Repository<User>().Where(x => x.IsActive && !x.Deleted && x.UserType != UserType.Member).Select(x => x.Id).ToList();

            foreach (var userId in userIds)
            {
                string key = $"userMenu_{userId}";
                _memoryCache.Remove(key);
            }
        }

        public ServiceResult PostOperation(AccessRightModel model)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var accessRight = new AccessRight()
            {
                Deleted = false,
                DisplayOrder = model.DisplayOrder,
                ParentId = model.ParentId,
                IsActive = model.IsActive,
                IsShowMenu = false,
                Name = model.Name,
                Type = AccessRightType.Operation
            };

            if (!string.IsNullOrEmpty(model.Endpoint))
            {
                var accessRightEndpoint = new AccessRightEndpoint()
                {
                    AccessRight = accessRight,
                    Endpoint = model.Endpoint,
                    Method = model.Method
                };
                _unitOfWork.Repository<AccessRightEndpoint>().Add(accessRightEndpoint);
            }
            else
            {
                _unitOfWork.Repository<AccessRight>().Add(accessRight);
            }
            _unitOfWork.Save();
            result.Message = AlertMessages.Post;
            ClearUserCache();

            return result;
        }

        public ServiceResult PutOperation(AccessRightModel model)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var accessRight = _unitOfWork.Repository<AccessRight>()
               .Where(x => x.Id == model.Id && !x.Deleted && x.Type == AccessRightType.Operation)
               .Include(x => x.AccessRightEndpoints)
               .FirstOrDefault();

            if (accessRight == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            accessRight.ParentId = model.ParentId;
            accessRight.IsActive = model.IsActive;
            accessRight.IsShowMenu = false;
            accessRight.Name = model.Name;
            accessRight.DisplayOrder = model.DisplayOrder;

            if (!string.IsNullOrEmpty(model.Endpoint))
            {
                if (accessRight.AccessRightEndpoints != null && accessRight.AccessRightEndpoints.Count > 0)
                {
                    var accessRightEndpoint = accessRight.AccessRightEndpoints.FirstOrDefault();
                    accessRightEndpoint.Endpoint = model.Endpoint;
                    accessRightEndpoint.Method = model.Method;
                }
                else
                {
                    var accessRightEndpoint = new AccessRightEndpoint()
                    {
                        AccessRightId = accessRight.Id,
                        Endpoint = model.Endpoint,
                        Method = model.Method
                    };
                    _unitOfWork.Repository<AccessRightEndpoint>().Add(accessRightEndpoint);
                }
            }
            else
            {
                if (accessRight.AccessRightEndpoints != null && accessRight.AccessRightEndpoints.Count > 0)
                {
                    var accessRightEndpoint = accessRight.AccessRightEndpoints.FirstOrDefault();
                    _unitOfWork.Repository<AccessRightEndpoint>().Delete(accessRightEndpoint);
                }
            }
            _unitOfWork.Save();
            result.Message = AlertMessages.Put;
            ClearUserCache();

            return result;
        }
    }
}