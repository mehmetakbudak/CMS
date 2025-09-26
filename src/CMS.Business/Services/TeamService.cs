using CMS.Business.Helper;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.Filter;
using CMS.Storage.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface ITeamService
    {
        Task<List<Team>> GetAll();
        PagedResponse<List<Team>> GetAllActive(PagerDto dto);
    }

    public class TeamService(IUnitOfWork<CMSContext> unitOfWork) : ITeamService
    {
        public async Task<List<Team>> GetAll()
        {
            var list = await unitOfWork.Repository<Team>()
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.Id).ToListAsync();
            return list;
        }

        public PagedResponse<List<Team>> GetAllActive(PagerDto dto)
        {
            var data = unitOfWork.Repository<Team>().Where(x => !x.Deleted && x.IsActive);
            var response = PaginationHelper.CreatePagedReponse(data, dto);
            return response;
        }
    }
}
