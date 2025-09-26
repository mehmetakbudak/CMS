using CMS.Business.Exceptions;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.AccessRight;
using CMS.Storage.Dtos.AcessRightCategory;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IAccessRightCategoryService
    {
        IQueryable<AccessRightCategoryDto> Get();
        Task<List<AccessRightCategoryWithRightDto>> GetWithAccessRights();
        Task<AccessRightCategoryDto> GetById(int id);
        Task<ServiceResult> Create(AccessRightCategoryDto dto);
        Task<ServiceResult> Update(AccessRightCategoryDto dto);
        Task<ServiceResult> Delete(int id);
    }

    public class AccessRightCategoryService(IUnitOfWork<CMSContext> unitOfWork) : IAccessRightCategoryService
    {
        public IQueryable<AccessRightCategoryDto> Get()
        {
            return unitOfWork.Repository<AccessRightCategory>()
                .Where(x => !x.Deleted)
                .Select(x => new AccessRightCategoryDto
                {
                    DisplayOrder = x.DisplayOrder,
                    Id = x.Id,
                    IsActive = x.IsActive,
                    Name = x.Name
                }).AsQueryable();
        }

        public async Task<List<AccessRightCategoryWithRightDto>> GetWithAccessRights()
        {
            var result = await unitOfWork.Repository<AccessRightCategory>()
                 .Where(x => !x.Deleted)
                 .Include(x => x.AccessRights)
                 .OrderBy(x => x.DisplayOrder)
                 .Select(x => new AccessRightCategoryWithRightDto
                 {
                     Id = x.Id,
                     Name = x.Name,
                     IsActive = x.IsActive,
                     DisplayOrder = x.DisplayOrder,
                     Items = x.AccessRights
                                     .Where(x => !x.Deleted)
                                     .OrderBy(x => x.DisplayOrder)
                                     .Select(a => new AccessRightDto
                                     {
                                         Id = a.Id,
                                         Name = a.Name,
                                         IsActive = a.IsActive,
                                         AccessRightCategoryId = x.Id,
                                         DisplayOrder = a.DisplayOrder
                                     }).ToList()
                 }).ToListAsync();
            return result;
        }

        public async Task<AccessRightCategoryDto> GetById(int id)
        {
            var accessRightCategory = await unitOfWork.Repository<AccessRightCategory>()
                .FirstOrDefault(x => x.Id == id && !x.Deleted);

            if (accessRightCategory is null)
            {
                throw new NotFoundException("");
            }

            var result = new AccessRightCategoryDto
            {
                Id = accessRightCategory.Id,
                Name = accessRightCategory.Name,
                IsActive = accessRightCategory.IsActive,
                DisplayOrder = accessRightCategory.DisplayOrder
            };
            return result;
        }

        public async Task<ServiceResult> Create(AccessRightCategoryDto dto)
        {
            var isExist = await unitOfWork.Repository<AccessRightCategory>()
               .Any(x => x.Name.ToLower() == dto.Name.ToLower() && !x.Deleted);

            if (isExist)
            {
                throw new FoundException("Already.Exist");
            }

            var accessRightCategory = new AccessRightCategory
            {
                Deleted = false,
                Name = dto.Name,
                DisplayOrder = dto.DisplayOrder,
                IsActive = dto.IsActive
            };

            await unitOfWork.Repository<AccessRightCategory>().Add(accessRightCategory);
            await unitOfWork.Save();

            return ServiceResult.Success(200, accessRightCategory);
        }

        public async Task<ServiceResult> Update(AccessRightCategoryDto dto)
        {
            var accessRightCategory = await unitOfWork.Repository<AccessRightCategory>()
                .FirstOrDefault(x => x.Id == dto.Id && !x.Deleted);

            if (accessRightCategory is null)
            {
                throw new NotFoundException("");
            }

            var isExist = await unitOfWork.Repository<AccessRightCategory>()
              .Any(x => x.Id != dto.Id && x.Name.ToLower() == dto.Name.ToLower() && !x.Deleted);

            if (isExist)
            {
                throw new FoundException("Already.Exist");
            }

            accessRightCategory.DisplayOrder = dto.DisplayOrder;
            accessRightCategory.IsActive = dto.IsActive;
            accessRightCategory.Name = dto.Name;

            await unitOfWork.Save();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var accessRightCategory = await unitOfWork.Repository<AccessRightCategory>()
              .FirstOrDefault(x => x.Id == id && !x.Deleted);

            if (accessRightCategory is null)
            {
                throw new NotFoundException("");
            }

            accessRightCategory.Deleted = true;
            await unitOfWork.Save();

            return ServiceResult.Success();
        }
    }
}
