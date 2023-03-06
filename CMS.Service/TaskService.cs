using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Service.Exceptions;
using CMS.Storage.Consts;
using CMS.Storage.Dto;
using CMS.Storage.Entity;
using CMS.Storage.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface ITaskService
    {
        IQueryable<TaskGetModel> GetAll(TaskFilterModel model);
        IQueryable<TaskGetModel> GetUserTasks(int userId);
        Task<TaskModel> GetById(int id);
        Task<ServiceResult> Post(TaskModel model);
        Task<ServiceResult> Put(TaskModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public TaskService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<TaskGetModel> GetAll(TaskFilterModel model)
        {
            var data = _unitOfWork.Repository<TaskDmo>()
                .Where(x => !x.Deleted)
                .Include(x => x.TaskCategory)
                .Include(x => x.TaskStatus)
                .Include(x => x.AssignUser)
                .AsQueryable();

            if (model.EndDate.HasValue)
                data = data.Where(x => x.InsertedDate.Date <= model.EndDate.Value.Date);
            if (model.StartDate.HasValue)
                data = data.Where(x => x.InsertedDate.Date >= model.StartDate.Value.Date);
            if (!string.IsNullOrEmpty(model.Title))
                data = data.Where(x => x.Title.Contains(model.Title));
            if (model.TaskCategoryId.HasValue)
                data = data.Where(x => x.TaskCategoryId == model.TaskCategoryId.Value);
            if (model.TaskStatusId.HasValue)
                data = data.Where(x => x.TaskStatusId == model.TaskStatusId.Value);
            if (model.AssignUserId.HasValue)
                data = data.Where(x => x.AssignUserId == model.AssignUserId.Value);
            if (model.IsActive.HasValue)
                data = data.Where(x => x.IsActive == model.IsActive.Value);

            var list = data.Select(x => new TaskGetModel
            {
                Description = x.Description,
                Id = x.Id,
                InsertedDate = x.InsertedDate,
                IsActive = x.IsActive,
                Title = x.Title,
                TaskCategoryId = x.TaskCategoryId,
                TaskCategoryName = x.TaskCategory.Name,
                TaskStatusId = x.TaskStatusId,
                TaskStatusName = x.TaskStatus.Name,
                UpdatedDate = x.UpdatedDate,
                AssignUserId = x.AssignUserId,
                UserNameSurname = x.AssignUser.Name + " " + x.AssignUser.Surname
            }).OrderByDescending(x => x.Id).AsQueryable();

            return list;
        }

        public IQueryable<TaskGetModel> GetUserTasks(int userId)
        {
            return _unitOfWork.Repository<TaskDmo>()
                .Where(x => !x.Deleted && x.IsActive && x.AssignUserId == userId)
                .Include(x => x.TaskCategory)
                .Include(x => x.TaskStatus)
                .Include(x => x.AssignUser)
                .Select(x => new TaskGetModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    InsertedDate = x.InsertedDate,
                    IsActive = x.IsActive,
                    Title = x.Title,
                    TaskCategoryId = x.TaskCategoryId,
                    TaskCategoryName = x.TaskCategory.Name,
                    TaskStatusId = x.TaskStatusId,
                    TaskStatusName = x.TaskStatus.Name,
                    UpdatedDate = x.UpdatedDate,
                    AssignUserId = x.AssignUserId,
                    UserNameSurname = x.AssignUser.Name + " " + x.AssignUser.Surname
                })
                .OrderByDescending(x => x.Id)
                .AsQueryable();
        }

        public async Task<TaskModel> GetById(int id)
        {
            var task = await _unitOfWork.Repository<TaskDmo>()
                .Where(x => x.Id == id)
                .Select(x => new TaskModel
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

        public async Task<ServiceResult> Post(TaskModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Post };

            if (model.Id == 0)
            {
                var task = new TaskDmo
                {
                    Deleted = false,
                    Description = model.Description,
                    InsertedDate = DateTime.Now,
                    IsActive = model.IsActive,
                    TaskCategoryId = model.TaskCategoryId,
                    Title = model.Title,
                    TaskStatusId = model.TaskStatusId,
                    AssignUserId = model.AssignUserId
                };

                await _unitOfWork.Repository<TaskDmo>().Add(task);

                await _unitOfWork.Save();
            }
            return serviceResult;
        }

        public async Task<ServiceResult> Put(TaskModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Put };

            var task = await _unitOfWork.Repository<TaskDmo>()
                   .FirstOrDefault(x => x.Id == model.Id);

            if (task == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            task.Description = model.Description;
            task.IsActive = model.IsActive;
            task.Title = model.Title;
            task.TaskCategoryId = model.TaskCategoryId;
            task.TaskStatusId = model.TaskStatusId;
            task.UpdatedDate = DateTime.Now;
            task.AssignUserId = model.AssignUserId;

            await _unitOfWork.Save();

            return serviceResult;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Delete };

            var task = await _unitOfWork.Repository<TaskDmo>()
                   .FirstOrDefault(x => x.Id == id);

            if (task == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            task.Deleted = true;

            await _unitOfWork.Save();

            return serviceResult;
        }        
    }
}
