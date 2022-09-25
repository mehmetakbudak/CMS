using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Service.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Service
{
    public interface ITeamService
    {
        List<Team> GetAll();
    }

    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public TeamService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Team> GetAll()
        {
            var list = _unitOfWork.Repository<Team>()
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.Id).ToList();
            return list;
        }
    }
}
