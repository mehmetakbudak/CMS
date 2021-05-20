using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Dto;
using CMS.Model.Entity;
using CMS.Model.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface ITodoStatusService
    {
        IQueryable<TodoStatus> GetAll();
        List<LookupModel> Lookup(int categoryId);
        ServiceResult CreateOrUpdate(TodoStatus model);
        ServiceResult Delete(int id);
    }

    public class TodoStatusService : ITodoStatusService
    {
        private readonly IUnitOfWork<CMSContext> unitOfWork;

        public TodoStatusService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<TodoStatus> GetAll()
        {
            return unitOfWork.Repository<TodoStatus>()
                .GetAll(x => !x.Deleted, x => x
                .Include(o => o.TodoCategory))
                .OrderByDescending(x => x.Id)
                .AsQueryable();
        }

        public List<LookupModel> Lookup(int categoryId)
        {
            return unitOfWork.Repository<TodoStatus>()
                .GetAll(x => !x.Deleted &&
                              x.IsActive &&
                              x.TodoCategoryId == categoryId)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
        }

        public ServiceResult CreateOrUpdate(TodoStatus model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            try
            {
                if (model.Id == 0)
                {
                    var todoStatus = new TodoStatus
                    {
                        Deleted = false,
                        IsActive = model.IsActive,
                        TodoCategoryId = model.TodoCategoryId,
                        Name = model.Name
                    };
                    unitOfWork.Repository<TodoStatus>().Add(todoStatus);
                    unitOfWork.Save();
                }
                else
                {
                    var todoStatus = unitOfWork.Repository<TodoStatus>()
                        .Find(x => x.Id == model.Id);

                    if (todoStatus != null)
                    {
                        todoStatus.IsActive = model.IsActive;
                        todoStatus.Name = model.Name;
                        todoStatus.TodoCategoryId = model.TodoCategoryId;
                        unitOfWork.Save();
                    }
                    else
                    {
                        serviceResult.StatusCode = (int)HttpStatusCode.NotFound;
                        serviceResult.Message = "Kayıt bulunamadı.";
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public ServiceResult Delete(int id)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            try
            {
                var todoStatus = unitOfWork.Repository<TodoStatus>()
                       .Find(x => x.Id == id);

                if (todoStatus != null)
                {
                    todoStatus.Deleted = true;
                    unitOfWork.Save();
                }
                else
                {
                    serviceResult.StatusCode = (int)HttpStatusCode.NotFound;
                    serviceResult.Message = "Kayıt bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }
    }
}
