using CMS.Data.Context;
using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface IUserAccessRightService
    {
        UserAccessRightModel Get(int userId);
        List<UserAccessRight> GetByUserId(int userId);
        ServiceResult CreateOrUpdate(UserAccessRightModel model);
    }
    public class UserAccessRightService : IUserAccessRightService
    {
        private readonly CMSContext context;

        public UserAccessRightService(CMSContext context)
        {
            this.context = context;
        }

        public UserAccessRightModel Get(int userId)
        {
            var model = new UserAccessRightModel();
            var user = context.Users.FirstOrDefault(x => !x.Deleted && x.IsActive && x.Id == userId && x.UserType != UserType.Member);

            if (user != null)
            {
                model.UserId = userId;
                if (user.UserType == UserType.SuperAdmin)
                {
                    var accessRights = context.AccessRights.Where(x => !x.Deleted).ToList();

                    if (accessRights != null && accessRights.Any())
                    {
                        model.MenuUserAccessRights = accessRights
                            .Where(x => x.Type == AccessRightType.Menu)
                            .Select(x => x.Id).ToList();

                        model.OperationUserAccessRights = accessRights
                            .Where(x => x.Type == AccessRightType.Operation)
                            .Select(x => x.Id).ToList();
                    }
                }
                else
                {
                    var accessRights = context.UserAccessRights
                        .Where(x => x.UserId == userId)
                        .Select(x => x.AccessRight).ToList();

                    if (accessRights != null && accessRights.Any())
                    {
                        model.MenuUserAccessRights = accessRights
                            .Where(x => !x.Deleted && x.Type == AccessRightType.Menu)
                            .Select(x => x.Id).ToList();

                        model.OperationUserAccessRights = accessRights
                            .Where(x => !x.Deleted && x.Type == AccessRightType.Operation)
                            .Select(x => x.Id).ToList();
                    }
                }
            }
            return model;
        }

        public List<UserAccessRight> GetByUserId(int userId)
        {
            return context.UserAccessRights
                .Include(o => o.AccessRight)
                .Where(x => x.UserId == userId).ToList();
        }

        public ServiceResult CreateOrUpdate(UserAccessRightModel model)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            try
            {
                var user = context.Users.FirstOrDefault(x => !x.Deleted && x.IsActive && x.Id == model.UserId && x.UserType != UserType.Member);
                if (user != null)
                {
                    if (user.UserType != UserType.SuperAdmin)
                    {
                        var userAccessRights = context.UserAccessRights
                            .Include(x => x.AccessRight)
                            .Where(x => x.UserId == model.UserId).ToList();
                        if (userAccessRights != null && userAccessRights.Any())
                        {
                            var addList = new List<int>();
                            var deleteList = new List<AccessRight>();

                            var accessRights = userAccessRights.Select(x => x.AccessRight).ToList();
                            var menuAccessRights = accessRights.Where(x => !x.Deleted && x.Type == AccessRightType.Menu).ToList();
                            if (menuAccessRights != null && menuAccessRights.Any())
                            {
                                var addingList = model.MenuUserAccessRights
                                    .Where(x => !menuAccessRights.Select(x => x.Id).Contains(x)).ToList();

                                if (addingList != null && addingList != null)
                                {
                                    foreach (var a in addingList)
                                    {
                                        if (!addList.Any(x => x == a))
                                        {
                                            addList.Add(a);
                                        }
                                    }
                                }

                                var deletingList = menuAccessRights.Where(x => !model.MenuUserAccessRights.Contains(x.Id)).ToList();

                                if (deletingList != null && deletingList.Any())
                                {
                                    foreach (var a in deletingList)
                                    {
                                        if (!deleteList.Any(x => x.Id == a.Id))
                                        {
                                            deleteList.Add(a);
                                        }
                                    }
                                }
                            }
                            var operationAccessRights = accessRights.Where(x => !x.Deleted && x.Type == AccessRightType.Operation).ToList();
                            if (operationAccessRights != null && operationAccessRights.Any())
                            {
                                var addingList = model.OperationUserAccessRights
                                    .Where(x => !operationAccessRights.Select(x => x.Id).Contains(x)).ToList();

                                if (addingList != null && addingList != null)
                                {
                                    foreach (var a in addingList)
                                    {
                                        if (!addList.Any(x => x == a))
                                        {
                                            addList.Add(a);
                                        }
                                    }
                                }

                                var deletingList = operationAccessRights.Where(x => !model.OperationUserAccessRights.Contains(x.Id)).ToList();

                                if (deletingList != null && deletingList.Any())
                                {
                                    foreach (var a in deletingList)
                                    {
                                        if (!deleteList.Any(x => x.Id == a.Id))
                                        {
                                            deleteList.Add(a);
                                        }
                                    }
                                }
                            }

                            if (addList.Any())
                            {
                                context.UserAccessRights.AddRange(addList.Select(x => new UserAccessRight
                                {
                                    AccessRightId = x,
                                    UserId = model.UserId
                                }).ToList());
                            }
                            if (deleteList.Any())
                            {
                                foreach (var a in deleteList)
                                {
                                    var userAccessRight = userAccessRights.FirstOrDefault(x => x.AccessRightId == a.Id);
                                    if (userAccessRight != null)
                                    {
                                        context.UserAccessRights.Remove(userAccessRight);
                                    }
                                }
                            }
                            context.SaveChanges();
                        }
                    }
                }
                else
                {
                    result.StatusCode = (int)HttpStatusCode.NotFound;
                    result.Message = "Kullanıcı bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = (int)HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
