using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Consts;
using CMS.Storage.Entity;
using CMS.Storage.Model;
using CMS.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface ITaskStatusService
    {
        IQueryable<TaskStatusDmo> GetAll();
        Task<List<TaskStatusDmo>> GetByTaskCategoryId(int categoryId);
        Task<ServiceResult> Post(TaskStatusDmo model);
        Task<ServiceResult> Put(TaskStatusDmo model);
        Task<ServiceResult> Delete(int id);
    }

    public class TaskStatusService : ITaskStatusService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public TaskStatusService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<TaskStatusDmo> GetAll()
        {
            return _unitOfWork.Repository<TaskStatusDmo>()
                .Where(x => !x.Deleted)
                .Include(o => o.TaskCategory)
                .OrderBy(x => x.DisplayOrder)
                .AsQueryable();
        }

        public async Task<List<TaskStatusDmo>> GetByTaskCategoryId(int taskCategoryId)
        {
            return await _unitOfWork.Repository<TaskStatusDmo>()
                .Where(x => !x.Deleted && x.TaskCategoryId == taskCategoryId)
                .Include(o => o.TaskCategory)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        public async Task<ServiceResult> Post(TaskStatusDmo model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var taskStatus = new TaskStatusDmo
            {
                Deleted = false,
                IsActive = model.IsActive,
                TaskCategoryId = model.TaskCategoryId,
                Name = model.Name,
                DisplayOrder = model.DisplayOrder
            };

            await _unitOfWork.Repository<TaskStatusDmo>().Add(taskStatus);

            await _unitOfWork.Save();

            result.Message = AlertMessages.Post;

            return result;
        }

        public async Task<ServiceResult> Put(TaskStatusDmo model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var taskStatus = await _unitOfWork.Repository<TaskStatusDmo>()
                               .FirstOrDefault(x => x.Id == model.Id);

            if (taskStatus == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            taskStatus.IsActive = model.IsActive;
            taskStatus.Name = model.Name;
            taskStatus.TaskCategoryId = model.TaskCategoryId;
            taskStatus.DisplayOrder = model.DisplayOrder;

            await _unitOfWork.Save();

            result.Message = AlertMessages.Post;

            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var taskStatus = await _unitOfWork.Repository<TaskStatusDmo>()
                   .FirstOrDefault(x => x.Id == id);

            if (taskStatus == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            taskStatus.Deleted = true;

            await _unitOfWork.Save();

            result.Message = AlertMessages.Delete;

            return result;
        }
    }
}
