using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace CMS.Service
{
    public interface IRoleService
    {
        IQueryable<RoleModel> GetAll(int? page = null);
        List<LookupModel> Lookup();
        RoleModel GetById(int id);
        ServiceResult CreateOrUpdate(RoleModel model);
        ServiceResult Delete(int id);
    }
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork<CMSContext> unitOfWork;

        public RoleService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<RoleModel> GetAll(int? page = null)
        {

            var list = unitOfWork.Repository<Role>()
                .GetAll(x => !x.Deleted)
                .OrderByDescending(x => x.Id)
                .Select(x => new RoleModel
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    // TotalCount=x.TotalCount,
                    Name = x.Name
                }).AsQueryable();

            if (page.HasValue)
            {
                int skip = (page.Value - 1) * 10;
                list.Skip(skip).Take(10);
            }

            return list;
        }

        public List<LookupModel> Lookup()
        {
            return unitOfWork.Repository<Role>()
                .GetAll(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
        }

        public RoleModel GetById(int id)
        {
            var role = unitOfWork.Repository<Role>()
                .GetAll(x => x.Id == id)
                .Select(x => new RoleModel
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    Name = x.Name
                }).FirstOrDefault();
            return role;
        }

        public ServiceResult CreateOrUpdate(RoleModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                if (model.Id == 0)
                {
                    var role = new Role
                    {
                        Deleted = false,
                        IsActive = model.IsActive,
                        Name = model.Name
                    };
                    unitOfWork.Repository<Role>().Add(role);
                }
                else
                {
                    var role = unitOfWork.Repository<Role>().Find(x => x.Id == model.Id);
                    if (role != null)
                    {
                        role.IsActive = model.IsActive;
                        role.Name = model.Name;
                    }
                    else
                    {
                        throw new Exception("Kayıt bulunamadı.");
                    }
                }
                unitOfWork.Save();
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
                var role = unitOfWork.Repository<Role>().Find(x => x.Id == id);
                if (role != null)
                {
                    role.Deleted = true;
                    unitOfWork.Save();
                }
                else
                    throw new Exception("Kayıt bulunamadı.");
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
