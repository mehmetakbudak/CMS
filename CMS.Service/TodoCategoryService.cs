using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Consts;
using CMS.Model.Entity;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface ITodoCategoryService
    {
        IQueryable<TodoCategory> GetAll();
        Task<ServiceResult> Post(TodoCategory model);
        Task<ServiceResult> Put(TodoCategory model);
        Task<ServiceResult> Delete(int id);
    }

    public class TodoCategoryService : ITodoCategoryService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public TodoCategoryService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<TodoCategory> GetAll()
        {
            return _unitOfWork.Repository<TodoCategory>()
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.Id)
                .AsQueryable();
        }

        public async Task<ServiceResult> Post(TodoCategory model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var todoCategory = new TodoCategory
            {
                Deleted = false,
                IsActive = model.IsActive,
                Name = model.Name
            };

            await _unitOfWork.Repository<TodoCategory>().Add(todoCategory);

            await _unitOfWork.Save();

            result.Message = AlertMessages.Post;

            return result;
        }

        public async Task<ServiceResult> Put(TodoCategory model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var todoCategory = await _unitOfWork.Repository<TodoCategory>()
                    .FirstOrDefault(x => x.Id == model.Id);

            if (todoCategory == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            todoCategory.Name = model.Name;
            todoCategory.IsActive = model.IsActive;

            await _unitOfWork.Save();

            result.Message = AlertMessages.Put;

            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var todoCategory = await _unitOfWork.Repository<TodoCategory>()
                   .FirstOrDefault(x => x.Id == id);

            if (todoCategory == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            todoCategory.Deleted = true;

            await _unitOfWork.Save();

            result.Message = AlertMessages.Delete;

            return result;
        }
    }
}
