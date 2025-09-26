using CMS.Business.Exceptions;
using CMS.Business.Extensions;
using CMS.Business.Helper;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Dtos.UserFile;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IUserFileService
    {
        Task<List<UserFileDto>> GetByType(int type);
        IQueryable<UserFileDto> GetUserFiles(int id);
        Task<ServiceResult> Create(UserFileCreateDto model);
        Task<ServiceResult> SetDefault(int id);
        Task<ServiceResult> Delete(int id);
    }

    public class UserFileService(
            IUnitOfWork<CMSContext> unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment environment) : IUserFileService
    {
        public async Task<List<UserFileDto>> GetByType(int type)
        {
            var loginUser = httpContextAccessor.HttpContext.User.Parse();

            return await unitOfWork.Repository<UserFile>()
                .Where(x => x.UserId == loginUser.UserId && x.FileType == (UserFileType)type)
                .OrderByDescending(x => x.Id)
                .Select(x => new UserFileDto
                {
                    Id = x.Id,
                    FileUrl = x.FileUrl,
                    FileName = x.FileName,
                    IsDefault = x.IsDefault,
                    FileType = (UserFileType)type,
                    InsertedDate = x.InsertedDate,
                    UpdatedDate = x.UpdatedDate,
                    FileTypeName = x.FileType.GetDescription()
                }).ToListAsync();
        }

        public IQueryable<UserFileDto> GetUserFiles(int id)
        {
            var userFiles = unitOfWork.Repository<UserFile>()
                .Where(x => x.UserId == id)
                .Select(x => new UserFileDto
                {

                    Id = x.Id,
                    FileName = x.FileName,
                    FileType = x.FileType,
                    FileTypeName = x.FileType.GetDescription(),
                    FileUrl = x.FileUrl,
                    InsertedDate = x.InsertedDate,
                    UpdatedDate = x.UpdatedDate,
                    IsDefault = x.IsDefault
                }).AsQueryable();
            return userFiles;
        }

        public async Task<ServiceResult> Create(UserFileCreateDto model)
        {
            var strategy = unitOfWork.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                try
                {
                    if (model is null)
                        throw new NotFoundException("Model null olamaz.");

                    if (model.File is null)
                        throw new BadRequestException("Resim ekleyiniz.");

                    var extension = Path.GetExtension(model.File.FileName);

                    if (extension != ".pdf")
                        throw new BadRequestException("Sadece PDF dosyası eklenebilir.");

                    await unitOfWork.CreateTransaction();

                    string fileUrl = $"/files/cv/{Guid.NewGuid()}{extension}";
                    var fileUploadUrl = $"{environment.WebRootPath}{fileUrl}";
                    model.File.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));

                    var loginUser = httpContextAccessor.HttpContext.User.Parse();

                    var userFiles = await unitOfWork.Repository<UserFile>()
                    .Where(x => x.UserId == loginUser.UserId && x.FileType == model.FileType).ToListAsync();

                    if (userFiles.Count > 0)
                    {
                        if (model.IsDefault)
                        {
                            var isDefaultFile = userFiles.FirstOrDefault(x => x.IsDefault);
                            if (isDefaultFile != null)
                            {
                                isDefaultFile.IsDefault = false;
                            }
                        }
                    }
                    else
                    {
                        model.IsDefault = true;
                    }

                    var userFile = new UserFile
                    {
                        FileName = model.File.FileName,
                        FileType = model.FileType,
                        FileUrl = fileUrl,
                        InsertedDate = DateTime.Now,
                        IsDefault = model.IsDefault,
                        UserId = loginUser.UserId
                    };
                    await unitOfWork.Repository<UserFile>().Add(userFile);

                    await unitOfWork.Save();
                    await unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    await unitOfWork.Rollback();
                    throw new Exception(ex.Message);
                }
            });
            return ServiceResult.Success(204);
        }

        public async Task<ServiceResult> SetDefault(int id)
        {
            var loginUser = httpContextAccessor.HttpContext.User.Parse();

            var userFile = await unitOfWork.Repository<UserFile>()
                    .FirstOrDefault(x => x.UserId == loginUser.UserId && x.Id == id);

            if (userFile is null)
                throw new NotFoundException("Kayıt bulunamadı.");

            var currentDefault = await unitOfWork.Repository<UserFile>()
                    .FirstOrDefault(x => x.UserId == loginUser.UserId && x.FileType == userFile.FileType && x.IsDefault);

            if (currentDefault != null)
                currentDefault.IsDefault = false;

            userFile.IsDefault = true;
            userFile.UpdatedDate = DateTime.Now;

            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var loginUser = httpContextAccessor.HttpContext.User.Parse();

            var userFile = await unitOfWork.Repository<UserFile>()
                      .FirstOrDefault(x => x.Id == id && x.UserId == loginUser.UserId);

            if (userFile is null)
                throw new NotFoundException("Kayıt bulunamadı!");

            if (userFile.IsDefault)
                throw new BadRequestException("Varsayılan silinemez!");

            await unitOfWork.Repository<UserFile>().Delete(userFile);
            await unitOfWork.Save();

            var currentFileUrl = Path.Combine(environment.WebRootPath, userFile.FileUrl);

            if (File.Exists(currentFileUrl))
                File.Delete(currentFileUrl);

            return ServiceResult.Success(200);
        }
    }
}
