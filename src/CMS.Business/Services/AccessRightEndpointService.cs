using CMS.Business.Exceptions;
using CMS.Business.Helper;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.AccessRightEndpoint;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Entity;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IAccessRightEndpointService
    {
        IQueryable<AccessRightEndpointListDto> Get();
        Task<AccessRightEndpointDto> GetById(int id);
        Task<ServiceResult> Create(AccessRightEndpointDto model);
        Task<ServiceResult> Update(AccessRightEndpointDto model);
        Task<ServiceResult> Delete(int id);
    }

    public class AccessRightEndpointService(IUnitOfWork<CMSContext> unitOfWork) : IAccessRightEndpointService
    {
        public IQueryable<AccessRightEndpointListDto> Get()
        {
            return unitOfWork.Repository<AccessRightEndpoint>()
                .Where(x => !x.AccessRight.Deleted)
                .Select(x => new AccessRightEndpointListDto
                {
                    AccessRightId = x.AccessRightId,
                    Endpoint = x.Endpoint,
                    Id = x.Id,
                    MethodName = x.Method.GetDescription(),
                }).AsQueryable();
        }

        public async Task<AccessRightEndpointDto> GetById(int id)
        {
            var accessRightEndpoint = await unitOfWork.Repository<AccessRightEndpoint>()
               .FirstOrDefault(x => x.Id == id);

            if (accessRightEndpoint is null)
            {
                throw new NotFoundException();
            }

            var result = new AccessRightEndpointDto
            {
                AccessRightId = accessRightEndpoint.AccessRightId,
                Endpoint = accessRightEndpoint.Endpoint,
                Id = accessRightEndpoint.Id,
                Method = accessRightEndpoint.Method
            };
            return result;
        }

        public async Task<ServiceResult> Create(AccessRightEndpointDto dto)
        {
            var accessRightEndpoint = new AccessRightEndpoint
            {
                AccessRightId = dto.AccessRightId,
                Endpoint = dto.Endpoint,
                Method = dto.Method
            };

            await unitOfWork.Repository<AccessRightEndpoint>().Add(accessRightEndpoint);
            await unitOfWork.Save();

            return ServiceResult.Success(200, accessRightEndpoint);
        }

        public async Task<ServiceResult> Update(AccessRightEndpointDto dto)
        {
            var accessRightEndpoint = await unitOfWork.Repository<AccessRightEndpoint>()
                .FirstOrDefault(x => x.Id == dto.Id);

            if (accessRightEndpoint is null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            accessRightEndpoint.Method = dto.Method;
            accessRightEndpoint.Endpoint = dto.Endpoint;
            accessRightEndpoint.AccessRightId = dto.AccessRightId;

            await unitOfWork.Save();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Delete(int id)
        {
            await unitOfWork.Repository<AccessRightEndpoint>().Delete(id);
            await unitOfWork.Save();

            return ServiceResult.Success();
        }
    }
}
