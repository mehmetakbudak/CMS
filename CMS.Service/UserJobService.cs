using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Service.Helper;
using CMS.Storage.Consts;
using CMS.Storage.Entity;
using CMS.Storage.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IUserJobService
    {
        Task<bool> CheckUserJob(int userId, int jobId);
        Task<ServiceResult> Post(UserJobPostModel model);
        Task<ServiceResult> Delete(int id);
        Task<List<UserJobModel>> GetAppliedJobs();
    }

    public class UserJobService : IUserJobService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserJobService(
            IUnitOfWork<CMSContext> unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<UserJobModel>> GetAppliedJobs()
        {
            var loginUser = _httpContextAccessor.HttpContext.User.Parse();

            var result = await _unitOfWork.Repository<UserJob>()
                .Where(x => x.UserId == loginUser.UserId && !x.Job.Deleted)
                .Include(x => x.Job)
                .Select(x => new UserJobModel
                {
                    Id = x.Id,
                    JobId = x.JobId,
                    InsertedDate = x.InsertedDate,
                    Position = x.Job.Position,
                    JobLocationName = x.Job.JobLocation.Name,
                    CompanyName = x.Job.Company.Name,
                    CompanyImageUrl = x.Job.Company.ImageUrl,
                    WorkTypeName = EnumHelper.GetDescription(x.Job.WorkType)
                }).ToListAsync();

            return result;
        }

        public async Task<bool> CheckUserJob(int userId, int jobId)
        {
            return await _unitOfWork.Repository<UserJob>()
                .Any(x => x.UserId == userId && x.JobId == jobId);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Delete };

            var loginUser = _httpContextAccessor.HttpContext.User.Parse();

            var userJob = await _unitOfWork.Repository<UserJob>()
                .FirstOrDefault(x => x.Id == id && x.UserId == loginUser.UserId);

            if (userJob != null)
            {
                await _unitOfWork.Repository<UserJob>().Delete(userJob);
                await _unitOfWork.Save();
            }

            return result;
        }

        public async Task<ServiceResult> Post(UserJobPostModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.SuccessApplyJob };

            var loginUser = _httpContextAccessor.HttpContext.User.Parse();

            if (!await CheckUserJob(loginUser.UserId, model.JobId))
            {
                await _unitOfWork.Repository<UserJob>().Add(
                    new UserJob
                    {
                        InsertedDate = DateTime.Now,
                        JobId = model.JobId,
                        UserFileId = model.UserFileId,
                        UserId = loginUser.UserId
                    });
                await _unitOfWork.Save();
            }

            return result;
        }
    }
}
