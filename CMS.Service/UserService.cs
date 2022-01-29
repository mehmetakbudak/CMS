using CMS.Data.Context;
using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Helper;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using CMS.Service.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface IUserService
    {
        IQueryable<UserModel> GetAll();
        User GetById(int id);
        ServiceResult Authenticate(LoginModel model);
        User GetTokenInfo(int userId, string token);
        List<LookupModel> Lookup();
        ServiceResult Post(UserModel model);
        ServiceResult Put(UserModel model);
        ServiceResult Delete(int id);
    }

    public class UserService : IUserService
    {
        private readonly CMSContext context;
        private readonly IJwtHelper jwtHelper;
        public UserService(CMSContext context,
            IJwtHelper jwtHelper)
        {
            this.context = context;
            this.jwtHelper = jwtHelper;
        }

        public IQueryable<UserModel> GetAll()
        {
            return context.Users
                .Where(x => !x.Deleted && x.UserType != UserType.Member)
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

        public User GetById(int id)
        {
            return context.Users.FirstOrDefault(x => x.Id == id && !x.Deleted);
        }

        public ServiceResult Authenticate(LoginModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var hassPassword = Security.MD5Crypt(model.Password);
            var user = context.Users
                .Include(x => x.UserAccessRights)
                .ThenInclude(x => x.AccessRight)
                .FirstOrDefault(x =>
            x.EmailAddress == model.EmailAddress &&
            x.Password == hassPassword && !x.Deleted);

            if (user == null)
            {
                throw new NotFoundException("Email adresi veya şifre hatalıdır.");
            }

            if (!user.IsActive)
            {
                throw new BadRequestException("Hesabınız aktif değildir.");
            }

            var tokenResult = jwtHelper.GenerateJwtToken(user);

            user.Token = tokenResult.Token;
            user.TokenExpireDate = tokenResult.ExpireDate;
            context.SaveChanges();
            List<AccessRight> accessRights = new List<AccessRight>();

            if (user.UserType == UserType.SuperAdmin)
            {
                accessRights = context.AccessRights.ToList();
            }
            else
            {
                if (user.UserAccessRights != null && user.UserAccessRights.Any())
                {
                    accessRights = user.UserAccessRights.Select(x => x.AccessRight).ToList();
                }
            }

            serviceResult.Data = new
            {
                Token = tokenResult.Token,
                FullName = $"{user.Name} {user.Surname}",
                IsAccessAdminPanel = user.UserType != UserType.Member ? true : false,
                OperationAccessRights = accessRights.Where(x => !string.IsNullOrEmpty(x.Endpoint) && !x.Deleted && x.IsActive && x.Type == AccessRightType.Operation)
                .Select(x => x.Endpoint).ToList(),
                MenuAccessRights = accessRights.Where(x => !string.IsNullOrEmpty(x.Endpoint) && !x.Deleted && x.IsActive && x.Type == AccessRightType.Menu)
                .Select(x => x.Endpoint).ToList()
            };
            return serviceResult;
        }

        public User GetTokenInfo(int userId, string token)
        {
            var user = context.Users
                .Include(x => x.UserAccessRights)
                .FirstOrDefault(x => !x.Deleted && x.IsActive && x.Id == userId && x.Token == token && x.TokenExpireDate >= DateTime.Now);
            return user;
        }

        public List<LookupModel> Lookup()
        {
            return context.Users
                .Where(x => !x.Deleted && x.IsActive && x.UserType != UserType.Member)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name + " " + x.Surname
                }).ToList();
        }

        public ServiceResult Post(UserModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var checkEmail = context.Users
                .Any(x => x.EmailAddress == model.EmailAddress &&
                !x.Deleted);

            if (!checkEmail)
            {
                var user = new User
                {
                    Deleted = false,
                    EmailAddress = model.EmailAddress,
                    IsActive = model.IsActive,
                    Name = model.Name,
                    Surname = model.Surname,
                    UserType = (UserType)model.UserType,
                    InsertedDate = DateTime.Now
                };
                context.Users.Add(user);
                context.SaveChanges();
            }
            else
            {
                throw new FoundException("Email adresi ile daha önce kullanıcı kaydedilmiş.");
            }
            return serviceResult;
        }

        public ServiceResult Put(UserModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var isExistUser = context.Users
                       .Any(x => x.EmailAddress == model.EmailAddress &&
                       !x.Deleted && x.Id != model.Id);

            if (!isExistUser)
            {
                var user = context.Users.FirstOrDefault(x => !x.Deleted && x.Id == model.Id);
                if (user != null)
                {
                    user.EmailAddress = model.EmailAddress;
                    user.IsActive = model.IsActive;
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    user.UserType = (UserType)model.UserType;
                    context.SaveChanges();
                }
                else
                {
                    throw new NotFoundException("Kayıt bulunamadı.");
                }
            }
            else
            {
                throw new FoundException("Email adresi ile daha önce kullanıcı kaydedilmiş.");
            }
            return serviceResult;
        }

        public ServiceResult Delete(int id)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var user = context.Users.FirstOrDefault(x => x.Id == id && !x.Deleted);

            if (user != null)
            {
                user.Deleted = true;
                context.SaveChanges();
            }
            else
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            return serviceResult;
        }
    }
}
