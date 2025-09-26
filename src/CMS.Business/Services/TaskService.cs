using CMS.Business.Exceptions;
using CMS.Business.Extensions;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Dtos.Task;
using CMS.Storage.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface ITaskService
    {
        IQueryable<TaskListDto> Get();
        IQueryable<TaskListDto> GetUserTasks(int userId);
        Task<TaskDto> GetById(int id);
        Task<ServiceResult> Create(TaskDto dto);
        Task<ServiceResult> Update(TaskDto dto);
        Task<ServiceResult> UpdateUserTask(UpdateUserTaskDto dto);
        Task<ServiceResult> Delete(int id);
    }

    public class TaskService(IUnitOfWork<CMSContext> unitOfWork,
        IHttpContextAccessor httpContextAccessor) : ITaskService
    {
        public IQueryable<TaskListDto> Get()
        {
            var result = unitOfWork.Repository<TaskDmo>()
                .Where(x => !x.Deleted)
                .Include(x => x.TaskCategory)
                .Include(x => x.TaskStatus)
                .Include(x => x.AssignUser)
                .Select(x => new TaskListDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    IsActive = x.IsActive,
                    UpdatedDate = x.UpdatedDate,
                    Description = x.Description,
                    AssignUserId = x.AssignUserId,
                    TaskStatusId = x.TaskStatusId,
                    InsertedDate = x.InsertedDate,
                    TaskCategoryId = x.TaskCategoryId,
                    TaskStatusName = x.TaskStatus.Name,
                    IsCompleted = x.TaskStatus.IsCompleted,
                    TaskCategoryName = x.TaskCategory.Name,
                    UserNameSurname = x.AssignUser.Name + " " + x.AssignUser.Surname,
                });
            return result;
        }

        public IQueryable<TaskListDto> GetUserTasks(int userId)
        {
            return unitOfWork.Repository<TaskDmo>()
                .Where(x => !x.Deleted && x.IsActive && x.AssignUserId == userId)
                .Include(x => x.TaskCategory)
                .Include(x => x.TaskStatus)
                .Select(x => new TaskListDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    IsActive = x.IsActive,
                    Description = x.Description,
                    UpdatedDate = x.UpdatedDate,
                    TaskStatusId = x.TaskStatusId,
                    InsertedDate = x.InsertedDate,
                    AssignUserId = x.AssignUserId,
                    TaskCategoryId = x.TaskCategoryId,
                    TaskStatusName = x.TaskStatus.Name,
                    TaskCategoryName = x.TaskCategory.Name,
                }).AsQueryable();
        }

        public async Task<TaskDto> GetById(int id)
        {
            var task = await unitOfWork.Repository<TaskDmo>()
                .Where(x => x.Id == id)
                .Select(x => new TaskDto
                {
                    Description = x.Description,
                    IsActive = x.IsActive,
                    Id = x.Id,
                    TaskCategoryId = x.TaskCategoryId,
                    TaskStatusId = x.TaskStatusId,
                    Title = x.Title,
                    AssignUserId = x.AssignUserId
                }).FirstOrDefaultAsync();
            return task;
        }

        public async Task<ServiceResult> Create(TaskDto dto)
        {
            if (dto.Id == 0)
            {
                var task = new TaskDmo
                {
                    Deleted = false,
                    Description = dto.Description,
                    InsertedDate = DateTime.Now,
                    IsActive = dto.IsActive,
                    TaskCategoryId = dto.TaskCategoryId,
                    Title = dto.Title,
                    TaskStatusId = dto.TaskStatusId,
                    AssignUserId = dto.AssignUserId
                };

                await unitOfWork.Repository<TaskDmo>().Add(task);
                await unitOfWork.Save();
            }
            return ServiceResult.Success(204);
        }

        public async Task<ServiceResult> Update(TaskDto dto)
        {
            var task = await unitOfWork.Repository<TaskDmo>()
                   .FirstOrDefault(x => x.Id == dto.Id);

            if (task is null)
                throw new NotFoundException("Task.Notfound");

            task.Description = dto.Description;
            task.IsActive = dto.IsActive;
            task.Title = dto.Title;
            task.TaskCategoryId = dto.TaskCategoryId;
            task.TaskStatusId = dto.TaskStatusId;
            task.UpdatedDate = DateTime.Now;
            task.AssignUserId = dto.AssignUserId;

            await unitOfWork.Save();
            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> UpdateUserTask(UpdateUserTaskDto dto)
        {
            var user = httpContextAccessor.HttpContext.User.Parse();

            var task = await unitOfWork.Repository<TaskDmo>()
                .FirstOrDefault(x => !x.Deleted && x.Id == dto.Id && x.AssignUserId == user.UserId);

            if (task is null)
                throw new NotFoundException("Task.NotFound");

            task.UpdatedDate = DateTime.Now;
            task.TaskStatusId= dto.TaskStatusId;

            unitOfWork.Repository<TaskDmo>().Update(task);
            await unitOfWork.Save(); 
            
            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var task = await unitOfWork.Repository<TaskDmo>()
                   .FirstOrDefault(x => x.Id == id);

            if (task is null)
                throw new NotFoundException("Task.Notfound");

            task.Deleted = true;
            await unitOfWork.Save();
            return ServiceResult.Success(200);
        }
    }
}
