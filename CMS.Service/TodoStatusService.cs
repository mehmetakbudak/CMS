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
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface ITodoStatusService
    {
        IQueryable<TodoStatus> GetAll();
        Task<List<TodoStatus>> GetByTodoCategoryId(int categoryId);
        Task<ServiceResult> Post(TodoStatus model);
        Task<ServiceResult> Put(TodoStatus model);
        Task<ServiceResult> Delete(int id);
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

        public async Task<List<TodoStatus>> GetByTodoCategoryId(int todoCategoryId)
        {
            return await _unitOfWork.Repository<TodoStatus>()
                .Where(x => !x.Deleted && x.TodoCategoryId == todoCategoryId)
                .Include(o => o.TodoCategory)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        public async Task<ServiceResult> Post(TodoStatus model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var todoStatus = new TodoStatus
            {
                Deleted = false,
                IsActive = model.IsActive,
                TodoCategoryId = model.TodoCategoryId,
                Name = model.Name,
                DisplayOrder = model.DisplayOrder
            };

            await _unitOfWork.Repository<TodoStatus>().Add(todoStatus);

            await _unitOfWork.Save();

            result.Message = AlertMessages.Post;

            return result;
        }

        public async Task<ServiceResult> Put(TodoStatus model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var todoStatus = await _unitOfWork.Repository<TodoStatus>()
                               .FirstOrDefault(x => x.Id == model.Id);

            if (todoStatus == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            todoStatus.IsActive = model.IsActive;
            todoStatus.Name = model.Name;
            todoStatus.TodoCategoryId = model.TodoCategoryId;
            todoStatus.DisplayOrder = model.DisplayOrder;

            await _unitOfWork.Save();

            result.Message = AlertMessages.Post;

            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var todoStatus = await _unitOfWork.Repository<TodoStatus>()
                   .FirstOrDefault(x => x.Id == id);

            if (todoStatus == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            todoStatus.Deleted = true;

            await _unitOfWork.Save();

            result.Message = AlertMessages.Delete;

            return result;
        }
    }
}
