using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Service.Exceptions;
using CMS.Service.Helper;
using CMS.Storage.Entity;
using CMS.Storage.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IJobService
    {
        Task<List<JobModel>> GetActiveJobs(string position, List<int> jobLocationId, List<int> workTypeId);
        Task<List<JobModel>> GetAll();
        Task<JobModel> GetById(int id);
    }

    public class JobService : IJobService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JobService(
            IUnitOfWork<CMSContext> unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<JobModel>> GetActiveJobs(string position, List<int> location, List<int> workType)
        {
            var query = _unitOfWork.Repository<Job>()
                .Where(x => x.IsActive && !x.Deleted)
                .Include(x => x.Company)
                .Include(x => x.JobLocation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(position))
            {
                query = query.Where(x => x.Position.ToLower().Contains(position.ToLower()));
            }
            if (location != null && location.Count > 0)
            {
                query = query.Where(x => location.Contains(x.JobLocationId));
            }
            if (workType != null && workType.Count > 0)
            {
                query = query.Where(x => workType.Contains((int)x.WorkType));
            }

            return await query.Select(x => new JobModel
            {
                Id = x.Id,
                Description = x.Description,
                JobLocationName = x.JobLocation.Name,
                CompanyName = x.Company.Name,
                CompanyImageUrl = x.Company.ImageUrl,
                Position = x.Position,
                WorkTypeName = EnumHelper.GetDescription(x.WorkType)
            }).ToListAsync();
        }

        public async Task<List<JobModel>> GetAll()
        {
            return await _unitOfWork.Repository<Job>()
                .Where(x => !x.Deleted)
                .Select(x => new JobModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    JobLocationName = x.JobLocation.Name,
                    CompanyName = x.Company.Name,
                    CompanyImageUrl = x.Company.ImageUrl,
                    Position = x.Position,
                    WorkTypeName = EnumHelper.GetDescription(x.WorkType),
                    IsActive = x.IsActive,
                    InsertedDate = x.InsertedDate
                }).ToListAsync();
        }

        public async Task<JobModel> GetById(int id)
        {
            var job = await _unitOfWork.Repository<Job>()
                .Where(x => x.Id == id)
                .Include(x => x.Company)
                .Include(x => x.JobLocation)
                .FirstOrDefaultAsync();

            if (job == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            var loginUser = _httpContextAccessor.HttpContext.User.Parse();

            var isApplyUser = await _unitOfWork.Repository<UserJob>().Any(x => x.UserId == loginUser.UserId && x.JobId == job.Id);

            return new JobModel
            {
                Id = job.Id,
                CompanyImageUrl = job.Company.ImageUrl,
                CompanyName = job.Company.Name,
                CompanyLinkedinUrl = job.Company.LinkedUrl,
                CompanyTwitterUrl = job.Company.TwitterUrl,
                CompanyWebSiteUrl = job.Company.WebSiteUrl,
                LastUpdatedDate = job.UpdatedDate.HasValue ? job.UpdatedDate.Value : job.InsertedDate,
                Description = job.Description,
                JobLocationName = job.JobLocation.Name,
                Position = job.Position,
                IsActive = job.IsActive,
                WorkTypeName = EnumHelper.GetDescription(job.WorkType),
                IsApplyUser = isApplyUser
            };
        }
    }
}
