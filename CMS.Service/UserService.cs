using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Consts;
using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using CMS.Service.Helper;
using CMS.Service.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IUserService
    {
        IQueryable<UserModel> GetAll();

        User GetById(int id);

        Task<ServiceResult> Authenticate(LoginModel model);

        User GetTokenInfo(int userId, string token);

        Task<ServiceResult> Post(UserModel model);

        ServiceResult Put(UserModel model);

        ServiceResult Delete(int id);

        UserProfileModel GetProfile();

        ServiceResult UpdateProfile(UserProfileModel model);

        Task<ServiceResult> AddMember(AddMemberModel model);

        Task<ServiceResult> ForgotPassword(ForgotPasswordModel model);

        ServiceResult ChangePassword(ChangePasswordModel model);

        ServiceResult EmailVerified(string code);

        ResetPasswordInfoModel GetUserByCode(string code);

        ServiceResult ResetPassword(ResetPasswordModel model);
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;
        private readonly IJwtHelper _jwtHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IMemoryCache _memoryCache;

        public UserService(IUnitOfWork<CMSContext> unitOfWork,
            IJwtHelper jwtHelper,
            IMailHelper mailHelper,
            IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _jwtHelper = jwtHelper;
            _mailHelper = mailHelper;
            _memoryCache = memoryCache;
        }

        public IQueryable<UserModel> GetAll()
        {
            return _unitOfWork.Repository<User>()
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.Id)
                .Select(x => new UserModel
                {
                    UserType = (int)x.UserType,
                    UserTypeName = EnumHelper.GetDescription<UserType>(x.UserType),
                    EmailAddress = x.EmailAddress,
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname,
                    IsActive = x.IsActive,
                    Status = EnumHelper.GetDescription<UserStatus>(x.Status),
                });
        }

        public User GetById(int id)
        {
            return _unitOfWork.Repository<User>().FirstOrDefault(x => x.Id == id && !x.Deleted);
        }

        public async Task<ServiceResult> Authenticate(LoginModel model)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var hassPassword = Security.MD5Crypt(model.Password);
            var user = _unitOfWork.Repository<User>()
                .Where(x => x.EmailAddress == model.EmailAddress && x.Password == hassPassword && !x.Deleted)
                .Include(x => x.UserAccessRights)
                .ThenInclude(x => x.AccessRight)
                .ThenInclude(x => x.AccessRightEndpoints)
                .FirstOrDefault();

            if (user == null)
            {
                throw new NotFoundException("Email adresi veya şifre hatalıdır.");
            }

            if (!user.IsActive)
            {
                throw new BadRequestException("Hesabınız aktif değildir.");
            }

            if (user.Status == UserStatus.EmailNotVerified)
            {
                throw new BadRequestException("Email adresiniz doğrulanmamış.");
            }

            if (user.PasswordExpireDate < DateTime.Now)
            {
                result = await SendMailResetPassword(user);
                throw new ForbiddenException("Şifre geçerlilik süresi dolmuş. Mail adresinize şifre belirleme linki gönderildi.");
            }

            var tokenResult = _jwtHelper.GenerateJwtToken(user);

            user.Token = tokenResult.Token;
            user.TokenExpireDate = tokenResult.ExpireDate;
            _unitOfWork.Save();
            List<AccessRight> accessRights = new List<AccessRight>();

            if (user.UserType == UserType.SuperAdmin)
            {
                accessRights = _unitOfWork.Repository<AccessRight>()
                    .Where(x => !x.Deleted)
                    .Include(x => x.AccessRightEndpoints).ToList();
            }
            else
            {
                if (user.UserAccessRights != null && user.UserAccessRights.Any())
                {
                    accessRights = user.UserAccessRights
                        .Select(x => x.AccessRight)
                        .Where(x => !x.Deleted && x.IsActive).ToList();
                }
            }

            result.Data = new TokenResponseModel()
            {
                Token = tokenResult.Token,
                FullName = user.FullName,
                IsAccessAdminPanel = user.UserType != UserType.Member ? true : false,
                OperationAccessRights = accessRights.Where(x => x.Type == AccessRightType.Operation)
                                                    .SelectMany(x => x.AccessRightEndpoints)
                                                    .Select(x => x.Endpoint).ToList(),
                MenuAccessRights = accessRights.Where(x => x.Type == AccessRightType.Menu)
                                                    .SelectMany(x => x.AccessRightEndpoints)
                                                    .Select(x => x.Endpoint).ToList()
            };
            return result;
        }

        public User GetTokenInfo(int userId, string token)
        {
            var user = _unitOfWork.Repository<User>()
                .Where(x => !x.Deleted && x.IsActive && x.Id == userId && x.Token == token && x.TokenExpireDate >= DateTime.Now)
                .Include(x => x.UserAccessRights)
                .FirstOrDefault();
            return user;
        }

        public async Task<ServiceResult> Post(UserModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var checkEmail = _unitOfWork.Repository<User>()
                .Any(x => x.Id != model.Id && x.EmailAddress == model.EmailAddress && !x.Deleted);

            if (checkEmail)
            {
                throw new FoundException("Email adresi ile daha önce kullanıcı kaydedilmiş.");
            }

            var user = new User
            {
                Deleted = false,
                EmailAddress = model.EmailAddress,
                IsActive = model.IsActive,
                Name = model.Name,
                Surname = model.Surname,
                UserType = (UserType)model.UserType,
                InsertedDate = DateTime.Now,
                HashCode = Security.RandomBase64(),
                Status = UserStatus.NotSetPassword
            };
            _unitOfWork.Repository<User>().Add(user);
            _unitOfWork.Save();

            // kullanıcıya email gönderiliyor                
            result = await SendMailResetPassword(user);
            result.Message = AlertMessages.Post;
            return result;
        }

        public ServiceResult Put(UserModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var isExistUser = _unitOfWork.Repository<User>()
                .Any(x => x.EmailAddress == model.EmailAddress && !x.Deleted && x.Id != model.Id);

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

            if (user.UserType != UserType.Member)
            {
                _memoryCache.Remove($"userMenu_{AuthTokenContent.Current.UserId}");
            }
            result.Message = AlertMessages.Put;
            return result;
        }

        public ServiceResult Delete(int id)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var user = GetById(id);

            if (user == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            user.Deleted = true;
            _unitOfWork.Save();

            if (user.UserType != UserType.Member)
            {
                _memoryCache.Remove($"userMenu_{AuthTokenContent.Current.UserId}");
            }
            result.Message = AlertMessages.Delete;
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
                throw new NotFoundException(AlertMessages.NotFound);
            }
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.UpdatedDate = DateTime.Now;
            _unitOfWork.Save();
            result.Message = AlertMessages.Put;
            return result;
        }

        public async Task<ServiceResult> AddMember(AddMemberModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            var isExistEmail = _unitOfWork.Repository<User>().Any(x => x.EmailAddress == model.EmailAddress && !x.Deleted);

            if (isExistEmail)
            {
                throw new FoundException($"{model.EmailAddress} mail adresiyle daha önce üye mevcuttur. Lütfen tekrar deneyiniz.");
            }
            var user = new User()
            {
                Deleted = false,
                EmailAddress = model.EmailAddress,
                HashCode = Security.RandomBase64(),
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

            //kullanıcıya email gönderiliyor
            var emailVerifyTemplateModel = new TemplateModel
            {
                FullName = user.FullName,
                Url = $"{Global.UIUrl}email-dogrulama/{user.HashCode}"
            };

            result = await _mailHelper.SendWithTemplate(new MailWithTemplateModel()
            {
                EmailAddress = model.EmailAddress,
                TemplateType = TemplateType.EmailVerificationLink,
                Data = emailVerifyTemplateModel
            });
            result.Message = "Email adresinize onay maili gönderilmiştir.";
            _unitOfWork.Save();
            return result;
        }

        public async Task<ServiceResult> ForgotPassword(ForgotPasswordModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            var user = _unitOfWork.Repository<User>()
                .FirstOrDefault(x => x.EmailAddress == model.EmailAddress && !x.Deleted);

            if (user != null)
            {
                user.HashCode = Security.RandomBase64();
                user.Status = UserStatus.NotSetPassword;
                user.UpdatedDate = DateTime.Now;
                _unitOfWork.Save();

                // kullanıcıya mail gönderiliyor
                result = await SendMailResetPassword(user);
            }
            result.Message = "Email adresiniz mevcutsa şifre belirleme linki gönderilmiştir.";
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
                throw new BadRequestException("Mevcut şifreniz hatalıdır.");
            }
            user.Password = Security.MD5Crypt(model.NewPassword);
            user.PasswordExpireDate = DateTime.Now.AddMonths(3);
            user.UpdatedDate = DateTime.Now;
            user.HashCode = String.Empty;
            _unitOfWork.Save();
            return result;
        }

        public ServiceResult EmailVerified(string code)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            var user = _unitOfWork.Repository<User>().FirstOrDefault(x => x.HashCode == code && x.Status == UserStatus.EmailNotVerified);
            if (user == null)
            {
                throw new BadRequestException(AlertMessages.UserNotFound);
            }
            user.HashCode = String.Empty;
            user.Status = UserStatus.Active;
            _unitOfWork.Save();
            return result;
        }

        public ServiceResult ResetPassword(ResetPasswordModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            var user = _unitOfWork.Repository<User>().FirstOrDefault(x => x.HashCode == model.Code && x.Status == UserStatus.NotSetPassword);
            if (user == null)
            {
                throw new BadRequestException(AlertMessages.UserNotFound);
            }
            if (model.NewPassword != model.ReNewPassword)
            {
                throw new BadRequestException("Şifre alanları uyuşmamaktadır.");
            }
            user.Password = Security.MD5Crypt(model.NewPassword);
            user.PasswordExpireDate = DateTime.Now.AddMonths(3);
            user.UpdatedDate = DateTime.Now;
            user.HashCode = String.Empty;
            user.Status = UserStatus.Active;
            _unitOfWork.Save();
            result.Message = AlertMessages.SuccessResetPassword;
            return result;
        }

        private async Task<ServiceResult> SendMailResetPassword(User user)
        {
            var forgotPasswordTemplateModel = new TemplateModel
            {
                FullName = user.FullName,
                Url = $"{Global.UIUrl}sifre-belirle/{user.HashCode}"
            };

            var result = await _mailHelper.SendWithTemplate(new MailWithTemplateModel()
            {
                EmailAddress = user.EmailAddress,
                TemplateType = TemplateType.SetPasswordLink,
                Data = forgotPasswordTemplateModel
            });
            return result;
        }

        public ResetPasswordInfoModel GetUserByCode(string code)
        {
            var user = _unitOfWork.Repository<User>().FirstOrDefault(x => x.HashCode == code && x.Status == UserStatus.NotSetPassword);
            if (user == null)
            {
                throw new BadRequestException(AlertMessages.UserNotFound);
            }
            var model = new ResetPasswordInfoModel()
            {
                FullName = $"{user.Name} {user.Surname}"
            };
            return model;
        }
    }
}
