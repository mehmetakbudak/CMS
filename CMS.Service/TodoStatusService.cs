using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Consts;
using CMS.Model.Entity;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface ITodoStatusService
    {
        IQueryable<TodoStatus> GetAll();
        List<TodoStatus> GetByTodoCategoryId(int categoryId);
        ServiceResult Post(TodoStatus model);
        ServiceResult Put(TodoStatus model);
        ServiceResult Delete(int id);
    }

    public class TodoStatusService : ITodoStatusService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public TodoStatusService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<TodoStatus> GetAll()
        {
            return _unitOfWork.Repository<TodoStatus>()
                .Where(x => !x.Deleted)
                .Include(o => o.TodoCategory)
                .OrderBy(x => x.DisplayOrder)
                .AsQueryable();
        }

        public List<TodoStatus> GetByTodoCategoryId(int todoCategoryId)
        {
            return _unitOfWork.Repository<TodoStatus>()
                .Where(x => !x.Deleted && x.TodoCategoryId == todoCategoryId)
                .Include(o => o.TodoCategory)
                .OrderBy(x => x.DisplayOrder)
                .ToList();
        }

        public ServiceResult Post(TodoStatus model)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var todoStatus = new TodoStatus
            {
                Deleted = false,
                IsActive = model.IsActive,
                TodoCategoryId = model.TodoCategoryId,
                Name = model.Name,
                DisplayOrder = model.DisplayOrder
            };
            _unitOfWork.Repository<TodoStatus>().Add(todoStatus);
            _unitOfWork.Save();
            result.Message = AlertMessages.Post;

            return result;
        }

        public ServiceResult Put(TodoStatus model)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var todoStatus = _unitOfWork.Repository<TodoStatus>()
                               .FirstOrDefault(x => x.Id == model.Id);

            if (todoStatus == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }
            todoStatus.IsActive = model.IsActive;
            todoStatus.Name = model.Name;
            todoStatus.TodoCategoryId = model.TodoCategoryId;
            todoStatus.DisplayOrder = model.DisplayOrder;
            _unitOfWork.Save();
            result.Message = AlertMessages.Post;

            return result;
        }

        public ServiceResult Delete(int id)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var todoStatus = _unitOfWork.Repository<TodoStatus>()
                   .FirstOrDefault(x => x.Id == id);

            if (todoStatus == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }
            todoStatus.Deleted = true;
            _unitOfWork.Save();
            result.Message = AlertMessages.Delete;

            return result;
        }
    }
}
