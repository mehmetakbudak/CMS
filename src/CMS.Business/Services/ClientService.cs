using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Entity;
using CMS.Storage.Dtos.Client;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IClientService
    {
        Task<List<ClientDto>> GetAllActive();
    }

    public class ClientService(IUnitOfWork<CMSContext> unitOfWork) : IClientService
    {
        public async Task<List<ClientDto>> GetAllActive()
        {
            var list = await unitOfWork.Repository<Client>()
                .Where(x => !x.Deleted && x.IsActive)
                .OrderByDescending(x => x.Id).Select(x => new ClientDto
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    Title = x.Title
                }).ToListAsync();

            return list;
        }
    }
}
