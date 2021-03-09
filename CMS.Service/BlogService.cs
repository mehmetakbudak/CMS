using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;

namespace CMS.Service
{
    public interface IBlogService
    {
        ServiceResult GetByUrl(string url);
    }

    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork<CMSContext> unitOfWork;

        public BlogService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public ServiceResult GetByUrl(string url)
        {
            try
            {
                var result = new ServiceResult { StatusCode = HttpStatusCode.OK };
               
                if(string.IsNullOrEmpty(url))
                {
                    result.StatusCode = HttpStatusCode.BadRequest;
                    result.Exceptions.Add("Url bulunamadı.");
                }
                var blog = unitOfWork.Repository<Blog>()
                .Find(x => x.Url == url &&
                    !x.Deleted &&
                    x.Published &&
                    x.IsActive,
                    x => x.Include(y => y.BlogCategory));

                if (blog == null)
                {
                    result.Exceptions.Add("Kayıt bulunamadı.");
                    result.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    result.Data = new BlogDetailModel
                    {
                        Content = blog.Content,
                        Count = blog.Count,
                        Id = blog.Id,
                        InsertedDate = blog.InsertedDate,
                        Title = blog.Title,
                        CategoryName = blog.BlogCategory.Name,
                        CategoryUrl = blog.BlogCategory.Url
                    };
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
