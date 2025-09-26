using CMS.Business.Exceptions;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Dtos.TaskStatus;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface ITaskStatusService
    {
        IQueryable<TaskStatusListDmo> Get();
        Task<TaskStatusDto> GetById(int id);
        Task<List<Storage.Entity.TaskStatus>> GetByTaskCategoryId(int categoryId);
        Task<ServiceResult> Create(TaskStatusDto model);
        Task<ServiceResult> Update(TaskStatusDto model);
        Task<ServiceResult> Delete(int id);
    }

    public class TaskStatusService(IUnitOfWork<CMSContext> unitOfWork) : ITaskStatusService
    {
        public IQueryable<TaskStatusListDmo> Get()
        {
            return unitOfWork.Repository<Storage.Entity.TaskStatus>()
                .Where(x => !x.Deleted && !x.TaskCategory.Deleted)
                .Select(x => new TaskStatusListDmo
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    IsCompleted = x.IsCompleted,
                    DisplayOrder = x.DisplayOrder,
                    TaskCategoryId = x.TaskCategoryId
                }).AsQueryable();
        }

        public async Task<TaskStatusDto> GetById(int id)
        {
            var taskStatus = await unitOfWork.Repository<Storage.Entity.TaskStatus>()
                               .FirstOrDefault(x => x.Id == id);

            if (taskStatus is null)
                throw new NotFoundException();

            var data = new TaskStatusDto
            {
                DisplayOrder = taskStatus.DisplayOrder,
                Id = taskStatus.Id,
                IsActive = taskStatus.IsActive,
                Name = taskStatus.Name,
                TaskCategoryId = taskStatus.TaskCategoryId,
                IsCompleted = taskStatus.IsCompleted
            };

            return data;
        }

        public async Task<List<Storage.Entity.TaskStatus>> GetByTaskCategoryId(int taskCategoryId)
        {
            var list = await unitOfWork.Repository<Storage.Entity.TaskStatus>()
                .Where(x => !x.Deleted && x.TaskCategoryId == taskCategoryId)
                .Include(o => o.TaskCategory)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();

            return list;
        }

        public async Task<ServiceResult> Create(TaskStatusDto model)
        {
            if (model.IsCompleted)
            {
                var completedTaskStatus = await unitOfWork.Repository<Storage.Entity.TaskStatus>()
                    .FirstOrDefault(x => x.IsCompleted && !x.Deleted);

                if (completedTaskStatus != null)
                {
                    completedTaskStatus.IsCompleted = false;
                }
            }

            var taskStatus = new Storage.Entity.TaskStatus
            {
                Deleted = false,
                IsActive = model.IsActive,
                TaskCategoryId = model.TaskCategoryId,
                Name = model.Name,
                DisplayOrder = model.DisplayOrder,
                IsCompleted = model.IsCompleted
            };

            await unitOfWork.Repository<Storage.Entity.TaskStatus>().Add(taskStatus);
            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Update(TaskStatusDto model)
        {
            var taskStatus = await unitOfWork.Repository<Storage.Entity.TaskStatus>()
                .FirstOrDefault(x => x.Id == model.Id);

            if (taskStatus is null)
                throw new NotFoundException();

            if (model.IsCompleted)
            {
                var completedTaskStatus = await unitOfWork.Repository<Storage.Entity.TaskStatus>()
                    .FirstOrDefault(x => x.IsCompleted && !x.Deleted);

                if (completedTaskStatus != null)
                {
                    completedTaskStatus.IsCompleted = false;
                }
            }

            taskStatus.IsActive = model.IsActive;
            taskStatus.Name = model.Name;
            taskStatus.TaskCategoryId = model.TaskCategoryId;
            taskStatus.DisplayOrder = model.DisplayOrder;
            taskStatus.IsCompleted = model.IsCompleted;

            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var taskStatus = await unitOfWork.Repository<Storage.Entity.TaskStatus>()
                .FirstOrDefault(x => x.Id == id);

            if (taskStatus is null)
                throw new NotFoundException();

            taskStatus.Deleted = true;
            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }
    }
}
