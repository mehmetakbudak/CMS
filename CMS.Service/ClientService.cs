using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IClientService
    {
        Task<List<ClientModel>> GetAllActive();
    }

    public class ClientService : IClientService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public ClientService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ClientModel>> GetAllActive()
        {
            var list = await _unitOfWork.Repository<Client>()
                .Where(x => !x.Deleted && x.IsActive)
                .OrderByDescending(x => x.Id).Select(x => new ClientModel
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    Title = x.Title
                }).ToListAsync();

            return list;
        }
    }
}
