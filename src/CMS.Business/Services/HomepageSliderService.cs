using CMS.Business.Exceptions;
using CMS.Business.Helper;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.HomepageSlider;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Dtos.Service;
using CMS.Storage.Entity;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IHomepageSliderService
    {
        IQueryable<HomepageSliderListDto> Get();
        Task<IEnumerable<HomepageSliderDto>> GetAllActive();
        Task<HomepageSliderDto> GetById(int id);
        Task<ServiceResult> Create(HomepageSliderDto dto);
        Task<ServiceResult> Update(HomepageSliderDto dto);
        Task<ServiceResult> Delete(int id);
    }

    public class HomepageSliderService(
        IDbConnection dbConnection,
        IWebHostEnvironment environment,
        IUnitOfWork<CMSContext> unitOfWork) : IHomepageSliderService
    {
        public IQueryable<HomepageSliderListDto> Get()
        {
            return unitOfWork.Repository<HomepageSlider>()
                .Where(x => !x.Deleted)
                .Select(x => new HomepageSliderListDto
                {
                    Id = x.Id,
                    Url = x.Url,
                    Title = x.Title,
                    IsActive = x.IsActive,
                    DisplayOrder = x.DisplayOrder
                });
        }

        public async Task<IEnumerable<HomepageSliderDto>> GetAllActive()
        {
            var list = await dbConnection.QueryAsync<HomepageSliderDto>("select [Id], [Description], [ImageUrl], [Title], [Url] from HomepageSliders where Deleted=0 and IsActive=1 order by DisplayOrder");
            return list;
        }

        public async Task<HomepageSliderDto> GetById(int id)
        {
            var homepageSlider = await unitOfWork.Repository<HomepageSlider>()
                .FirstOrDefault(c => !c.Deleted && c.Id == id);

            if (homepageSlider is null)
                throw new NotFoundException("HomepageSlider.NotFound");

            return new HomepageSliderDto
            {
                Id = id,
                ImageUrl = homepageSlider.ImageUrl,
                Url = homepageSlider.Url,
                Description = homepageSlider.Description,
                Title = homepageSlider.Title,
                DisplayOrder = homepageSlider.DisplayOrder,
                IsActive = homepageSlider.IsActive                
            };
        }

        public async Task<ServiceResult> Create(HomepageSliderDto dto)
        {
            var extension = Path.GetExtension(dto.Image.FileName);
            string imageUrl = $"/images/homepage-sliders/{Guid.NewGuid()}{extension}";
            var fileUploadUrl = $"{environment.WebRootPath}{imageUrl}";
            dto.Image.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));

            await unitOfWork.Repository<HomepageSlider>().Add(new()
            {
                Deleted = false,
                IsActive = dto.IsActive,
                Description = dto.Description,
                Title = dto.Title,
                Url = dto.Url,
                ImageUrl = imageUrl,
                DisplayOrder = dto.DisplayOrder                
            });

            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Update(HomepageSliderDto dto)
        {
            var homepageSlider = await unitOfWork.Repository<HomepageSlider>()
                .FirstOrDefault(x => !x.Deleted && x.Id == dto.Id);

            if (homepageSlider is null)
                throw new NotFoundException("HomepageSlider.NotFound");

            if (dto.Image != null)
            {
                var currentFileUrl = Path.Combine(environment.WebRootPath, dto.ImageUrl);

                if (File.Exists(currentFileUrl))
                {
                    File.Delete(currentFileUrl);
                }
                var extension = Path.GetExtension(dto.Image.FileName);
                string imageUrl = $"/images/homepage-sliders/{Guid.NewGuid()}{extension}";
                var fileUploadUrl = $"{environment.WebRootPath}{imageUrl}";
                dto.Image.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));

                homepageSlider.ImageUrl = imageUrl;
            }

            homepageSlider.Title = dto.Title;
            homepageSlider.Url = dto.Url;
            homepageSlider.IsActive = dto.IsActive;
            homepageSlider.DisplayOrder = dto.DisplayOrder;
            homepageSlider.Description = dto.Description;


            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var homepageSlider = await unitOfWork.Repository<HomepageSlider>()
                .FirstOrDefault(c => c.Id == id);

            if (homepageSlider is null)
                throw new NotFoundException("HomepageSlider.NotFound");

            homepageSlider.Deleted = true;

            unitOfWork.Repository<HomepageSlider>().Update(homepageSlider);
            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }
    }
}
