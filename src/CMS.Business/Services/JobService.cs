using CMS.Business.Exceptions;
using CMS.Business.Extensions;
using CMS.Business.Helper;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.Job;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Entity;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IJobService
    {
        IQueryable<JobListDto> Get();
        Task<IEnumerable<JobListDto>> GetAllActive();
        Task<List<JobDetailDto>> GetAll();
        Task<JobDetailDto> GetDetailById(int id);
        Task<JobDto> GetById(int id);
        Task<ServiceResult> Create(JobDto dto);
        Task<ServiceResult> Update(JobDto dto);
        Task<ServiceResult> Delete(int id);
    }

    public class JobService(
            IUnitOfWork<CMSContext> unitOfWork,
            IDbConnection dbConnection,
            ILanguageHelper languageHelper,
            IHttpContextAccessor httpContextAccessor) : IJobService
    {
        public IQueryable<JobListDto> Get()
        {           
            return unitOfWork.Repository<Job>()
                .Where(x => x.IsActive && !x.Deleted)                
                .Select(x => new JobListDto
                {
                    Id = x.Id,
                    JobLocationId = x.JobLocationId,
                    JobLocationName = x.JobLocation.Name,
                    TenantName = x.Tenant.Name,
                    TenantId = x.TenantId,
                    TenantImageUrl = x.Tenant.ImageUrl,
                    Position = x.Position,
                    Url = x.Url,
                    WorkType = (int)x.WorkType,
                    WorkTypeName = x.WorkType.GetDescription(),
                    InsertedUserName = x.InsertedUser.FullName,
                    InsertedDate = x.InsertedDate,
                    UpdatedDate = x.UpdatedDate                    
                }).AsQueryable();
        }        

        public async Task<IEnumerable<JobListDto>> GetAllActive()
        {
            var list = await dbConnection.QueryAsync<JobListDto>("select J.[Id], J.[JobLocationId], L.[Name] as JobLocationName, T.[Name] as TenantName, T.[ImageUrl] as TenantImageUrl, J.[Position], J.[Url], J.[WorkType], U.[Name] + ' '  + U.[Surname] as InsertedUserName, J.[InsertedDate], J.[UpdatedDate] from Jobs as J inner join Tenants as T on J.TenantId=T.Id inner join JobLocations as L on J.JobLocationId=L.Id inner join Users as U on J.InsertedUserId=U.Id where J.IsActive=1 and T.IsActive=1 and L.IsActive=1 and J.Deleted=0 and T.Deleted=0 and L.Deleted=0");
            return list;
        }

        public async Task<List<JobDetailDto>> GetAll()
        {
            return await unitOfWork.Repository<Job>()
                .Where(x => !x.Deleted)
                .Select(x => new JobDetailDto
                {
                    Id = x.Id,
                    Description = x.Description,
                    JobLocationName = x.JobLocation.Name,
                    TenantName = x.Tenant.Name,
                    TenantImageUrl = x.Tenant.ImageUrl,
                    Position = x.Position,
                    WorkTypeName = x.WorkType.GetDescription(),
                    IsActive = x.IsActive,
                    InsertedDate = x.InsertedDate
                }).ToListAsync();
        }

        public async Task<JobDto> GetById(int id)
        {
            var job = await unitOfWork.Repository<Job>()
                .FirstOrDefault(x => !x.Deleted && x.Id == id);

            if (job is null)
                throw new NotFoundException("Job.NotFound");

            return new JobDto
            {
                Id = job.Id,
                Description = job.Description,
                IsActive = job.IsActive,
                JobLocationId = job.JobLocationId,
                Position = job.Position,
                TenantId = job.TenantId,
                WorkType = job.WorkType
            };
        }

        public async Task<JobDetailDto> GetDetailById(int id)
        {
            var job = await unitOfWork.Repository<Job>()
                .Where(x => x.Id == id)
                .Include(x => x.Tenant)
                .Include(x => x.JobLocation)
                .FirstOrDefaultAsync();

            if (job is null)
                throw new NotFoundException("Job.NotFound");

            bool isApplyUser = false;

            var loginUser = httpContextAccessor.HttpContext.User.Parse();

            if (loginUser.UserId > 0)
            {
                isApplyUser = await unitOfWork.Repository<UserJob>()
                    .Any(x => x.UserId == loginUser.UserId && x.JobId == job.Id);
            }
            return new JobDetailDto
            {
                Id = job.Id,
                TenantImageUrl = job.Tenant.ImageUrl,
                TenantName = job.Tenant.Name,
                TenantLinkedinUrl = job.Tenant.LinkedUrl,
                TenantTwitterUrl = job.Tenant.TwitterUrl,
                TenantWebSiteUrl = job.Tenant.WebSiteUrl,
                LastUpdatedDate = job.UpdatedDate.HasValue ? job.UpdatedDate.Value : job.InsertedDate,
                Description = job.Description,
                JobLocationName = job.JobLocation.Name,
                Position = job.Position,
                IsActive = job.IsActive,
                WorkTypeName = job.WorkType.GetDescription(),
                IsApplyUser = isApplyUser
            };
        }

        public async Task<ServiceResult> Create(JobDto dto)
        {
            var user = httpContextAccessor.HttpContext.User.Parse();
            var url = UrlHelper.FriendlyUrl(dto.Position);
            var isExistUrl = await unitOfWork.Repository<Job>()
                .Any(x => !x.Deleted && x.Url == url);

            if (isExistUrl)
                throw new FoundException(languageHelper.Translate("Job.AlreadyExist", "Job is already exist"));

            var job = new Job
            {
                Deleted = false,
                Description = dto.Description,
                InsertedDate = DateTime.Now,
                InsertedUserId = user.UserId,
                IsActive = dto.IsActive,
                JobLocationId = dto.JobLocationId,
                Position = dto.Position,
                TenantId = dto.TenantId,
                WorkType = dto.WorkType,
                Url = url
            };
            await unitOfWork.Repository<Job>().Add(job);
            await unitOfWork.Save();
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Update(JobDto dto)
        {
            var user = httpContextAccessor.HttpContext.User.Parse();
            var url = UrlHelper.FriendlyUrl(dto.Position);
            var isExistUrl = await unitOfWork.Repository<Job>()
                .Any(x => x.Id != dto.Id && !x.Deleted && x.Url == url);

            if (isExistUrl)
                throw new FoundException("Job.AlreadyExist");

            var job = await unitOfWork.Repository<Job>()
                .FirstOrDefault(x => !x.Deleted && x.Id == dto.Id);

            if (job is null)
                throw new NotFoundException("Job.NotFound");

            job.UpdatedDate = DateTime.Now;
            job.IsActive = dto.IsActive;
            job.JobLocationId = dto.JobLocationId;
            job.Position = dto.Position;
            job.TenantId = dto.TenantId;
            job.WorkType = dto.WorkType;
            job.Url = url;
            job.Description = dto.Description;

            unitOfWork.Repository<Job>().Update(job);
            await unitOfWork.Save();
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var job = await unitOfWork.Repository<Job>()
               .FirstOrDefault(x => !x.Deleted && x.Id == id);

            if (job is null)
                throw new NotFoundException("Job.NotFound");

            job.UpdatedDate = DateTime.Now;
            job.Deleted = true;

            unitOfWork.Repository<Job>().Update(job);
            await unitOfWork.Save();
            return ServiceResult.Success();
        }
    }
}