using CMS.Business.Exceptions;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Dtos.Role;
using CMS.Storage.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IRoleService
    {
        IQueryable<RoleDataDto> Get();
        Task<RoleDataDto> GetById(int id);
        Task<ServiceResult> Create(RoleDto dto);
        Task<ServiceResult> Update(RoleDto dto);
        Task<ServiceResult> Delete(int id);
    }

    public class RoleService(IUnitOfWork<CMSContext> unitOfWork) : IRoleService
    {
        public IQueryable<RoleDataDto> Get()
        {
            return unitOfWork.Repository<Role>()
                .Where(x => !x.Deleted)
                .Select(x => new RoleDataDto
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    Name = x.Name
                }).AsQueryable();
        }

        public async Task<RoleDataDto> GetById(int id)
        {
            var dto = await unitOfWork.Repository<Role>()
                .Where(x => !x.Deleted && x.Id == id)
                .Include(x => x.RoleAccessRights)
                .Select(x => new RoleDataDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive
                }).FirstOrDefaultAsync();

            return dto;
        }

        public async Task<ServiceResult> Create(RoleDto dto)
        {
            if (dto.AccessRightIds is null || !dto.AccessRightIds.Any())
                throw new NotFoundException("Erişim hakkı seçiniz.");

            var isExist = await unitOfWork.Repository<Role>()
                .Any(x => !x.Deleted && x.Name == dto.Name);

            if (isExist)
                throw new FoundException("Daha önce aynı isimden role tanımlanmış.");

            var role = new Role
            {
                Deleted = false,
                Name = dto.Name,
                IsActive = dto.IsActive
            };

            await unitOfWork.Repository<Role>().Add(role);
            await unitOfWork.Save();

            foreach (var accessRightId in dto.AccessRightIds)
            {
                await unitOfWork.Repository<RoleAccessRight>()
                    .Add(new RoleAccessRight
                    {
                        AccessRightId = accessRightId,
                        RoleId = role.Id
                    });
            }
            await unitOfWork.Save();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Update(RoleDto dto)
        {
            if (dto.AccessRightIds is null || !dto.AccessRightIds.Any())
                throw new NotFoundException("Erişim hakkı seçiniz.");

            var role = await unitOfWork.Repository<Role>()
               .Where(x => !x.Deleted && x.Id == dto.Id)
               .Include(x => x.RoleAccessRights)
               .FirstOrDefaultAsync();

            if (role is null)
                throw new NotFoundException();

            role.Name = dto.Name;
            role.IsActive = dto.IsActive;

            var addingList = dto.AccessRightIds
                .Where(x => !role.RoleAccessRights.Select(x => x.AccessRightId).Contains(x)).ToList();

            if (addingList != null && addingList != null)
            {
                foreach (var accessRightId in addingList)
                {
                    await unitOfWork.Repository<RoleAccessRight>()
                        .Add(new RoleAccessRight
                        {
                            AccessRightId = accessRightId,
                            RoleId = role.Id
                        });
                }
            }

            var deletingList = role.RoleAccessRights
                .Where(x => !dto.AccessRightIds.Contains(x.AccessRightId)).ToList();

            if (deletingList != null && deletingList.Any())
            {
                foreach (var roleAccessRight in deletingList)
                {
                    await unitOfWork.Repository<RoleAccessRight>().Delete(roleAccessRight);
                }
            }

            await unitOfWork.Save();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var role = await unitOfWork.Repository<Role>()
              .Where(x => !x.Deleted && x.Id == id)
              .Include(x => x.UserRoles)
              .FirstOrDefaultAsync();

            if (role is null)
                throw new NotFoundException("Role.Notfound");

            role.Deleted = true;

            if (role.UserRoles.Any())
            {
                unitOfWork.Repository<UserRole>().DeleteRange(role.UserRoles);
            }
            await unitOfWork.Save();

            return ServiceResult.Success();
        }
    }
}
