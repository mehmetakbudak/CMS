using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Service.Exceptions;
using CMS.Service.Helper;
using CMS.Storage.Consts;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using CMS.Storage.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IUserFileService
    {
        Task<List<UserFileModel>> GetByType(int type);
        Task<ServiceResult> Post(UserFilePostModel model);
        Task<ServiceResult> SetDefault(int id);
        Task<ServiceResult> Delete(int id);
    }

    public class UserFileService : IUserFileService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _environment;

        public UserFileService(
            IUnitOfWork<CMSContext> unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        public async Task<List<UserFileModel>> GetByType(int type)
        {
            var loginUser = _httpContextAccessor.HttpContext.User.Parse();

            return await _unitOfWork.Repository<UserFile>()
                .Where(x => x.UserId == loginUser.UserId && x.FileType == (UserFileType)type)
                .OrderByDescending(x => x.Id)
                .Select(x => new UserFileModel
                {
                    Id = x.Id,
                    FileName = x.FileName,
                    FileType = (UserFileType)type,
                    FileUrl = x.FileUrl,
                    InsertedDate = x.InsertedDate,
                    IsDefault = x.IsDefault
                }).ToListAsync();
        }

        public async Task<ServiceResult> Post(UserFilePostModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var strategy = _unitOfWork.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                try
                {
                    if (model == null)
                    {
                        throw new NotFoundException("Model null olamaz.");
                    }
                    if (model.File == null)
                    {
                        throw new BadRequestException("Resim ekleyiniz.");
                    }

                    var extension = Path.GetExtension(model.File.FileName);
                    if (extension != ".pdf")
                    {
                        throw new BadRequestException("Sadece PDF dosyası eklenebilir.");
                    }
                    await _unitOfWork.CreateTransaction();

                    string fileUrl = $"/files/cv/{Guid.NewGuid()}{extension}";
                    var fileUploadUrl = $"{_environment.WebRootPath}{fileUrl}";
                    model.File.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));

                    var loginUser = _httpContextAccessor.HttpContext.User.Parse();

                    var userFiles = await _unitOfWork.Repository<UserFile>()
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
                    await _unitOfWork.Repository<UserFile>().Add(userFile);
                    await _unitOfWork.Save();
                    result.Message = AlertMessages.Post;
                }
                catch (Exception ex)
                {
                    await _unitOfWork.Rollback();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    await _unitOfWork.Commit();
                }
            });
            return result;
        }

        public async Task<ServiceResult> SetDefault(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Put };

            var loginUser = _httpContextAccessor.HttpContext.User.Parse();

            var userFile = await _unitOfWork.Repository<UserFile>()
                    .FirstOrDefault(x => x.UserId == loginUser.UserId && x.Id == id);

            if (userFile == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            var currentDefault = await _unitOfWork.Repository<UserFile>()
                    .FirstOrDefault(x => x.UserId == loginUser.UserId && x.FileType == userFile.FileType && x.IsDefault);

            if (currentDefault != null)
            {
                currentDefault.IsDefault = false;
            }

            userFile.IsDefault = true;

            await _unitOfWork.Save();

            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Delete };

            var loginUser = _httpContextAccessor.HttpContext.User.Parse();

            var userFile = await _unitOfWork.Repository<UserFile>()
                      .FirstOrDefault(x => x.Id == id && x.UserId == loginUser.UserId);

            if (userFile == null)
            {
                throw new NotFoundException("Kayıt bulunamadı!");
            }

            if (userFile.IsDefault)
            {
                throw new BadRequestException("Varsayılan silinemez!");
            }

            await _unitOfWork.Repository<UserFile>().Delete(userFile);
            await _unitOfWork.Save();

            var currentFileUrl = Path.Combine(_environment.WebRootPath, userFile.FileUrl);

            if (File.Exists(currentFileUrl))
            {
                File.Delete(currentFileUrl);
            }

            return result;
        }
    }
}
