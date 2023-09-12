using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Service.Exceptions;
using CMS.Storage.Consts;
using CMS.Storage.Entity;
using CMS.Storage.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IRoleService
    {
        Task<List<Role>> Get();
        Task<RoleModel> GetById(int id);
        Task<ServiceResult> Post(RoleModel model);
        Task<ServiceResult> Put(RoleModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public RoleService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Role>> Get()
        {
            return await _unitOfWork.Repository<Role>()
                .Where(x => !x.Deleted)
                .ToListAsync();
        }

        public async Task<RoleModel> GetById(int id)
        {
            var model = await _unitOfWork.Repository<Role>()
                .Where(x => !x.Deleted && x.Id == id)
                .Include(x => x.RoleAccessRights)
                .Select(x => new RoleModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    AccessRightIds = x.RoleAccessRights
                                      .Where(x => !x.AccessRight.Deleted)
                                      .Select(x => x.AccessRightId).ToList()
                }).FirstOrDefaultAsync();

            return model;
        }

        public async Task<ServiceResult> Post(RoleModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Post };

            if (model.AccessRightIds == null || !model.AccessRightIds.Any())
            {
                throw new NotFoundException("Erişim hakkı seçiniz.");
            }

            var isExist = await _unitOfWork.Repository<Role>()
                .Any(x => !x.Deleted && x.Name == model.Name);

            if (isExist)
            {
                throw new FoundException("Daha önce aynı isimden role tanımlanmış.");
            }

            var role = new Role
            {
                Deleted = false,
                Name = model.Name,
                IsActive = model.IsActive
            };

            await _unitOfWork.Repository<Role>().Add(role);
            await _unitOfWork.Save();

            foreach (var accessRightId in model.AccessRightIds)
            {
                await _unitOfWork.Repository<RoleAccessRight>()
                    .Add(new RoleAccessRight
                    {
                        AccessRightId = accessRightId,
                        RoleId = role.Id
                    });
            }
            await _unitOfWork.Save();

            return result;
        }

        public async Task<ServiceResult> Put(RoleModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Put };

            if (model.AccessRightIds == null || !model.AccessRightIds.Any())
            {
                throw new NotFoundException("Erişim hakkı seçiniz.");
            }

            var role = await _unitOfWork.Repository<Role>()
               .Where(x => !x.Deleted && x.Id == model.Id)
               .Include(x => x.RoleAccessRights)
               .FirstOrDefaultAsync();

            if (role == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            role.Name = model.Name;
            role.IsActive = model.IsActive;

            var addingList = model.AccessRightIds
                            .Where(x => !role.RoleAccessRights.Select(x => x.AccessRightId).Contains(x)).ToList();

            if (addingList != null && addingList != null)
            {
                foreach (var accessRightId in addingList)
                {
                    await _unitOfWork.Repository<RoleAccessRight>().Add(
                            new RoleAccessRight
                            {
                                AccessRightId = accessRightId,
                                RoleId = role.Id
                            });
                }
            }

            var deletingList = role.RoleAccessRights
                .Where(x => !model.AccessRightIds.Contains(x.AccessRightId)).ToList();

            if (deletingList != null && deletingList.Any())
            {
                foreach (var roleAccessRight in deletingList)
                {
                    await _unitOfWork.Repository<RoleAccessRight>().Delete(roleAccessRight);
                }
            }

            await _unitOfWork.Save();

            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Delete };

            var role = await _unitOfWork.Repository<Role>()
              .FirstOrDefault(x => !x.Deleted && x.Id == id);

            if (role == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            role.Deleted = true;
            await _unitOfWork.Save();

            return result;
        }
    }
}
