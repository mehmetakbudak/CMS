using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Model;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Service
{
    public interface IHomepageSliderService
    {
        List<HomepageSliderModel> GetAllActive();
    }

    public class HomepageSliderService : IHomepageSliderService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public HomepageSliderService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<HomepageSliderModel> GetAllActive()
        {
            return _unitOfWork.Repository<HomepageSlider>()
                .Where(x => !x.Deleted && x.IsActive)
                .OrderBy(x => x.DisplayOrder).Select(x => new HomepageSliderModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    Title = x.Title,
                    Url = x.Url
                }).ToList();
        }
    }
}
