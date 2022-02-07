using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Enum;
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
        UserProfileModel GetProfile();
        ServiceResult UpdateProfile(UserProfileModel model);
        ServiceResult AddMember(AddMemberModel model);
        ServiceResult ForgotPassword(ForgotPasswordModel model);
        ServiceResult ChangePassword(ChangePasswordModel model);
        object GetMemberComments();
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;
        private readonly IJwtHelper _jwtHelper;
        public UserService(IUnitOfWork<CMSContext> unitOfWork,
            IJwtHelper jwtHelper)
        {
            _unitOfWork = unitOfWork;
            _jwtHelper = jwtHelper;
        }

        public IQueryable<UserModel> GetAll()
        {
            return _unitOfWork.Repository<User>()
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

        public User GetById(int id)
        {
            return _unitOfWork.Repository<User>().Find(x => x.Id == id && !x.Deleted);
        }

        public ServiceResult Authenticate(LoginModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var hassPassword = Security.MD5Crypt(model.Password);
            var user = _unitOfWork.Repository<User>()
                .GetAll(x => x.EmailAddress == model.EmailAddress && x.Password == hassPassword && !x.Deleted)
                .Include(x => x.UserAccessRights)
                .ThenInclude(x => x.AccessRight)
                .FirstOrDefault();

            if (user == null)
            {
                throw new NotFoundException("Email adresi veya şifre hatalıdır.");
            }

            if (!user.IsActive)
            {
                throw new BadRequestException("Hesabınız aktif değildir.");
            }

            var tokenResult = _jwtHelper.GenerateJwtToken(user);

            user.Token = tokenResult.Token;
            user.TokenExpireDate = tokenResult.ExpireDate;
            _unitOfWork.Save();
            List<AccessRight> accessRights = new List<AccessRight>();

            if (user.UserType == UserType.SuperAdmin)
            {
                accessRights = _unitOfWork.Repository<AccessRight>()
                    .GetAll(x => !x.Deleted).ToList();
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
            var user = _unitOfWork.Repository<User>()
                .GetAll(x => !x.Deleted && x.IsActive && x.Id == userId && x.Token == token && x.TokenExpireDate >= DateTime.Now)
                .Include(x => x.UserAccessRights)
                .FirstOrDefault();
            return user;
        }

        public List<LookupModel> Lookup()
        {
            return _unitOfWork.Repository<User>()
                .GetAll(x => !x.Deleted && x.IsActive && x.UserType != UserType.Member)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name + " " + x.Surname
                }).ToList();
        }

        public ServiceResult Post(UserModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var checkEmail = _unitOfWork.Repository<User>()
                .GetAll(x => x.Id != model.Id && x.EmailAddress == model.EmailAddress && !x.Deleted).Any();

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
                    InsertedDate = DateTime.Now,
                    HashCode = Guid.NewGuid().ToString(),
                    Status = UserStatus.NotSetPassword
                };
                _unitOfWork.Repository<User>().Add(user);
                _unitOfWork.Save();
                // kullanıcıya email gönderiliyor
            }
            else
            {
                throw new FoundException("Email adresi ile daha önce kullanıcı kaydedilmiş.");
            }
            return result;
        }

        public ServiceResult Put(UserModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var isExistUser = _unitOfWork.Repository<User>()
                .GetAll(x => x.EmailAddress == model.EmailAddress && !x.Deleted && x.Id != model.Id)
                .Any();

            if (isExistUser)
            {
                throw new FoundException("Email adresi ile daha önce kullanıcı kaydedilmiş.");
            }

            var user = GetById(model.Id);
            if (user == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            user.EmailAddress = model.EmailAddress;
            user.IsActive = model.IsActive;
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.UserType = (UserType)model.UserType;
            _unitOfWork.Save();
            return result;
        }

        public ServiceResult Delete(int id)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var user = GetById(id);

            if (user == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            user.Deleted = true;
            _unitOfWork.Save();
            return result;
        }

        public UserProfileModel GetProfile()
        {
            UserProfileModel model = null;
            var user = GetById(AuthTokenContent.Current.UserId);
            if (user == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            model = new UserProfileModel()
            {
                EmailAddress = user.EmailAddress,
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname
            };

            return model;
        }

        public ServiceResult UpdateProfile(UserProfileModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            var user = GetById(AuthTokenContent.Current.UserId);
            if (user == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            var isExistEmail = _unitOfWork.Repository<User>()
                .GetAll(x => x.Id == model.Id && x.EmailAddress == model.EmailAddress && !x.Deleted).Any();
            if (isExistEmail)
            {
                throw new FoundException($"{model.EmailAddress} mail adresiyle daha önce üye mevcuttur. Lütfen tekrar deneyiniz.");
            }
            user.EmailAddress = model.EmailAddress;
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.UpdatedDate = DateTime.Now;
            _unitOfWork.Save();

            return result;
        }

        public ServiceResult AddMember(AddMemberModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            var isExistEmail = _unitOfWork.Repository<User>()
                .GetAll(x => x.EmailAddress == model.EmailAddress && !x.Deleted).Any();
            if (isExistEmail)
            {
                throw new FoundException($"{model.EmailAddress} mail adresiyle daha önce üye mevcuttur. Lütfen tekrar deneyiniz.");
            }
            var user = new User()
            {
                Deleted = false,
                EmailAddress = model.EmailAddress,
                HashCode = Guid.NewGuid().ToString(),
                Name = model.Name,
                Surname = model.Surname,
                InsertedDate = DateTime.Now,
                IsActive = true,
                Password = Security.MD5Crypt(model.Password),
                PasswordExpireDate = DateTime.Now.AddMonths(3),
                UserType = UserType.Member,
                Status = UserStatus.EmailNotVerified
            };
            _unitOfWork.Repository<User>().Add(user);
            _unitOfWork.Save();
            //kullanıcıya email gönderiliyor
            return result;
        }

        public ServiceResult ForgotPassword(ForgotPasswordModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            var user = _unitOfWork.Repository<User>()
                .Find(x => x.EmailAddress == model.EmailAddress && !x.Deleted);
            if (user == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            user.HashCode = Guid.NewGuid().ToString();
            user.Status = UserStatus.NotSetPassword;
            user.UpdatedDate = DateTime.Now;
            _unitOfWork.Save();
            // kullanıcıya mail gönderiliyor
            return result;
        }

        public ServiceResult ChangePassword(ChangePasswordModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            if (model.NewPassword != model.ReNewPassword)
            {
                throw new BadRequestException("Şifre alanları uyuşmamaktadır.");
            }
            var user = GetById(AuthTokenContent.Current.UserId);
            if (user == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            var oldPassword = Security.MD5Crypt(model.OldPassword);
            if (user.Password != oldPassword)
            {
                throw new BadRequestException("Eski şifreniz hatalıdır.");
            }
            user.Password = Security.MD5Crypt(model.NewPassword);
            user.PasswordExpireDate = DateTime.Now.AddMonths(3);
            user.UpdatedDate = DateTime.Now;
            user.HashCode = String.Empty;
            _unitOfWork.Save();

            return result;
        }

        public object GetMemberComments()
        {
            throw new NotImplementedException();
        }
    }
}
