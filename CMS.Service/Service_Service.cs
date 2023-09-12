using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Service.Exceptions;
using CMS.Service.Helper;
using CMS.Storage.Consts;
using CMS.Storage.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IService_Service
    {
        IQueryable<Storage.Entity.Service> GetAll();
        Task<List<ServiceModel>> GetAllActive();
        Task<Storage.Entity.Service> GetByUrl(string url);
        Task<ServiceModel> GetById(int id);
        Task<ServiceResult> Post(ServiceModel model);
        Task<ServiceResult> Put(ServiceModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class Service_Service : IService_Service
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public Service_Service(
            IUnitOfWork<CMSContext> unitOfWork,
            IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Delete };

            var service = await _unitOfWork.Repository<Storage.Entity.Service>()
                .FirstOrDefault(c => c.Id == id);
            if (service == null)
            {
                throw new NotFoundException("Kayıt bulunamadı");
            }
            service.Deleted = true;

            _unitOfWork.Repository<Storage.Entity.Service>().Update(service);
            await _unitOfWork.Save();

            return result;
        }

        public IQueryable<Storage.Entity.Service> GetAll()
        {
            var list = _unitOfWork.Repository<Storage.Entity.Service>()
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.Id).AsQueryable();

            return list;
        }

        public async Task<List<ServiceModel>> GetAllActive()
        {
            return await GetAll()
                .Where(x => x.IsActive)
                .Select(x => new ServiceModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageUrl = x.ImageUrl,
                    Url = x.Url,
                    Content = x.Content
                }).ToListAsync();
        }

        public async Task<ServiceModel> GetById(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Delete };

            var service = await _unitOfWork.Repository<Storage.Entity.Service>()
                .FirstOrDefault(c => c.Id == id);
            if (service == null)
            {
                throw new NotFoundException("Kayıt bulunamadı");
            }
            return new ServiceModel
            {
                Content = service.Content,
                Id = id,
                ImageUrl = service.ImageUrl,
                Name = service.Name,
                Url = service.Url,
                IsActive = service.IsActive
            };
        }

        public async Task<Storage.Entity.Service> GetByUrl(string url)
        {
            return await _unitOfWork.Repository<Storage.Entity.Service>()
                .FirstOrDefault(x => !x.Deleted && x.Url == url);
        }

        public async Task<ServiceResult> Post(ServiceModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Post };

            model.Url = UrlHelper.FriendlyUrl(model.Name);

            var isExist = await _unitOfWork.Repository<Storage.Entity.Service>()
                .Any(x => !x.Deleted && x.Url == model.Url);

            if (isExist)
            {
                throw new FoundException("Url ile daha önce kayıt mevcuttur.");
            }

            var extension = Path.GetExtension(model.Image.FileName);
            string imageUrl = $"/images/services/{Guid.NewGuid()}{extension}";
            var fileUploadUrl = $"{_environment.WebRootPath}{imageUrl}";
            model.Image.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));

            await _unitOfWork.Repository<Storage.Entity.Service>()
                .Add(new()
                {
                    Deleted = false,
                    IsActive = model.IsActive,
                    Content = model.Content,
                    Name = model.Name,
                    Url = model.Url,
                    ImageUrl = imageUrl
                });

            await _unitOfWork.Save();

            return result;
        }

        public async Task<ServiceResult> Put(ServiceModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Put };

            var service = await _unitOfWork.Repository<Storage.Entity.Service>()
                .FirstOrDefault(x => !x.Deleted && x.Id == model.Id);

            if (service == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            model.Url = UrlHelper.FriendlyUrl(model.Name);

            var isExist = await _unitOfWork.Repository<Storage.Entity.Service>()
                .Any(x => !x.Deleted && x.Url == model.Url && x.Id != model.Id);

            if (isExist)
            {
                throw new FoundException("Url ile daha önce kayıt mevcuttur.");
            }

            if (model.Image != null)
            {
                var currentFileUrl = Path.Combine(_environment.WebRootPath, service.ImageUrl);

                if (File.Exists(currentFileUrl))
                {
                    File.Delete(currentFileUrl);
                }
                var extension = Path.GetExtension(model.Image.FileName);
                string imageUrl = $"/images/blogs/{Guid.NewGuid()}{extension}";
                var fileUploadUrl = $"{_environment.WebRootPath}{imageUrl}";
                model.Image.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));

                service.ImageUrl = imageUrl;
            }

            service.Name = model.Name;
            service.Url = model.Url;
            service.IsActive = model.IsActive;
            service.Content = model.Content;

            await _unitOfWork.Save();

            return result;
        }
    }
}
