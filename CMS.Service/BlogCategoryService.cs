using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Model;
using Microsoft.EntityFrameworkCore;
using System;
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
            try
            {
                var entity = unitOfWork.Repository<BlogCategory>()
                    .GetAll(x =>
                    !x.Deleted && x.IsActive,
                    x => x.Include(y => y.Blogs));
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ServiceResult GetBlogByCategoryUrl(string url, int page)
        {
            try
            {
                var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

                if (string.IsNullOrEmpty(url))
                {
                    result.StatusCode = HttpStatusCode.BadRequest;
                    result.Exceptions.Add("Url bulunamadı.");
                }

                var categoryBlog = unitOfWork.Repository<BlogCategory>().Find(x =>
                    x.Url == url &&
                    !x.Deleted && x.IsActive,
                    x => x.Include(y => y.Blogs));

                if (categoryBlog != null)
                {
                    var skip = (page - 1) * 10;
                    var blogs = categoryBlog.Blogs
                        .Where(x => x.IsActive && !x.Deleted && x.Published)
                        .OrderBy(x => x.Sequence)
                        .ToList();

                    result.Data = new BlogCategoryModel
                    {
                        Id = categoryBlog.Id,
                        Name = categoryBlog.Name,
                        Url = categoryBlog.Url,
                        TotalCount = blogs.Count,
                        Blogs = blogs.Skip(skip).Take(10)
                        .Select(x => new BlogModel
                        {
                            Content = x.Content,
                            Id = x.Id,
                            Title = x.Title,
                            Url = x.Url
                        }).ToList()
                    };
                }
                else
                {
                    result.StatusCode = HttpStatusCode.NotFound;
                    result.Exceptions.Add($"{url} ile blog kategorisi bulunamadı.");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
