using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Service.Exceptions;
using CMS.Storage.Consts;
using CMS.Storage.Entity;
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
    public interface ITestimonialService
    {
        IQueryable<Testimonial> GetAll();
        Task<List<TestimonialModel>> GetAllActive(int? top = null);
        Task<TestimonialModel> GetById(int id);
        Task<ServiceResult> Post(TestimonialModel model);
        Task<ServiceResult> Put(TestimonialModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class TestimonialService : ITestimonialService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public TestimonialService(
            IUnitOfWork<CMSContext> unitOfWork,
            IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Delete };

            var testimonial = await _unitOfWork.Repository<Testimonial>()
                .FirstOrDefault(c => c.Id == id);
            if (testimonial == null)
            {
                throw new NotFoundException("Kayıt bulunamadı");
            }
            testimonial.Deleted = true;

            _unitOfWork.Repository<Testimonial>().Update(testimonial);
            await _unitOfWork.Save();

            return result;
        }

        public IQueryable<Testimonial> GetAll()
        {
            var list = _unitOfWork.Repository<Testimonial>()
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.Id).AsQueryable();

            return list;
        }

        public Task<List<TestimonialModel>> GetAllActive(int? top = null)
        {
            var data = GetAll().Where(x => x.IsActive);

            if (top != null)
            {
                data = data.Take(top.Value);
            }

            var list = data.Select(x => new TestimonialModel
            {
                Id = x.Id,
                Name = x.Name,
                CorporateName = x.CorporateName,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Surname = x.Surname,
                Title = x.Title
            }).ToListAsync();

            return list;
        }

        public async Task<TestimonialModel> GetById(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Delete };

            var testimonial = await _unitOfWork.Repository<Testimonial>().FirstOrDefault(c => c.Id == id);

            if (testimonial == null)
            {
                throw new NotFoundException("Kayıt bulunamadı");
            }
            return new TestimonialModel
            {
                Description = testimonial.Description,
                Id = id,
                ImageUrl = testimonial.ImageUrl,
                Name = testimonial.Name,
                Surname = testimonial.Surname,
                IsActive = testimonial.IsActive,
                CorporateName = testimonial.CorporateName,
                Title = testimonial.Title
            };
        }

        public async Task<ServiceResult> Post(TestimonialModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Post };

            var extension = Path.GetExtension(model.Image.FileName);
            string imageUrl = $"/images/services/{Guid.NewGuid()}{extension}";
            var fileUploadUrl = $"{_environment.WebRootPath}{imageUrl}";
            model.Image.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));

            await _unitOfWork.Repository<Testimonial>()
                .Add(new()
                {
                    Deleted = false,
                    IsActive = model.IsActive,
                    Description = model.Description,
                    Name = model.Name,
                    CorporateName = model.CorporateName,
                    Surname = model.Surname,
                    Title = model.Title,
                    ImageUrl = imageUrl
                });

            await _unitOfWork.Save();

            return result;
        }

        public async Task<ServiceResult> Put(TestimonialModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Put };

            var testimonial = await _unitOfWork.Repository<Testimonial>().FirstOrDefault(x => !x.Deleted && x.Id == model.Id);

            if (testimonial == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            
            if (model.Image != null)
            {
                var currentFileUrl = Path.Combine(_environment.WebRootPath, testimonial.ImageUrl);

                if (File.Exists(currentFileUrl))
                {
                    File.Delete(currentFileUrl);
                }
                var extension = Path.GetExtension(model.Image.FileName);
                string imageUrl = $"/images/blogs/{Guid.NewGuid()}{extension}";
                var fileUploadUrl = $"{_environment.WebRootPath}{imageUrl}";
                model.Image.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));

                testimonial.ImageUrl = imageUrl;
            }

            testimonial.Name = model.Name;
            testimonial.Surname = model.Surname;
            testimonial.IsActive = model.IsActive;
            testimonial.Description = model.Description;
            testimonial.CorporateName = model.CorporateName;
            testimonial.Title = model.Title;

            await _unitOfWork.Save();

            return result;
        }
    }
}
