using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Dto;
using CMS.Model.Entity;
using CMS.Model.Model;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Service
{
    public interface IMenuService
    {
        List<LookupModel> Lookup();
    }

    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork<CMSContext> unitOfWork;
        public MenuService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<LookupModel> Lookup()
        {
            var list = unitOfWork.Repository<Menu>()
                .GetAll(x => !x.Deleted && x.IsActive)
                .Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            return list;
        }
    }
}
