using CMS.Business.Extensions;
using CMS.Business.Helper;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Dtos.UserJob;
using CMS.Storage.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;  
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IUserJobService
    {
        Task<List<UserJobDto>> GetAppliedJobs();
        Task<List<UserJobDto>> GetUserJobs(int userId);
        Task<bool> CheckUserJob(int userId, int jobId);
        Task<ServiceResult> Create(UserJobCreateDto model);
        Task<ServiceResult> Delete(int id);
    }

    public class UserJobService(
            IUnitOfWork<CMSContext> unitOfWork,
            IHttpContextAccessor httpContextAccessor) : IUserJobService
    {
        public async Task<List<UserJobDto>> GetAppliedJobs()
        {
            var loginUser = httpContextAccessor.HttpContext.User.Parse();

            var result = await unitOfWork.Repository<UserJob>()
                .Where(x => x.UserId == loginUser.UserId && !x.Job.Deleted)
                .Include(x => x.Job)
                .ThenInclude(x => x.Tenant)
                .Select(x => new UserJobDto
                {
                    Id = x.Id,
                    JobId = x.JobId,
                    InsertedDate = x.InsertedDate,
                    Position = x.Job.Position,
                    JobLocationName = x.Job.JobLocation.Name,
                    TenantName = x.Job.Tenant.Name,
                    TenantImageUrl = x.Job.Tenant.ImageUrl,
                    WorkTypeName = x.Job.WorkType.GetDescription()
                }).ToListAsync();

            return result;
        }

        public async Task<List<UserJobDto>> GetUserJobs(int userId)
        {
            var userJobs = await unitOfWork.Repository<UserJob>()
                .Where(x => x.UserId == userId && !x.Job.Deleted)
                .Include(x => x.Job.Tenant)
                .Include(x => x.Job.JobLocation)
                .Select(x => new UserJobDto
                {
                    Id = x.Id,
                    JobId = x.JobId,
                    InsertedDate = x.InsertedDate,
                    Position = x.Job.Position,
                    JobLocationName = x.Job.JobLocation.Name,
                    TenantName = x.Job.Tenant.Name,
                    TenantImageUrl = x.Job.Tenant.ImageUrl,
                    WorkTypeName = x.Job.WorkType.GetDescription()
                }).ToListAsync();
            return userJobs;
        }

        public async Task<bool> CheckUserJob(int userId, int jobId)
        {
            return await unitOfWork.Repository<UserJob>()
                .Any(x => x.UserId == userId && x.JobId == jobId);
        }

        public async Task<ServiceResult> Create(UserJobCreateDto model)
        {
            var loginUser = httpContextAccessor.HttpContext.User.Parse();

            if (!await CheckUserJob(loginUser.UserId, model.JobId))
            {
                await unitOfWork.Repository<UserJob>().Add(
                    new UserJob
                    {
                        InsertedDate = DateTime.Now,
                        JobId = model.JobId,
                        UserFileId = model.UserFileId,
                        UserId = loginUser.UserId
                    });
                await unitOfWork.Save();
            }

            return ServiceResult.Success(204);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var loginUser = httpContextAccessor.HttpContext.User.Parse();

            var userJob = await unitOfWork.Repository<UserJob>()
                .FirstOrDefault(x => x.Id == id && x.UserId == loginUser.UserId);

            if (userJob != null)
            {
                await unitOfWork.Repository<UserJob>().Delete(userJob);
                await unitOfWork.Save();
            }

            return ServiceResult.Success(200);
        }
    }
}
