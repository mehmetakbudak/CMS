using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Consts;
using CMS.Model.Entity;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface IBlogCategoryService
    {
        IQueryable<BlogCategory> GetAll();
        List<BlogCategoryWithCountModel> GetAllActive();
        BlogCategoryModel GetByUrl(string url);
        ServiceResult Post(BlogCategoryModel model);
        ServiceResult Put(BlogCategoryModel model);
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
            var list = _unitOfWork.Repository<BlogCategory>()
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.Id)
                .AsQueryable();
            return list;
        }

        public List<BlogCategoryWithCountModel> GetAllActive()
        {
            var list = _unitOfWork.Repository<BlogCategory>()
                .Where(x => !x.Deleted && x.IsActive)
                .Include(x => x.SelectedBlogCategories)
                .ThenInclude(x => x.Blog)
                .OrderByDescending(x => x.Id)
                .Select(x => new BlogCategoryWithCountModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Url = x.Url,
                    BlogCount = x.SelectedBlogCategories
                                 .Select(s => s.Blog)
                                 .Count(x => x.IsActive && x.Published && !x.Deleted)
                }).ToList();
            return list;
        }

        public BlogCategoryModel GetByUrl(string url)
        {
            return _unitOfWork.Repository<BlogCategory>()
                .Where(x => !x.Deleted && x.Url == url)
                .Select(x => new BlogCategoryModel
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    IsShowHome = x.IsShowHome,
                    Name = x.Name,
                    Url = x.Url
                }).FirstOrDefault();
        }

        public ServiceResult Post(BlogCategoryModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Post };
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

        public ServiceResult Put(BlogCategoryModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Put };
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
