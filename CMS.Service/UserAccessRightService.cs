using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Model;
using CMS.Service.Exceptions;
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
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public UserAccessRightService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public UserAccessRightModel Get(int userId)
        {
            var model = new UserAccessRightModel();
            var user = _unitOfWork.Repository<User>()
                .FirstOrDefault(x => !x.Deleted && x.IsActive && x.Id == userId && x.UserType != UserType.Member);

            if (user != null)
            {
                model.UserId = userId;
                if (user.UserType == UserType.SuperAdmin)
                {
                    var accessRights = _unitOfWork.Repository<AccessRight>()
                        .Where(x => !x.Deleted).ToList();

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
                    var accessRights = _unitOfWork.Repository<UserAccessRight>()
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
            return _unitOfWork.Repository<UserAccessRight>()
                .Where(x => x.UserId == userId)
                .Include(o => o.AccessRight)
                .ToList();
        }

        public ServiceResult CreateOrUpdate(UserAccessRightModel model)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var user = _unitOfWork.Repository<User>()
                .FirstOrDefault(x => !x.Deleted && x.IsActive && x.Id == model.UserId && x.UserType != UserType.Member);

            if (user != null)
            {
                if (user.UserType != UserType.SuperAdmin)
                {
                    var userAccessRights = _unitOfWork.Repository<UserAccessRight>()
                        .Where(x => x.UserId == model.UserId)
                        .Include(x => x.AccessRight)
                        .ToList();

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
                            _unitOfWork.Repository<UserAccessRight>()
                                .AddRange(addList.Select(x => new UserAccessRight
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
                                    _unitOfWork.Repository<UserAccessRight>().Delete(userAccessRight);
                                }
                            }
                        }
                        _unitOfWork.Save();
                    }
                }
            }
            else
            {
                throw new NotFoundException("Kullanıcı bulunamadı.");
            }
            return result;
        }
    }
}
