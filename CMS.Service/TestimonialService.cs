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
    public interface ITestimonialService
    {
        IQueryable<Testimonial> GetAll();
        Task<List<TestimonialModel>> GetAllActive(int? top = null);
    }

    public class TestimonialService : ITestimonialService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public TestimonialService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Testimonial> GetAll()
        {
            var list = _unitOfWork.Repository<Testimonial>()
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.Id).AsQueryable();

            return list;
        }

        public Task<List<TestimonialModel>> GetAllActive(int? top = null)
        {
            var data = GetAll().Where(x => x.IsActive);

            if (top != null)
            {
                data = data.Take(top.Value);
            }

            var list = data.Select(x => new TestimonialModel
            {
                Id = x.Id,
                Name = x.Name,
                CorporateName = x.CorporateName,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Surname = x.Surname,
                Title = x.Title
            }).ToListAsync();

            return list;
        }
    }
}
