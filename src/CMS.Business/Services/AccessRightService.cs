using CMS.Business.Exceptions;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.AccessRight;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IAccessRightService
    {
        IQueryable<AccessRightListDto> Get();
        Task<List<SelectAccessRightWithCategoryDto>> GetAccessRightsWithCategory(int? roleId);
        Task<AccessRightDto> GetById(int id);
        Task<ServiceResult> Create(AccessRightDto dto);
        Task<ServiceResult> Update(AccessRightDto dto);
        Task<ServiceResult> Delete(int id);
    }

    public class AccessRightService(
            IUnitOfWork<CMSContext> unitOfWork,
            IMemoryCache memoryCache) : IAccessRightService
    {
        public IQueryable<AccessRightListDto> Get()
        {
            var result = unitOfWork.Repository<AccessRight>()
                .Where(x => !x.Deleted)
                .Select(x => new AccessRightListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    DisplayOrder = x.DisplayOrder,
                    AccessRightCategoryId = x.AccessRightCategoryId,
                }).AsQueryable();

            return result;
        }

        public async Task<List<SelectAccessRightWithCategoryDto>> GetAccessRightsWithCategory(int? roleId)
        {
            var accessRightIds = new List<int>();
            if (roleId.HasValue)
            {
                accessRightIds = await unitOfWork.Repository<RoleAccessRight>()
                .Where(x => x.RoleId == roleId)
                .Select(x => x.AccessRightId)
                .ToListAsync();
            }
            var list = await unitOfWork.Repository<AccessRightCategory>().Where(x => !x.Deleted)
                .Include(x => x.AccessRights)
                .Select(x => new SelectAccessRightWithCategoryDto
                {
                    Value = x.Id,
                    Label = x.Name,
                    Items = x.AccessRights.Where(x => !x.Deleted)
                    .OrderBy(x => x.DisplayOrder)
                    .Select(x =>
                    new SelectAccessRightDto
                    {
                        Value = x.Id,
                        Label = x.Name,
                        Selected = accessRightIds.Contains(x.Id)
                    }).ToList()
                }).ToListAsync();

            return list;
        }

        public async Task<AccessRightDto> GetById(int id)
        {
            var accessRight = await unitOfWork.Repository<AccessRight>()
                .Where(x => x.Id == id && !x.Deleted)
                .Include(x => x.AccessRightEndpoints)
                .FirstOrDefaultAsync();

            if (accessRight is null)
                throw new NotFoundException();

            var model = new AccessRightDto()
            {
                Id = id,
                Name = accessRight.Name,
                IsActive = accessRight.IsActive,
                DisplayOrder = accessRight.DisplayOrder,
                AccessRightCategoryId = accessRight.AccessRightCategoryId
            };
            return model;
        }

        public async Task<ServiceResult> Create(AccessRightDto dto)
        {
            var accessRight = new AccessRight()
            {
                Deleted = false,
                DisplayOrder = dto.DisplayOrder,
                AccessRightCategoryId = dto.AccessRightCategoryId,
                IsActive = dto.IsActive,
                Name = dto.Name
            };

            await unitOfWork.Repository<AccessRight>().Add(accessRight);

            await unitOfWork.Save();

            await ClearUserCache();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Update(AccessRightDto dto)
        {
            var accessRight = await unitOfWork.Repository<AccessRight>()
               .FirstOrDefault(x => x.Id == dto.Id && !x.Deleted);

            if (accessRight is null)
                throw new NotFoundException("AccessRight.Notfound");

            accessRight.AccessRightCategoryId = dto.AccessRightCategoryId;
            accessRight.IsActive = dto.IsActive;
            accessRight.Name = dto.Name;
            accessRight.DisplayOrder = dto.DisplayOrder;

            await unitOfWork.Save();

            await ClearUserCache();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var accessRight = await unitOfWork.Repository<AccessRight>()
               .Where(x => x.Id == id && !x.Deleted)
               .Include(x => x.AccessRightEndpoints)
               .FirstOrDefaultAsync();

            if (accessRight is null)
                throw new NotFoundException("");

            accessRight.Deleted = true;

            if (accessRight.AccessRightEndpoints != null && accessRight.AccessRightEndpoints.Count > 0)
            {
                var accessRightEndpoint = accessRight.AccessRightEndpoints.FirstOrDefault();
                await unitOfWork.Repository<AccessRightEndpoint>().Delete(accessRightEndpoint);
            }

            await unitOfWork.Save();

            await ClearUserCache();

            return ServiceResult.Success();
        }

        public async Task ClearUserCache()
        {
            var userIds = await unitOfWork.Repository<User>()
                .Where(x => x.IsActive && !x.Deleted && x.UserType != UserType.Member)
                .Select(x => x.Id)
                .ToListAsync();

            foreach (var userId in userIds)
            {
                string key = $"userMenu:{userId}";
                memoryCache.Remove(key);
            }
        }

    }
}