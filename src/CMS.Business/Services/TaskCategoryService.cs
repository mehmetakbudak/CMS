using CMS.Business.Exceptions;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Dtos.TaskCategory;
using CMS.Storage.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface ITaskCategoryService
    {
        IQueryable<TaskCategoryListDto> Get();
        Task<TaskCategoryDto> GetById(int id);
        Task<ServiceResult> Create(TaskCategoryDto model);
        Task<ServiceResult> Update(TaskCategoryDto model);
        Task<ServiceResult> Delete(int id);
    }

    public class TaskCategoryService(IUnitOfWork<CMSContext> unitOfWork) : ITaskCategoryService
    {
        public IQueryable<TaskCategoryListDto> Get()
        {
            return unitOfWork.Repository<TaskCategory>()
                .Where(x => !x.Deleted)
                .Select(x => new TaskCategoryListDto
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    Name = x.Name
                });
        }

        public async Task<TaskCategoryDto> GetById(int id)
        {
            var taskCategory = await unitOfWork.Repository<TaskCategory>()
                .FirstOrDefault(x => x.Id == id);

            if (taskCategory is null)
                throw new NotFoundException();

            return new TaskCategoryDto
            {
                Id = taskCategory.Id,
                IsActive = taskCategory.IsActive,
                Name = taskCategory.Name
            };
        }

        public async Task<ServiceResult> Create(TaskCategoryDto model)
        {
            var taskCategory = new TaskCategory
            {
                Deleted = false,
                IsActive = model.IsActive,
                Name = model.Name
            };

            await unitOfWork.Repository<TaskCategory>().Add(taskCategory);
            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Update(TaskCategoryDto model)
        {
            var taskCategory = await unitOfWork.Repository<TaskCategory>()
                    .FirstOrDefault(x => x.Id == model.Id);

            if (taskCategory is null)
                throw new NotFoundException();

            taskCategory.Name = model.Name;

            taskCategory.IsActive = model.IsActive;

            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var taskCategory = await unitOfWork.Repository<TaskCategory>()
                   .FirstOrDefault(x => x.Id == id);

            if (taskCategory is null)
                throw new NotFoundException();

            taskCategory.Deleted = true;
            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }
    }
}
