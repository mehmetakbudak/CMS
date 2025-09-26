using CMS.Business.Exceptions;
using CMS.Business.Helper;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Dtos.Service;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IService_Service
    {
        IQueryable<ServiceListDto> Get();
        Task<ServiceDetailDto> GetByUrl(string url);
        Task<ServiceDto> GetById(int id);
        Task<ServiceResult> Create(ServiceDto model);
        Task<ServiceResult> Update(ServiceDto model);
        Task<ServiceResult> Delete(int id);
    }

    public class Service_Service(
            IUnitOfWork<CMSContext> unitOfWork,
            IWebHostEnvironment environment) : IService_Service
    {
        public IQueryable<ServiceListDto> Get()
        {
            var list = unitOfWork.Repository<Storage.Entity.Service>()
                .Where(x => !x.Deleted)
                .Select(x => new ServiceListDto
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    IsActive = x.IsActive,
                    Name = x.Name,
                    Url = x.Url
                }).AsQueryable();

            return list;
        }

        public async Task<ServiceDto> GetById(int id)
        {
            var service = await unitOfWork.Repository<Storage.Entity.Service>()
                .FirstOrDefault(c => c.Id == id);

            if (service is null)
                throw new NotFoundException("Service.NotFound");

            return new ServiceDto
            {
                Content = service.Content,
                Id = id,
                ImageUrl = service.ImageUrl,
                Name = service.Name,
                Url = service.Url,
                IsActive = service.IsActive
            };
        }

        public async Task<ServiceDetailDto> GetByUrl(string url)
        {
            var service = await unitOfWork.Repository<Storage.Entity.Service>()
                .FirstOrDefault(x => !x.Deleted && x.Url == url);

            if (service is null)
                throw new NotFoundException("Service.NotFound");

            return new ServiceDetailDto
            {
                Content = service.Content,
                Id = service.Id,
                ImageUrl = service.ImageUrl,
                Name = service.Name,
                Url = service.Url
            };
        }

        public async Task<ServiceResult> Create(ServiceDto model)
        {
            model.Url = UrlHelper.FriendlyUrl(model.Name);

            var isExist = await unitOfWork.Repository<Storage.Entity.Service>()
                .Any(x => !x.Deleted && x.Url == model.Url);

            if (isExist)
                throw new FoundException("AlreadyExistWithUrl");

            var extension = Path.GetExtension(model.Image.FileName);
            string imageUrl = $"/images/services/{Guid.NewGuid()}{extension}";
            var fileUploadUrl = $"{environment.WebRootPath}{imageUrl}";
            model.Image.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));

            await unitOfWork.Repository<Storage.Entity.Service>().Add(new()
            {
                Deleted = false,
                IsActive = model.IsActive,
                Content = model.Content,
                Name = model.Name,
                Url = model.Url,
                ImageUrl = imageUrl
            });

            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Update(ServiceDto model)
        {
            var service = await unitOfWork.Repository<Storage.Entity.Service>()
                .FirstOrDefault(x => !x.Deleted && x.Id == model.Id);

            if (service is null)
                throw new NotFoundException("Service.NotFound");

            model.Url = UrlHelper.FriendlyUrl(model.Name);

            var isExist = await unitOfWork.Repository<Storage.Entity.Service>()
                .Any(x => !x.Deleted && x.Url == model.Url && x.Id != model.Id);

            if (isExist)
                throw new FoundException("AlreadyExistWithUrl");

            if (model.Image != null)
            {
                var currentFileUrl = Path.Combine(environment.WebRootPath, service.ImageUrl);

                if (File.Exists(currentFileUrl))
                {
                    File.Delete(currentFileUrl);
                }
                var extension = Path.GetExtension(model.Image.FileName);
                string imageUrl = $"/images/services/{Guid.NewGuid()}{extension}";
                var fileUploadUrl = $"{environment.WebRootPath}{imageUrl}";
                model.Image.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));

                service.ImageUrl = imageUrl;
            }

            service.Name = model.Name;
            service.Url = model.Url;
            service.IsActive = model.IsActive;
            service.Content = model.Content;

            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var service = await unitOfWork.Repository<Storage.Entity.Service>()
                .FirstOrDefault(c => c.Id == id);

            if (service is null)
                throw new NotFoundException("Service.NotFound");

            service.Deleted = true;

            unitOfWork.Repository<Storage.Entity.Service>().Update(service);
            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }
    }
}
