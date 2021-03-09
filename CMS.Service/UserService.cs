using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Dto;
using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Helper;
using CMS.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface IUserService
    {
        IQueryable<UserModel> GetAll();
        List<LookupModel> Lookup();
        ServiceResult CreateOrUpdate(UserModel model);
        ServiceResult Delete(int id);
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork<CMSContext> unitOfWork;
        public UserService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<UserModel> GetAll()
        {
            return unitOfWork.Repository<User>()
                .GetAll(x => !x.Deleted && x.UserType != UserType.Member)
                .OrderByDescending(x => x.Id)
                .Select(x => new UserModel
                {
                    UserType = (int)x.UserType,
                    UserTypeName = EnumHelper.GetDescription<UserType>(x.UserType),
                    EmailAddress = x.EmailAddress,
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname,
                    IsActive = x.IsActive
                });
        }

        public List<LookupModel> Lookup()
        {
            return unitOfWork.Repository<User>()
                .GetAll(x => !x.Deleted && x.IsActive && x.UserType != UserType.Member)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name + " " + x.Surname
                }).ToList();
        }

        public ServiceResult CreateOrUpdate(UserModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                if (model.Id == 0)
                {
                    var checkEmail = unitOfWork.Repository<User>()
                        .Find(x => x.EmailAddress == model.EmailAddress &&
                        !x.Deleted);
                    if (checkEmail == null)
                    {
                        var user = new User
                        {
                            Deleted = false,
                            EmailAddress = model.EmailAddress,
                            IsActive = model.IsActive,
                            IsNewUser = true,
                            Name = model.Name,
                            Surname = model.Surname,
                            UserType = (UserType)model.UserType
                        };
                        unitOfWork.Repository<User>().Add(user);
                        unitOfWork.Save();
                    }
                    else
                    {
                        serviceResult.StatusCode = HttpStatusCode.Found;
                        serviceResult.Exceptions.Add("Email adresi ile daha önce kullanıcı kaydedilmiş.");
                    }
                }
                else
                {
                    var checkEmail = unitOfWork.Repository<User>()
                       .Find(x => x.EmailAddress == model.EmailAddress &&
                       !x.Deleted && x.Id != model.Id);

                    if (checkEmail == null)
                    {
                        var user = unitOfWork.Repository<User>()
                       .Find(x => !x.Deleted && x.Id == model.Id);

                        if (user != null)
                        {
                            user.EmailAddress = model.EmailAddress;
                            user.IsActive = model.IsActive;
                            user.Name = model.Name;
                            user.Surname = model.Surname;
                            user.UserType = (UserType)model.UserType;
                            unitOfWork.Save();
                        }
                        else
                        {
                            serviceResult.StatusCode = HttpStatusCode.NotFound;
                            serviceResult.Exceptions.Add("Kayıt bulunamadı.");
                        }
                    }
                    else
                    {
                        serviceResult.StatusCode = HttpStatusCode.BadRequest;
                        serviceResult.Exceptions.Add("Email adresi ile daha önce kullanıcı kaydedilmiş.");
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Exceptions.Add(ex.Message);
            }
            return serviceResult;
        }

        public ServiceResult Delete(int id)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var user = unitOfWork.Repository<User>()
                       .Find(x => x.Id == id);

                if (user != null)
                {
                    user.Deleted = true;
                    unitOfWork.Save();
                }
                else
                {
                    serviceResult.StatusCode = HttpStatusCode.NotFound;
                    serviceResult.Exceptions.Add("Kayıt bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Exceptions.Add(ex.Message);
            }
            return serviceResult;
        }
    }
}
