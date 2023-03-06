using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Consts;
using CMS.Storage.Entity;
using CMS.Storage.Model;
using CMS.Service.Exceptions;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface ITaskCategoryService
    {
        IQueryable<TaskCategory> GetAll();
        Task<ServiceResult> Post(TaskCategory model);
        Task<ServiceResult> Put(TaskCategory model);
        Task<ServiceResult> Delete(int id);
    }

    public class TaskCategoryService : ITaskCategoryService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public TaskCategoryService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<TaskCategory> GetAll()
        {
            return _unitOfWork.Repository<TaskCategory>()
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.Id)
                .AsQueryable();
        }

        public async Task<ServiceResult> Post(TaskCategory model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var taskCategory = new TaskCategory
            {
                Deleted = false,
                IsActive = model.IsActive,
                Name = model.Name
            };

            await _unitOfWork.Repository<TaskCategory>().Add(taskCategory);

            await _unitOfWork.Save();

            result.Message = AlertMessages.Post;

            return result;
        }

        public async Task<ServiceResult> Put(TaskCategory model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var taskCategory = await _unitOfWork.Repository<TaskCategory>()
                    .FirstOrDefault(x => x.Id == model.Id);

            if (taskCategory == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            taskCategory.Name = model.Name;
            taskCategory.IsActive = model.IsActive;

            await _unitOfWork.Save();

            result.Message = AlertMessages.Put;

            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var taskCategory = await _unitOfWork.Repository<TaskCategory>()
                   .FirstOrDefault(x => x.Id == id);

            if (taskCategory == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            taskCategory.Deleted = true;

            await _unitOfWork.Save();

            result.Message = AlertMessages.Delete;

            return result;
        }
    }
}
