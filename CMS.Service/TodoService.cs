using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Consts;
using CMS.Storage.Dto;
using CMS.Storage.Entity;
using CMS.Storage.Model;
using CMS.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface ITodoService
    {
        Task<List<TodoGetModel>> GetAll(TodoFilterModel model);
        IQueryable<TodoGetModel> GetUserTodos(int userId);
        Task<ServiceResult> Post(TodoModel model);
        Task<ServiceResult> Put(TodoModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class TodoService : ITodoService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public TodoService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TodoGetModel>> GetAll(TodoFilterModel model)
        {
            var data = _unitOfWork.Repository<TaskDmo>()
                .Where(x => !x.Deleted)
                .Include(x => x.TodoCategory)
                .Include(x => x.TodoStatus)
                .Include(x => x.AssignUser)
                .AsQueryable();

            if (model.EndDate.HasValue)
                data = data.Where(x => x.InsertedDate.Date <= model.EndDate.Value.Date);
            if (model.StartDate.HasValue)
                data = data.Where(x => x.InsertedDate.Date >= model.StartDate.Value.Date);
            if (!string.IsNullOrEmpty(model.Title))
                data = data.Where(x => x.Title.Contains(model.Title));
            if (model.TodoCategoryId.HasValue)
                data = data.Where(x => x.TodoCategoryId == model.TodoCategoryId.Value);
            if (model.TodoStatusId.HasValue)
                data = data.Where(x => x.TodoStatusId == model.TodoStatusId.Value);
            if (model.UserId.HasValue)
                data = data.Where(x => x.AssignUserId == model.UserId.Value);
            if (model.IsActive.HasValue)
                data = data.Where(x => x.IsActive == model.IsActive.Value);

            var list = await data.Select(x => new TodoGetModel
            {
                Description = x.Description,
                Id = x.Id,
                InsertedDate = x.InsertedDate,
                IsActive = x.IsActive,
                Title = x.Title,
                TodoCategoryId = x.TodoCategoryId,
                TodoCategoryName = x.TodoCategory.Name,
                TodoStatusId = x.TodoStatusId,
                TodoStatusName = x.TodoStatus.Name,
                UpdatedDate = x.UpdatedDate,
                UserId = x.AssignUserId,
                UserNameSurname = x.AssignUser.Name + " " + x.AssignUser.Surname
            }).OrderByDescending(x => x.Id).ToListAsync();

            return list;
        }

        public IQueryable<TodoGetModel> GetUserTodos(int userId)
        {
            return _unitOfWork.Repository<TaskDmo>()
                .Where(x => !x.Deleted && x.IsActive && x.AssignUserId == userId)
                .Include(x => x.TodoCategory)
                .Include(x => x.TodoStatus)
                .Include(x => x.AssignUser)
                .Select(x => new TodoGetModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    InsertedDate = x.InsertedDate,
                    IsActive = x.IsActive,
                    Title = x.Title,
                    TodoCategoryId = x.TodoCategoryId,
                    TodoCategoryName = x.TodoCategory.Name,
                    TodoStatusId = x.TodoStatusId,
                    TodoStatusName = x.TodoStatus.Name,
                    UpdatedDate = x.UpdatedDate,
                    UserId = x.AssignUserId,
                    UserNameSurname = x.AssignUser.Name + " " + x.AssignUser.Surname
                })
                .OrderByDescending(x => x.Id)
                .AsQueryable();
        }

        public async Task<ServiceResult> Post(TodoModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Post };

            if (model.Id == 0)
            {
                var todo = new TaskDmo
                {
                    Deleted = false,
                    Description = model.Description,
                    InsertedDate = DateTime.Now,
                    IsActive = model.IsActive,
                    TodoCategoryId = model.TodoCategoryId,
                    Title = model.Title,
                    TodoStatusId = model.TodoStatusId,
                    AssignUserId = model.UserId
                };

                await _unitOfWork.Repository<TaskDmo>().Add(todo);

                await _unitOfWork.Save();
            }
            return serviceResult;
        }

        public async Task<ServiceResult> Put(TodoModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Put };

            var todo = await _unitOfWork.Repository<TaskDmo>()
                   .FirstOrDefault(x => x.Id == model.Id);

            if (todo == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            todo.Description = model.Description;
            todo.IsActive = model.IsActive;
            todo.Title = model.Title;
            todo.TodoCategoryId = model.TodoCategoryId;
            todo.TodoStatusId = model.TodoStatusId;
            todo.UpdatedDate = DateTime.Now;
            todo.AssignUserId = model.UserId;

            await _unitOfWork.Save();

            return serviceResult;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Delete };

            var todo = await _unitOfWork.Repository<TaskDmo>()
                   .FirstOrDefault(x => x.Id == id);

            if (todo == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            todo.Deleted = true;

            await _unitOfWork.Save();

            return serviceResult;
        }
    }
}
