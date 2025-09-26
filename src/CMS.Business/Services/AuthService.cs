using CMS.Business.Exceptions;
using CMS.Business.Extensions;
using CMS.Business.Helper;
using CMS.Business.Infrastructure;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Consts;
using CMS.Storage.Dtos;
using CMS.Storage.Dtos.Auth;
using CMS.Storage.Dtos.Mail;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginDto model);
        Task<UserProfileDto> GetProfile();
        Task<ServiceResult> UpdateProfile(UserProfileDto model);
        Task<ServiceResult> Register(RegisterDto model);
        Task<ServiceResult> ForgotPassword(ForgotPasswordDto model);
        Task<ServiceResult> ChangePassword(ChangePasswordDto model);
        Task<ServiceResult> EmailVerified(string code);
        Task<ResetPasswordInfoDto> GetUserByCode(string code);
        Task<ServiceResult> ResetPassword(ResetPasswordDto model);
        Task Authorize(AuthorizeDto model);
        Task<LoginResponseDto> CreateTokenByRefreshToken(RefreshTokenDto model);
        Task<ServiceResult> RevokeRefreshToken(RefreshTokenDto model);
    }

    public class AuthService(
            IJwtHelper jwtHelper,
            IUnitOfWork<CMSContext> unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IMailTemplateService mailTemplateService,
            IMailHelper mailHelper,
            ILanguageHelper languageHelper,
            ISendEndpointProvider sendEndpointProvider) : IAuthService
    {
        public async Task<LoginResponseDto> Login(LoginDto model)
        {
            var hassPassword = SecurityHelper.MD5Crypt(model.Password);

            var user = await unitOfWork.Repository<User>()
                .FirstOrDefault(x => x.EmailAddress == model.EmailAddress &&
                                     x.Password == hassPassword && !x.Deleted);

            if (user is null)
                throw new NotFoundException(languageHelper.Translate("EmailOrPasswordWrong", "Email or password is wrong"));

            if (!user.IsActive)
                throw new BadRequestException("Hesabınız aktif değildir.");

            if (user.Status == UserStatus.EmailNotVerified)
                throw new BadRequestException("Email adresiniz doğrulanmamış.");

            if (user.UserType != UserType.SuperAdmin && user.PasswordExpireDate < DateTime.Now)
            {
                user.HashCode = SecurityHelper.RandomBase64();
                user.Status = UserStatus.NotSetPassword;
                await unitOfWork.Save();

                await userService.SendMailResetPassword(user);
                throw new ForbiddenException("Şifre geçerlilik süresi dolmuş. Mail adresinize şifre belirleme linki gönderildi.");
            }

            var jwtResult = jwtHelper.GenerateJwtToken(user);

            var userToken = await unitOfWork.Repository<UserToken>()
                .FirstOrDefault(x => x.UserId == user.Id);

            if (userToken is null)
            {
                userToken = new UserToken
                {
                    AccessToken = jwtResult.AccessToken,
                    AccessTokenExpireDate = jwtResult.AccessTokenExpireDate,
                    RefreshToken = jwtResult.RefreshToken,
                    RefreshTokenExpireDate = jwtResult.RefreshTokenExpireDate,
                    UserId = user.Id
                };
                await unitOfWork.Repository<UserToken>().Add(userToken);
            }
            else
            {
                userToken.AccessToken = jwtResult.AccessToken;
                userToken.AccessTokenExpireDate = jwtResult.AccessTokenExpireDate;
                userToken.RefreshToken = jwtResult.RefreshToken;
                userToken.RefreshTokenExpireDate = jwtResult.RefreshTokenExpireDate;

                unitOfWork.Repository<UserToken>().Update(userToken);
            }
            await unitOfWork.Save();

            var result = new LoginResponseDto
            {
                Id = user.Id,
                UserType = user.UserType,
                FullName = user.FullName,
                AccessToken = jwtResult.AccessToken,
                AccessTokenExpireDate = jwtResult.AccessTokenExpireDate,
                RefreshToken = jwtResult.RefreshToken,
                RefreshTokenExpireDate = jwtResult.RefreshTokenExpireDate
            };

            return result;
        }

        public async Task<UserProfileDto> GetProfile()
        {
            var loginUser = httpContextAccessor.HttpContext.User.Parse();

            var user = await unitOfWork.Repository<User>()
                .FirstOrDefault(x => x.Id == loginUser.UserId && !x.Deleted);

            if (user is null)
                throw new NotFoundException();

            var result = new UserProfileDto()
            {
                EmailAddress = user.EmailAddress,
                Name = user.Name,
                Surname = user.Surname,
                Phone = user.Phone
            };
            return result;
        }

        public async Task<ServiceResult> UpdateProfile(UserProfileDto model)
        {
            var loginUser = httpContextAccessor.HttpContext.User.Parse();

            var user = await unitOfWork.Repository<User>()
                .FirstOrDefault(x => x.Id == loginUser.UserId && !x.Deleted);

            if (user is null)
                throw new NotFoundException();

            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Phone = model.Phone;
            user.UpdatedDate = DateTime.Now;
            await unitOfWork.Save();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Register(RegisterDto model)
        {
            if (model.Password != model.RePassword)
                throw new BadRequestException("Şifre alanları uyuşmamaktadır.");

            var isExistEmail = await unitOfWork.Repository<User>()
                .Any(x => x.EmailAddress == model.EmailAddress && !x.Deleted);

            if (isExistEmail)
                throw new FoundException($"{model.EmailAddress} mail adresiyle üye mevcuttur.");

            var user = new User()
            {
                Deleted = false,
                EmailAddress = model.EmailAddress,
                HashCode = SecurityHelper.RandomBase64(),
                Name = model.Name,
                Surname = model.Surname,
                InsertedDate = DateTime.Now,
                IsActive = true,
                Password = SecurityHelper.MD5Crypt(model.Password),
                Phone = model.Phone,
                PasswordExpireDate = DateTime.Now.AddMonths(3),
                UserType = UserType.Member,
                Status = UserStatus.EmailNotVerified
            };

            await unitOfWork.Repository<User>().Add(user);

            //kullanıcıya email gönderiliyor
            var emailVerifyTemplateModel = new TemplateDto
            {
                FullName = user.FullName,
                Url = $"{Global.WebUrl}email-verify/{user.HashCode}"
            };

            var mailTemplate = await mailTemplateService.GetTemplateByType(emailVerifyTemplateModel, TemplateType.EmailVerificationLink);

            //if (mailTemplate != null)
            //{
            //    var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQEndpoint.EmailQueue}"));

            //    await sendEndpointProvider.Send(new MailDto
            //    {
            //        Body = mailTemplate.Body,
            //        EmailAddress = model.EmailAddress,
            //        Subject = mailTemplate.Subject
            //    });
            //}
            var result = await mailHelper.SendWithTemplate(new MailWithTemplateDto()
            {
                EmailAddress = model.EmailAddress,
                TemplateType = TemplateType.EmailVerificationLink,
                Data = emailVerifyTemplateModel
            });

            result.Message = "Email adresinize onay maili gönderilmiştir.";

            await unitOfWork.Save();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> ForgotPassword(ForgotPasswordDto model)
        {
            var result = new ServiceResult();

            var user = await unitOfWork.Repository<User>()
                .FirstOrDefault(x => x.EmailAddress == model.EmailAddress && !x.Deleted);

            if (user != null)
            {
                user.HashCode = SecurityHelper.RandomBase64();
                user.Status = UserStatus.NotSetPassword;
                user.UpdatedDate = DateTime.Now;

                await unitOfWork.Save();
                result = await userService.SendMailResetPassword(user);
            }
            if (result.IsSuccessful)
            {
                return ServiceResult.Success(StatusCodes.Status200OK, "Email adresiniz mevcutsa şifre belirleme linki gönderilmiştir.");
            }
            return result;
        }

        public async Task<ServiceResult> ChangePassword(ChangePasswordDto model)
        {
            if (model.NewPassword != model.ReNewPassword)
                throw new BadRequestException("");

            var loginUser = httpContextAccessor.HttpContext.User.Parse();

            var user = await unitOfWork.Repository<User>()
                .FirstOrDefault(x => x.Id == loginUser.UserId && !x.Deleted);

            if (user is null)
                throw new NotFoundException();

            var oldPassword = SecurityHelper.MD5Crypt(model.OldPassword);

            if (user.Password != oldPassword)
                throw new BadRequestException("Password.IsWrong");

            user.Password = SecurityHelper.MD5Crypt(model.NewPassword);
            user.PasswordExpireDate = DateTime.Now.AddMonths(3);
            user.UpdatedDate = DateTime.Now;
            user.HashCode = string.Empty;

            await unitOfWork.Save();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> EmailVerified(string code)
        {
            var user = await unitOfWork.Repository<User>()
                .FirstOrDefault(x => x.HashCode == code && x.Status == UserStatus.EmailNotVerified);

            if (user is null)
                throw new NotFoundException("User.NotFound");

            user.HashCode = string.Empty;
            user.Status = UserStatus.Active;

            await unitOfWork.Save();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await unitOfWork.Repository<User>()
                .FirstOrDefault(x => x.HashCode == model.Code && x.Status == UserStatus.NotSetPassword);

            if (user is null)
                throw new BadRequestException("User.NotFound");

            if (model.NewPassword != model.ReNewPassword)
                throw new BadRequestException("Password.NotMatched");

            user.Password = SecurityHelper.MD5Crypt(model.NewPassword);
            user.PasswordExpireDate = DateTime.Now.AddMonths(3);
            user.UpdatedDate = DateTime.Now;
            user.HashCode = string.Empty;
            user.Status = UserStatus.Active;
            await unitOfWork.Save();

            return ServiceResult.Success();
        }

        public async Task<ResetPasswordInfoDto> GetUserByCode(string code)
        {
            var user = await unitOfWork.Repository<User>()
                .FirstOrDefault(x => x.HashCode == code && x.Status == UserStatus.NotSetPassword);

            if (user is null)
                throw new NotFoundException("User.NotFound");

            var result = new ResetPasswordInfoDto
            {
                FullName = $"{user.Name} {user.Surname}",
                Code = code
            };
            return result;
        }

        public async Task Authorize(AuthorizeDto model)
        {
            var path = model.Path.Split("/");

            var endpoint = path[2];
            if (path.Length > 2)
            {
                endpoint = $"{path[2]}/{path[3]}";
            }
            Enum.TryParse(model.Method, out MethodType method);

            var accessRightEndpoint = await unitOfWork.Repository<AccessRightEndpoint>()
                .FirstOrDefault(x => x.Endpoint.ToLower() == endpoint.ToLower() &&
                                     x.Method == method);

            if (accessRightEndpoint is null)
                return;

            var accessRightId = accessRightEndpoint.AccessRightId;

            var userRoles = await unitOfWork.Repository<UserRole>()
                        .Where(x => x.UserId == model.UserId)
                        .Select(x => x.RoleId).ToListAsync();

            if (userRoles.Count == 0)
                throw new NotFoundException();

            var isCheck = await unitOfWork.Repository<RoleAccessRight>()
                .Any(x => userRoles.Contains(x.RoleId) && x.AccessRightId == accessRightId);

            if (!isCheck)
                throw new UnAuthorizedException();
        }

        public async Task<LoginResponseDto> CreateTokenByRefreshToken(RefreshTokenDto model)
        {
            var userToken = await unitOfWork.Repository<UserToken>()
                .FirstOrDefault(x => x.RefreshToken == model.RefreshToken);

            if (userToken is null)
                throw new NotFoundException("RefreshToken.NotFound");

            if (userToken.RefreshTokenExpireDate < DateTime.Now)
            {
                await RevokeRefreshToken(new RefreshTokenDto { RefreshToken = model.RefreshToken });
                throw new UnAuthorizedException("RefreshToken.Expired");
            }

            var user = await unitOfWork.Repository<User>()
                .FirstOrDefault(x => x.Id == userToken.UserId);

            if (user is null)
                throw new NotFoundException("User.NotFound");

            var jwtResult = jwtHelper.GenerateJwtToken(user);

            userToken.AccessToken = jwtResult.AccessToken;
            userToken.AccessTokenExpireDate = jwtResult.AccessTokenExpireDate;
            userToken.RefreshToken = jwtResult.RefreshToken;
            userToken.RefreshTokenExpireDate = jwtResult.RefreshTokenExpireDate;

            await unitOfWork.Save();

            var result = new LoginResponseDto
            {
                Id = user.Id,
                UserType = user.UserType,
                FullName = user.FullName,
                AccessToken = jwtResult.AccessToken,
                AccessTokenExpireDate = jwtResult.AccessTokenExpireDate,
                RefreshToken = jwtResult.RefreshToken,
                RefreshTokenExpireDate = jwtResult.RefreshTokenExpireDate
            };

            return result;
        }

        public async Task<ServiceResult> RevokeRefreshToken(RefreshTokenDto model)
        {
            var userToken = await unitOfWork.Repository<UserToken>()
                .FirstOrDefault(x => x.RefreshToken == model.RefreshToken);

            if (userToken is null)
                throw new NotFoundException("RefreshToken.NotFound");

            var loginUser = httpContextAccessor.HttpContext.User.Parse();

            if (userToken.UserId == loginUser.UserId)
            {
                await unitOfWork.Repository<UserToken>().Delete(userToken);
                await unitOfWork.Save();
                return ServiceResult.Success();
            }
            else
            {
                return ServiceResult.Fail(401, "Invalid.User");
            }
        }
    }
}
