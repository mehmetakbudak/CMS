using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Service.Exceptions;
using CMS.Service.Helper;
using CMS.Storage.Consts;
using CMS.Storage.Entity;
using CMS.Storage.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IAccessRightEndpointService
    {
        Task<List<AccessRightEndpointGetModel>> GetByAccessRightId(int accessRightId);
        Task<AccessRightEndpoint> GetById(int id);

        Task<ServiceResult> Post(AccessRightEndpoint model);

        Task<ServiceResult> Put(AccessRightEndpoint model);

        Task<ServiceResult> Delete(int id);
    }

    public class AccessRightEndpointService : IAccessRightEndpointService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public AccessRightEndpointService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Delete };

            await _unitOfWork.Repository<AccessRightEndpoint>().Delete(id);
            await _unitOfWork.Save();

            return result;
        }

        public async Task<List<AccessRightEndpointGetModel>> GetByAccessRightId(int accessRightId)
        {
            return await _unitOfWork.Repository<AccessRightEndpoint>()
                .Where(x => x.AccessRightId == accessRightId)
                .Select(x => new AccessRightEndpointGetModel
                {
                    AccessRightId = x.AccessRightId,
                    Endpoint = x.Endpoint,
                    Id = x.Id,
                    MethodName = EnumHelper.GetDescription(x.Method),
                    RouteLevel = x.RouteLevel
                }).ToListAsync();
        }

        public async Task<AccessRightEndpoint> GetById(int id)
        {
            return await _unitOfWork.Repository<AccessRightEndpoint>()
               .FirstOrDefault(x => x.Id == id);
        }

        public async Task<ServiceResult> Post(AccessRightEndpoint model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Post };

            await _unitOfWork.Repository<AccessRightEndpoint>().Add(model);
            await _unitOfWork.Save();

            return result;
        }

        public async Task<ServiceResult> Put(AccessRightEndpoint model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Put };

            var accessRightEndpoint = await _unitOfWork.Repository<AccessRightEndpoint>()
                .FirstOrDefault(x => x.Id == model.Id);

            if (accessRightEndpoint == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            accessRightEndpoint.RouteLevel = model.RouteLevel;
            accessRightEndpoint.Method = model.Method;
            accessRightEndpoint.Endpoint = model.Endpoint;

            await _unitOfWork.Save();

            return result;
        }
    }
}
