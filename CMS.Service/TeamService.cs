using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Entity;
using CMS.Service.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface ITeamService
    {
        Task<List<Team>> GetAll();
        Task<List<Team>> GetAllActive();
    }

    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public TeamService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Team>> GetAll()
        {
            var list = await _unitOfWork.Repository<Team>()
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.Id).ToListAsync();

            return list;
        }

        public async Task<List<Team>> GetAllActive()
        {
            return await _unitOfWork.Repository<Team>()
                .Where(x => !x.Deleted && x.IsActive)
                .OrderByDescending(x => x.Id).ToListAsync();
        }
    }
}
