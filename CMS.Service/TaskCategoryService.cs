using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Service.Exceptions;
using CMS.Storage.Consts;
using CMS.Storage.Entity;
using CMS.Storage.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface ITaskCategoryService
    {
        Task<List<TaskCategory>> GetAll();
        Task<TaskCategoryModel> GetById(int id);
        Task<ServiceResult> Post(TaskCategoryModel model);
        Task<ServiceResult> Put(TaskCategoryModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class TaskCategoryService : ITaskCategoryService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public TaskCategoryService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TaskCategory>> GetAll()
        {
            return await _unitOfWork.Repository<TaskCategory>()
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<TaskCategoryModel> GetById(int id)
        {
            var taskCategory = await _unitOfWork.Repository<TaskCategory>()
                   .FirstOrDefault(x => x.Id == id);

            if (taskCategory == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            return new TaskCategoryModel
            {
                Id = taskCategory.Id,
                IsActive = taskCategory.IsActive,
                Name = taskCategory.Name
            };
        }

        public async Task<ServiceResult> Post(TaskCategoryModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Post };

            var taskCategory = new TaskCategory
            {
                Deleted = false,
                IsActive = model.IsActive,
                Name = model.Name
            };

            await _unitOfWork.Repository<TaskCategory>().Add(taskCategory);
            await _unitOfWork.Save();
            return result;
        }

        public async Task<ServiceResult> Put(TaskCategoryModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Put };

            var taskCategory = await _unitOfWork.Repository<TaskCategory>()
                    .FirstOrDefault(x => x.Id == model.Id);

            if (taskCategory == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            taskCategory.Name = model.Name;
            taskCategory.IsActive = model.IsActive;
            await _unitOfWork.Save();

            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Delete };

            var taskCategory = await _unitOfWork.Repository<TaskCategory>()
                   .FirstOrDefault(x => x.Id == id);

            if (taskCategory == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            taskCategory.Deleted = true;
            await _unitOfWork.Save();

            return result;
        }
    }
}
