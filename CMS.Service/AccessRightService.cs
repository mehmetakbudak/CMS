using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Service.Exceptions;
using CMS.Storage.Consts;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using CMS.Storage.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IAccessRightService
    {
        Task<List<AccessRightGetModel>> Get();
        Task<AccessRightModel> GetById(int id);
        Task<ServiceResult> Post(AccessRightModel model);
        Task<ServiceResult> Put(AccessRightModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class AccessRightService : IAccessRightService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;
        private readonly IMemoryCache _memoryCache;

        public AccessRightService(
            IUnitOfWork<CMSContext> unitOfWork,
            IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }

        public async Task<List<AccessRightGetModel>> Get()
        {
            var list = await _unitOfWork.Repository<AccessRight>()
                .Where(x => !x.Deleted)
                .Select(x => new AccessRightGetModel
                {
                    Id = x.Id,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    Name = x.Name,
                    ParentId = x.ParentId
                })
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
            return list;
        }

        public async Task<AccessRightModel> GetById(int id)
        {
            var accessRight = await _unitOfWork.Repository<AccessRight>()
                .Where(x => x.Id == id && !x.Deleted)
                .Include(x => x.AccessRightEndpoints)
                .FirstOrDefaultAsync();

            if (accessRight == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            var model = new AccessRightModel()
            {
                DisplayOrder = accessRight.DisplayOrder,
                IsActive = accessRight.IsActive,
                Id = id,
                Name = accessRight.Name,
                ParentId = accessRight.ParentId.HasValue ? new List<int> { accessRight.ParentId.Value } : null
            };
            return model;
        }

        public async Task<ServiceResult> Post(AccessRightModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var accessRight = new AccessRight()
            {
                Deleted = false,
                DisplayOrder = model.DisplayOrder,
                ParentId = model.ParentId != null ? model.ParentId.First() : null,
                IsActive = model.IsActive,
                Name = model.Name
            };

            await _unitOfWork.Repository<AccessRight>().Add(accessRight);

            await _unitOfWork.Save();

            result.Message = AlertMessages.Post;

            await ClearUserCache();

            return result;
        }

        public async Task<ServiceResult> Put(AccessRightModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var accessRight = await _unitOfWork.Repository<AccessRight>()
               .FirstOrDefault(x => x.Id == model.Id && !x.Deleted);

            if (accessRight == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            accessRight.ParentId = model.ParentId != null ? model.ParentId.First() : null;
            accessRight.IsActive = model.IsActive;
            accessRight.Name = model.Name;
            accessRight.DisplayOrder = model.DisplayOrder;

            await _unitOfWork.Save();

            result.Message = AlertMessages.Put;

            await ClearUserCache();

            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var accessRight = await _unitOfWork.Repository<AccessRight>()
               .Where(x => x.Id == id && !x.Deleted)
               .Include(x => x.AccessRightEndpoints)
               .FirstOrDefaultAsync();

            if (accessRight == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            accessRight.Deleted = true;

            if (accessRight.AccessRightEndpoints != null && accessRight.AccessRightEndpoints.Count > 0)
            {
                var accessRightEndpoint = accessRight.AccessRightEndpoints.FirstOrDefault();

                await _unitOfWork.Repository<AccessRightEndpoint>().Delete(accessRightEndpoint);
            }

            await _unitOfWork.Save();

            result.Message = AlertMessages.Delete;

            await ClearUserCache();

            return result;
        }

        public async Task ClearUserCache()
        {
            var userIds = await _unitOfWork.Repository<User>()
                .Where(x => x.IsActive && !x.Deleted && x.UserType != UserType.Member)
                .Select(x => x.Id).ToListAsync();

            foreach (var userId in userIds)
            {
                string key = $"userMenu_{userId}";
                _memoryCache.Remove(key);
            }
        }
    }
}