using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Entity;
using CMS.Storage.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IHomepageSliderService
    {
        Task<List<HomepageSliderModel>> GetAllActive();
    }

    public class HomepageSliderService : IHomepageSliderService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public HomepageSliderService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<HomepageSliderModel>> GetAllActive()
        {
            return await _unitOfWork.Repository<HomepageSlider>()
                .Where(x => !x.Deleted && x.IsActive)
                .OrderBy(x => x.DisplayOrder).Select(x => new HomepageSliderModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    Title = x.Title,
                    Url = x.Url
                }).ToListAsync();
        }
    }
}
