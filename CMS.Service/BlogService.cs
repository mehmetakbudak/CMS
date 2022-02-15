using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Consts;
using CMS.Model.Entity;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface IBlogService
    {
        IQueryable<Blog> GetAll();
        IQueryable<BlogGetModel> GetBlogList(string text = null);
        ServiceResult GetByUrl(string url);
        ServiceResult Put(BlogPutModel model);
    }

    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public BlogService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Blog> GetAll()
        {
            var list = _unitOfWork.Repository<Blog>().Where(x => !x.Deleted);
            return list;
        }

        public IQueryable<BlogGetModel> GetBlogList(string text = null)
        {
            var data = _unitOfWork.Repository<Blog>()
                .Where(x => !x.Deleted && x.Published && x.IsActive);

            if (!string.IsNullOrEmpty(text))
            {
                data = data.Where(x => x.Content.Contains(text) || x.Title.Contains(text) || x.Description.Contains(text));
            }

            var list = data.OrderBy(x => x.DisplayOrder)
              .Select(x => new BlogGetModel()
              {
                  Content = x.Content,
                  Description = x.Description,
                  Id = x.Id,
                  ImageUrl = x.ImageUrl,
                  Title = x.Title,
                  Url = x.Url
              }).AsQueryable();
            return list;
        }

        public ServiceResult GetByUrl(string url)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            if (string.IsNullOrEmpty(url))
            {
                throw new NotFoundException("Url bulunamadı.");
            }

            var blog = _unitOfWork.Repository<Blog>()
                .FirstOrDefault(x => x.Url == url && !x.Deleted && x.Published && x.IsActive);

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

        public ServiceResult Put(BlogPutModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK, Message = AlertMessages.Put };

            var blog = _unitOfWork.Repository<Blog>().FirstOrDefault(x => x.Id == model.Id && !x.Deleted);

            if (blog == null)
            {
                throw new NotFoundException("Blog kaydı bulunamadı.");
            }
            blog.Content = model.Content;
            blog.NumberOfView = model.NumberOfView;
            blog.Published = model.Published;
            blog.IsActive = model.IsActive;
            blog.Description = model.Description;
            blog.ImageUrl = model.ImageUrl;
            blog.UpdatedDate = DateTime.Now;
            blog.Title = model.Title;
            blog.DisplayOrder = model.DisplayOrder;
            blog.Url = model.Url;

            _unitOfWork.Save();

            return result;
        }
    }
}
