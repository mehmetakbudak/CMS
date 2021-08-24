using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface ITodoCategoryService
    {
        IQueryable<TodoCategory> GetAll();
        List<LookupModel> Lookup();
        ServiceResult Post(TodoCategory model);
        ServiceResult Put(TodoCategory model);
        ServiceResult Delete(int id);
    }

    public class TodoCategoryService : ITodoCategoryService
    {
        private readonly IUnitOfWork<CMSContext> unitOfWork;

        public TodoCategoryService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<TodoCategory> GetAll()
        {
            return unitOfWork.Repository<TodoCategory>()
                .GetAll(x => !x.Deleted)
                .OrderByDescending(x => x.Id)
                .AsQueryable();
        }

        public List<LookupModel> Lookup()
        {
            return unitOfWork.Repository<TodoCategory>()
                .GetAll(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
        }

        public ServiceResult Post(TodoCategory model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            if (model.Id == 0)
            {
                var todoCategory = new TodoCategory
                {
                    Deleted = false,
                    IsActive = model.IsActive,
                    Name = model.Name
                };
                unitOfWork.Repository<TodoCategory>().Add(todoCategory);
                unitOfWork.Save();
            }
            return serviceResult;
        }

        public ServiceResult Put(TodoCategory model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var todoCategory = unitOfWork.Repository<TodoCategory>()
                    .Find(x => x.Id == model.Id);

            if (todoCategory != null)
            {
                todoCategory.Name = model.Name;
                todoCategory.IsActive = model.IsActive;
                unitOfWork.Save();
            }
            else
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            return serviceResult;
        }


        public ServiceResult Delete(int id)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var todoCategory = unitOfWork.Repository<TodoCategory>()
                   .Find(x => x.Id == id);

            if (todoCategory != null)
            {
                todoCategory.Deleted = true;
                unitOfWork.Save();
            }
            else
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            return serviceResult;
        }
    }
}
