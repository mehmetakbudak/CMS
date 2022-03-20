using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Consts;
using CMS.Model.Entity;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface IBlogCategoryService
    {
        IQueryable<BlogCategory> GetAll();
        ServiceResult Post(BlogCategoryDtoModel model);
        ServiceResult Put(BlogCategoryDtoModel model);
    }

    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public BlogCategoryService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<BlogCategory> GetAll()
        {
            var entity = _unitOfWork.Repository<BlogCategory>()
                .Where(x => !x.Deleted && x.IsActive)
                .OrderByDescending(x => x.Id)
                .AsQueryable();
            return entity;
        }       

        public ServiceResult Post(BlogCategoryDtoModel model)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK, Message = AlertMessages.Post };
            var isExist = _unitOfWork.Repository<BlogCategory>().Any(x => !x.Deleted && x.Url == model.Url);
            if (isExist)
            {
                throw new FoundException("Url ile daha önce kayıt mevcuttur.");
            }
            _unitOfWork.Repository<BlogCategory>().Add(new BlogCategory()
            {
                Deleted = false,
                IsActive = model.IsActive,
                IsShowHome = model.IsShowHome,
                Name = model.Name,
                Url = model.Url
            });
            _unitOfWork.Save();
            return result;
        }

        public ServiceResult Put(BlogCategoryDtoModel model)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK, Message = AlertMessages.Put };
            var blogCategory = _unitOfWork.Repository<BlogCategory>().FirstOrDefault(x => !x.Deleted && x.Id == model.Id);
            if (blogCategory == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            var isExist = _unitOfWork.Repository<BlogCategory>().Any(x => !x.Deleted && x.Url == model.Url && x.Id != model.Id);
            if (isExist)
            {
                throw new FoundException("Url ile daha önce kayıt mevcuttur.");
            }
            blogCategory.Name = model.Name;
            blogCategory.Url = model.Url;
            blogCategory.IsActive = model.IsActive;
            blogCategory.IsShowHome = model.IsShowHome;
            _unitOfWork.Save();
            return result;
        }
    }
}
