using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
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

            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            if (string.IsNullOrEmpty(url))
            {
                throw new NotFoundException("Url bulunamadı.");
            }
            var blog = unitOfWork.Repository<Blog>()
            .Find(x => x.Url == url &&
                !x.Deleted &&
                x.Published &&
                x.IsActive,
                x => x.Include(y => y.BlogCategory));

            if (blog == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            else
            {
                result.Data = new BlogDetailModel
                {
                    Content = blog.Content,
                    NumberOfView = blog.NumberOfView,
                    Id = blog.Id,
                    InsertedDate = blog.InsertedDate,
                    Title = blog.Title,
                    CategoryName = blog.BlogCategory.Name,
                    CategoryUrl = blog.BlogCategory.Url
                };
            }
            return result;
        }
    }
}
