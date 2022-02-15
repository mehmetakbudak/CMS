using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Consts;
using CMS.Model.Dto;
using CMS.Model.Entity;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface ITodoService
    {
        List<TodoGetModel> GetAll(TodoFilterModel model);
        IQueryable<TodoGetModel> GetUserTodos(int userId);
        ServiceResult Post(TodoModel model);
        ServiceResult Put(TodoModel model);
        ServiceResult Delete(int id);
    }

    public class TodoService : ITodoService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public TodoService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<TodoGetModel> GetAll(TodoFilterModel model)
        {
            var data = _unitOfWork.Repository<Todo>()
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
                data = data.Where(x => x.TodoStatusId== model.TodoStatusId.Value);
            if (model.UserId.HasValue)
                data = data.Where(x => x.AssignUserId == model.UserId.Value);
            if (model.IsActive.HasValue)
                data = data.Where(x => x.IsActive == model.IsActive.Value);

            var list = data.Select(x => new TodoGetModel
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
                AssignUserId = x.AssignUserId,
                UserNameSurname = x.AssignUser.Name + " " + x.AssignUser.Surname
            }).OrderByDescending(x => x.Id).ToList();
            return list;
        }

        public IQueryable<TodoGetModel> GetUserTodos(int userId)
        {
            return _unitOfWork.Repository<Todo>()
                .Where(x => !x.Deleted && x.IsActive && x.AssignUserId == userId, x => x
                .Include(x => x.TodoCategory)
                .Include(x => x.TodoStatus)
                .Include(x => x.AssignUser))
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
                    AssignUserId = x.AssignUserId,
                    UserNameSurname = x.AssignUser.Name + " " + x.AssignUser.Surname
                })
                .OrderByDescending(x => x.Id)
                .AsQueryable();
        }

        public ServiceResult Post(TodoModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK, Message = AlertMessages.Post };

            if (model.Id == 0)
            {
                var todo = new Todo
                {
                    Deleted = false,
                    Description = model.Description,
                    InsertedDate = DateTime.Now,
                    IsActive = model.IsActive,
                    TodoCategoryId = model.TodoCategoryId,
                    Title = model.Title,
                    TodoStatusId = model.TodoStatusId,
                    AssignUserId = model.AssignUserId
                };
                _unitOfWork.Repository<Todo>().Add(todo);
                _unitOfWork.Save();
            }
            return serviceResult;
        }

        public ServiceResult Put(TodoModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK, Message = AlertMessages.Put };

            var todo = _unitOfWork.Repository<Todo>()
                   .FirstOrDefault(x => x.Id == model.Id);

            if (todo != null)
            {
                todo.Description = model.Description;
                todo.IsActive = model.IsActive;
                todo.Title = model.Title;
                todo.TodoCategoryId = model.TodoCategoryId;
                todo.TodoStatusId = model.TodoStatusId;
                todo.UpdatedDate = DateTime.Now;
                todo.AssignUserId = model.AssignUserId;
                _unitOfWork.Save();
            }
            else
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            return serviceResult;
        }

        public ServiceResult Delete(int id)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK, Message = AlertMessages.Delete };

            var todo = _unitOfWork.Repository<Todo>()
                   .FirstOrDefault(x => x.Id == id);

            if (todo != null)
            {
                todo.Deleted = true;
                _unitOfWork.Save();
            }
            else
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            return serviceResult;
        }
    }
}
