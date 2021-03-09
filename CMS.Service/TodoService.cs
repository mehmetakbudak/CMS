using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Dto;
using CMS.Model.Entity;
using CMS.Model.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface ITodoService
    {
        IQueryable<TodoModel> GetAll();
        IQueryable<TodoModel> GetUserTodos(int userId);
        ServiceResult CreateOrUpdate(TodoModel model);
        ServiceResult Delete(int id);
    }

    public class TodoService : ITodoService
    {
        private readonly IUnitOfWork<CMSContext> unitOfWork;

        public TodoService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<TodoModel> GetAll()
        {
            return unitOfWork.Repository<Todo>()
                .GetAll(x => !x.Deleted, x => x
                .Include(x => x.TodoCategory)
                .Include(x => x.TodoStatus)
                .Include(x => x.User))
                .Select(x => new TodoModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    InsertDate = x.InsertDate,
                    IsActive = x.IsActive,
                    Title = x.Title,
                    TodoCategoryId = x.TodoCategoryId,
                    TodoCategoryName = x.TodoCategory.Name,
                    TodoStatusId = x.TodoStatusId,
                    TodoStatusName = x.TodoStatus.Name,
                    UpdateDate = x.UpdateDate,
                    UserComment = x.UserComment,
                    UserId = x.UserId,
                    UserNameSurname = x.User.Name + " " + x.User.Surname
                })
                .OrderByDescending(x => x.Id)
                .AsQueryable();
        }

        public IQueryable<TodoModel> GetUserTodos(int userId)
        {
            return unitOfWork.Repository<Todo>()
                .GetAll(x => !x.Deleted && x.IsActive && x.UserId == userId, x => x
                .Include(x => x.TodoCategory)
                .Include(x => x.TodoStatus)
                .Include(x => x.User))
                .Select(x => new TodoModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    InsertDate = x.InsertDate,
                    IsActive = x.IsActive,
                    Title = x.Title,
                    TodoCategoryId = x.TodoCategoryId,
                    TodoCategoryName = x.TodoCategory.Name,
                    TodoStatusId = x.TodoStatusId,
                    TodoStatusName = x.TodoStatus.Name,
                    UpdateDate = x.UpdateDate,
                    UserComment = x.UserComment,
                    UserId = x.UserId,
                    UserNameSurname = x.User.Name + " " + x.User.Surname
                })
                .OrderByDescending(x => x.Id)
                .AsQueryable();
        }

        public ServiceResult CreateOrUpdate(TodoModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                if (model.Id == 0)
                {
                    var todo = new Todo
                    {
                        Deleted = false,
                        Description = model.Description,
                        InsertDate = DateTime.Now,
                        IsActive = model.IsActive,
                        TodoCategoryId = model.TodoCategoryId,
                        Title = model.Title,
                        TodoStatusId = model.TodoStatusId,
                        UserId = model.UserId
                    };
                    unitOfWork.Repository<Todo>().Add(todo);
                    unitOfWork.Save();
                }
                else
                {
                    var todo = unitOfWork.Repository<Todo>()
                        .Find(x => x.Id == model.Id);

                    if (todo != null)
                    {
                        todo.Description = model.Description;
                        todo.IsActive = model.IsActive;
                        todo.Title = model.Title;
                        todo.TodoCategoryId = model.TodoCategoryId;
                        todo.TodoStatusId = model.TodoStatusId;
                        todo.UpdateDate = DateTime.Now;
                        todo.UserId = model.UserId;
                        unitOfWork.Save();
                    }
                    else
                    {
                        serviceResult.StatusCode = HttpStatusCode.NotFound;
                        serviceResult.Exceptions.Add("Kayıt bulunamadı.");
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Exceptions.Add(ex.Message);
            }
            return serviceResult;
        }

        public ServiceResult Delete(int id)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var todo = unitOfWork.Repository<Todo>()
                       .Find(x => x.Id == id);

                if (todo != null)
                {
                    todo.Deleted = true;
                    unitOfWork.Save();
                }
                else
                {
                    serviceResult.StatusCode = HttpStatusCode.NotFound;
                    serviceResult.Exceptions.Add("Kayıt bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Exceptions.Add(ex.Message);
            }
            return serviceResult;
        }
    }
}
