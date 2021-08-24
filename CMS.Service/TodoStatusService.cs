﻿using CMS.Data.Context;
using CMS.Data.Repository;
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
        List<LookupModel> Lookup(int todoCategoryId);
        ServiceResult Post(TodoStatus model);
        ServiceResult Put(TodoStatus model);
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
                .GetAll(x => !x.Deleted)
                .Include(o => o.TodoCategory)
                .OrderBy(x => x.DisplayOrder)
                .AsQueryable();
        }

        public List<TodoStatus> GetByTodoCategoryId(int todoCategoryId)
        {
            return unitOfWork.Repository<TodoStatus>()
                .GetAll(x => !x.Deleted && x.TodoCategoryId == todoCategoryId)
                .Include(o => o.TodoCategory)
                .OrderBy(x => x.DisplayOrder)
                .ToList();
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

        public ServiceResult Post(TodoStatus model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

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
            return serviceResult;
        }

        public ServiceResult Put(TodoStatus model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

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
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            return serviceResult;
        }

        public ServiceResult Delete(int id)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var todoStatus = unitOfWork.Repository<TodoStatus>()
                   .Find(x => x.Id == id);

            if (todoStatus != null)
            {
                todoStatus.Deleted = true;
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
