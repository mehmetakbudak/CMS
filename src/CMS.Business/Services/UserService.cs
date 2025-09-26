using CMS.Business.Exceptions;
using CMS.Business.Extensions;
using CMS.Business.Helper;
using CMS.Business.Infrastructure;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos;
using CMS.Storage.Dtos.Mail;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Dtos.User;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IUserService
    {
        IQueryable<UserListDto> Get();
        Task<UserDto> GetById(int id);
        Task<ServiceResult> Create(UserDto model);
        Task<ServiceResult> Update(UserDto model);
        Task<ServiceResult> Delete(int id);
        Task<ServiceResult> SendMailResetPassword(User user);
    }

    public class UserService(IUnitOfWork<CMSContext> unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IJwtHelper jwtHelper,
            IMailHelper mailHelper,
            IMemoryCache memoryCache) : IUserService
    {
        public IQueryable<UserListDto> Get()
        {
            var result = unitOfWork.Repository<User>()
                .Where(x => !x.Deleted)
                .Select(x => new UserListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Phone = x.Phone,
                    Status = x.Status,
                    Surname = x.Surname,
                    IsActive = x.IsActive,
                    UserType = x.UserType,
                    UpdatedDate = x.UpdatedDate,
                    EmailAddress = x.EmailAddress,
                    InsertedDate = x.InsertedDate,
                    StatusName = x.Status.GetDescription(),
                    UserTypeName = x.UserType.GetDescription(),
                }).AsQueryable();

            return result;
        }

        public Task<UserDto> GetById(int id)
        {
            return unitOfWork.Repository<User>()
                .Where(x => x.Id == id && !x.Deleted)
                .Include(x => x.UserRoles)
                .Select(x => new UserDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Phone = x.Phone,
                    Status = x.Status,
                    Surname = x.Surname,
                    IsActive = x.IsActive,
                    UserType = x.UserType,
                    EmailAddress = x.EmailAddress,
                    RoleIds = x.UserRoles.Select(x => x.RoleId).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<ServiceResult> Create(UserDto model)
        {
            var strategy = unitOfWork.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await unitOfWork.CreateTransaction();

                var checkEmail = await unitOfWork.Repository<User>()
                .Any(x => x.EmailAddress == model.EmailAddress && !x.Deleted);

                if (checkEmail)
                {
                    await unitOfWork.Rollback();
                    throw new FoundException("Email adresi ile daha önce kullanıcı kaydedilmiş.");
                }

                var user = new User
                {
                    Deleted = false,
                    Name = model.Name,
                    Phone = model.Phone,
                    Surname = model.Surname,
                    IsActive = model.IsActive,
                    InsertedDate = DateTime.Now,
                    EmailAddress = model.EmailAddress,
                    Status = UserStatus.NotSetPassword,
                    UserType = (UserType)model.UserType,
                    HashCode = SecurityHelper.RandomBase64()
                };

                await unitOfWork.Repository<User>().Add(user);

                await unitOfWork.Save();

                var result = await SendMailResetPassword(user);

                if (!result.IsSuccessful)
                {
                    throw new BadRequestException("An error occurred while sending the email.");
                }

                if (model.UserType == UserType.Admin)
                {
                    foreach (var roleId in model.RoleIds)
                    {
                        await unitOfWork.Repository<UserRole>().Add(new UserRole
                        {
                            RoleId = roleId,
                            UserId = user.Id
                        });
                    }
                    await unitOfWork.Save();
                }
                await unitOfWork.Commit();
            });

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Update(UserDto model)
        {
            var loginUser = httpContextAccessor.HttpContext.User.Parse();

            var isExistUser = await unitOfWork.Repository<User>()
                .Any(x => x.EmailAddress == model.EmailAddress && !x.Deleted && x.Id != model.Id);

            if (isExistUser)
                throw new FoundException("Email adresi ile daha önce kullanıcı kaydedilmiş.");

            var user = await unitOfWork.Repository<User>()
                .Where(x => x.Id == model.Id && !x.Deleted)
                .Include(x => x.UserRoles)
                .FirstOrDefaultAsync();

            if (user is null)
                throw new NotFoundException("User.Notfound");

            user.EmailAddress = model.EmailAddress;
            user.IsActive = model.IsActive;
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.UserType = model.UserType;
            user.Phone = model.Phone;
            user.UpdatedDate = DateTime.Now;

            if (model.Status.HasValue)
                user.Status = model.Status.Value;

            await unitOfWork.Save();

            if (model.UserType == UserType.Admin)
            {
                var addingList = model.RoleIds
                    .Where(x => !user.UserRoles.Select(x => x.RoleId)
                    .Contains(x)).ToList();

                if (addingList != null && addingList != null)
                {
                    foreach (var roleId in addingList)
                    {
                        await unitOfWork.Repository<UserRole>().Add(new UserRole
                        {
                            RoleId = roleId,
                            UserId = user.Id
                        });
                    }
                }

                var deletingList = user.UserRoles.Where(x => !model.RoleIds.Contains(x.RoleId)).ToList();

                if (deletingList != null && deletingList.Any())
                {
                    foreach (var userRole in deletingList)
                    {
                        await unitOfWork.Repository<UserRole>().Delete(userRole);
                    }
                }
            }
            else
            {
                unitOfWork.Repository<UserRole>().DeleteRange(user.UserRoles);
            }

            await unitOfWork.Save();

            if (user.UserType != UserType.Member)
            {
                memoryCache.Remove($"userMenu_{model.Id}");
            }
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var user = await unitOfWork.Repository<User>()
                .FirstOrDefault(x => x.Id == id && !x.Deleted);

            if (user is null)
                throw new NotFoundException("User.Notfound");

            user.Deleted = true;

            await unitOfWork.Save();

            if (user.UserType != UserType.Member)
            {
                memoryCache.Remove($"userMenu_{id}");
            }
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> SendMailResetPassword(User user)
        {
            var forgotPasswordTemplateModel = new TemplateDto
            {
                FullName = user.FullName,
                Url = $"{Global.WebUrl}set-password/{user.HashCode}"
            };

            var result = await mailHelper.SendWithTemplate(new MailWithTemplateDto
            {
                EmailAddress = user.EmailAddress,
                TemplateType = TemplateType.SetPasswordLink,
                Data = forgotPasswordTemplateModel
            });
            return result;
        }
    }
}
