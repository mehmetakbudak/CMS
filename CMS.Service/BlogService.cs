using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface IBlogService
    {
        IQueryable<Blog> GetAll();
        ServiceResult GetByUrl(string url);
    }

    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork<CMSContext> unitOfWork;

        public BlogService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Blog> GetAll()
        {
            var list = unitOfWork.Repository<Blog>().GetAll(x => !x.Deleted);
            return list;
        }

        public ServiceResult GetByUrl(string url)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            if (string.IsNullOrEmpty(url))
            {
                throw new NotFoundException("Url bulunamadı.");
            }

            var blog = unitOfWork.Repository<Blog>()
                .Find(x => x.Url == url && !x.Deleted && x.Published && x.IsActive);

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
                    Title = blog.Title
                };
            }
            return result;
        }
    }
}
