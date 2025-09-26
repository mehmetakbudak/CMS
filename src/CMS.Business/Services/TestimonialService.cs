using CMS.Business.Exceptions;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Dtos.Testimonial;
using CMS.Storage.Entity;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface ITestimonialService
    {
        IQueryable<TestimonialListDto> Get();      
        Task<TestimonialDto> GetById(int id);
        Task<ServiceResult> Create(TestimonialDto model);
        Task<ServiceResult> Update(TestimonialDto model);
        Task<ServiceResult> Delete(int id);
    }

    public class TestimonialService(
            IUnitOfWork<CMSContext> unitOfWork,
            IWebHostEnvironment environment) : ITestimonialService
    {        
        public IQueryable<TestimonialListDto> Get()
        {
            return unitOfWork.Repository<Testimonial>()
                .Where(x => !x.Deleted)
                .Select(x => new TestimonialListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    CorporateName = x.CorporateName,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    Surname = x.Surname,
                    Title = x.Title,
                    IsActive = x.IsActive
                }).AsQueryable();            
        }        

        public async Task<TestimonialDto> GetById(int id)
        {
            var testimonial = await unitOfWork.Repository<Testimonial>()
                .FirstOrDefault(c => c.Id == id);

            if (testimonial is null)
                throw new NotFoundException("Kayıt bulunamadı");

            return new TestimonialDto
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

        public async Task<ServiceResult> Create(TestimonialDto model)
        {
            var extension = Path.GetExtension(model.Image.FileName);

            string imageUrl = $"/images/services/{Guid.NewGuid()}{extension}";

            var fileUploadUrl = $"{environment.WebRootPath}{imageUrl}";

            model.Image.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));

            await unitOfWork.Repository<Testimonial>()
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

            await unitOfWork.Save();

            return ServiceResult.Success(204);
        }

        public async Task<ServiceResult> Update(TestimonialDto model)
        {
            var testimonial = await unitOfWork.Repository<Testimonial>()
                .FirstOrDefault(x => !x.Deleted && x.Id == model.Id);

            if (testimonial is null)
                throw new NotFoundException("Kayıt bulunamadı.");

            if (model.Image != null)
            {
                var currentFileUrl = Path.Combine(environment.WebRootPath, testimonial.ImageUrl);

                if (File.Exists(currentFileUrl))
                {
                    File.Delete(currentFileUrl);
                }
                var extension = Path.GetExtension(model.Image.FileName);
                string imageUrl = $"/images/blogs/{Guid.NewGuid()}{extension}";
                var fileUploadUrl = $"{environment.WebRootPath}{imageUrl}";
                model.Image.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));

                testimonial.ImageUrl = imageUrl;
            }

            testimonial.Name = model.Name;
            testimonial.Surname = model.Surname;
            testimonial.IsActive = model.IsActive;
            testimonial.Description = model.Description;
            testimonial.CorporateName = model.CorporateName;
            testimonial.Title = model.Title;

            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var testimonial = await unitOfWork.Repository<Testimonial>()
                .FirstOrDefault(c => c.Id == id);

            if (testimonial is null)
                throw new NotFoundException("Kayıt bulunamadı");

            testimonial.Deleted = true;

            unitOfWork.Repository<Testimonial>().Update(testimonial);
            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }
    }
}
