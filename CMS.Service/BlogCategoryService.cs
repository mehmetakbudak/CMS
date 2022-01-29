using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface IBlogCategoryService
    {
        IQueryable<BlogCategory> GetAll();

        ServiceResult GetBlogByCategoryUrl(string url, int page);
    }

    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly IUnitOfWork<CMSContext> unitOfWork;

        public BlogCategoryService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<BlogCategory> GetAll()
        {
            var entity = unitOfWork.Repository<BlogCategory>()
                .GetAll(x => !x.Deleted && x.IsActive)
                .AsQueryable();
            return entity;
        }

        public ServiceResult GetBlogByCategoryUrl(string url, int page)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            if (string.IsNullOrEmpty(url))
            {
                throw new NotFoundException("Url bulunamadı.");
            }

            var categoryBlog = unitOfWork.Repository<SelectedBlogCategory>()
                .GetAll(x => x.BlogCategory.Url == url && !x.BlogCategory.Deleted && x.BlogCategory.IsActive)
                .FirstOrDefault();

            //if (categoryBlog != null)
            //{
            //    var skip = (page - 1) * 10;
            //    var blogs = categoryBlog.Blogs
            //        .Where(x => x.IsActive && !x.Deleted && x.Published)
            //        .OrderBy(x => x.DisplayOrder)
            //        .ToList();

            //    result.Data = new BlogCategoryModel
            //    {
            //        Id = categoryBlog.Id,
            //        Name = categoryBlog.Name,
            //        Url = categoryBlog.Url,
            //        TotalCount = blogs.Count,
            //        Blogs = blogs.Skip(skip).Take(10)
            //        .Select(x => new BlogModel
            //        {
            //            Content = x.Content,
            //            Id = x.Id,
            //            Title = x.Title,
            //            Url = x.Url
            //        }).ToList()
            //    };
            //}
            //else
            //{
            //    throw new NotFoundException($"{url} ile blog kategorisi bulunamadı.");
            //}
            return result;
        }
    }
}
