using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Model;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Service
{
    public interface IClientService
    {
        List<ClientModel> GetAllActive();
    }

    public class ClientService : IClientService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public ClientService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<ClientModel> GetAllActive()
        {
            return _unitOfWork.Repository<Client>()
                .Where(x => !x.Deleted && x.IsActive)
                .OrderByDescending(x => x.Id).Select(x => new ClientModel
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    Title = x.Title
                }).ToList();
        }
    }
}
