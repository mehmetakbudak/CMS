using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Consts;
using CMS.Model.Entity;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface IBlogService
    {
        IQueryable<Blog> GetAll();
        BlogModel GetById(int id);
        BlogCategoryModel GetBlogList(string categoryUrl, string text = null);
        ServiceResult Put(BlogPutModel model);
        BlogDetailModel GetDetailById(int id);
        ServiceResult Seen(int id);        
        List<BlogGetModel> MostRead(int? blogCategoryId = null);
    }

    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public BlogService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<BlogGetModel> AllMostRead()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Blog> GetAll()
        {
            var list = _unitOfWork.Repository<Blog>().Where(x => !x.Deleted);
            return list;
        }

        public BlogCategoryModel GetBlogList(string categoryUrl, string text = null)
        {
            BlogCategoryModel model = null;

            var blogCategory = _unitOfWork.Repository<BlogCategory>()
                .Where(x => x.Url == categoryUrl && !x.Deleted)
                .Include(x => x.SelectedBlogCategories)
                .ThenInclude(x => x.Blog)
                .FirstOrDefault();

            if (blogCategory == null)
            {
                throw new NotFoundException("Blog kategorisi bulunamadı.");
            }

            var data = blogCategory.SelectedBlogCategories
                .Where(x => !x.Blog.Deleted && x.Blog.Published && x.Blog.IsActive && x.BlogCategoryId == blogCategory.Id)
                .Select(x => x.Blog);

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
              }).ToList();

            model = new BlogCategoryModel()
            {
                Blogs = list,
                Id = blogCategory.Id,
                Name = blogCategory.Name,
                Url = blogCategory.Url
            };
            return model;
        }

        public BlogModel GetById(int id)
        {
            BlogModel model = null;
            var blog = GetAll()
                .Include(x => x.SelectedBlogCategories)
                .ThenInclude(x => x.BlogCategory)
                .FirstOrDefault(x => x.Id == id);
            if (blog != null)
            {
                model = new BlogModel()
                {
                    Content = blog.Content,
                    Id = blog.Id,
                    Description = blog.Description,
                    Title = blog.Title,
                    Url = blog.Url,
                    IsActive = blog.IsActive,
                    Published = blog.Published,
                    DisplayOrder = blog.DisplayOrder,
                    BlogCategories = blog.SelectedBlogCategories.Select(x => x.BlogCategory.Id).ToList()
                };
            }
            return model;
        }

        public BlogDetailModel GetDetailById(int id)
        {
            BlogDetailModel model = null;

            var blog = _unitOfWork.Repository<Blog>()
                .Where(x => x.Id == id && !x.Deleted && x.Published && x.IsActive)
                .Include(x => x.SelectedBlogCategories)
                .ThenInclude(x => x.BlogCategory)
                .FirstOrDefault();

            if (blog == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            model = new BlogDetailModel
            {
                Content = blog.Content,
                NumberOfView = blog.NumberOfView,
                Id = blog.Id,
                InsertedDate = blog.InsertedDate,
                Title = blog.Title,
                ImageUrl = blog.ImageUrl,
                BlogCategories = blog.SelectedBlogCategories
                .Select(x => x.BlogCategory)
                .Select(x => new BlogDetailCategoryModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Url = x.Url
                }).ToList()
            };
            return model;
        }

        public List<BlogGetModel> MostRead(int? blogCategoryId = null)
        {
            var data = _unitOfWork.Repository<SelectedBlogCategory>()
                .Where(x => !x.Blog.Deleted && x.Blog.IsActive && x.Blog.Published)
                .Include(x => x.Blog)
                .AsQueryable();

            if (blogCategoryId != null)
            {
                data = data.Where(x => x.BlogCategoryId == blogCategoryId);
            }

            var list = data
                .OrderByDescending(x => x.Blog.NumberOfView)
                .Take(5)
                .Select(x => x.Blog)
                .Select(x => new BlogGetModel()
                {
                    Content = x.Content,
                    Description = x.Description,
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    Title = x.Title,
                    Url = x.Url
                }).ToList();

            return list;
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
            blog.Published = model.Published;
            blog.IsActive = model.IsActive;
            blog.Description = model.Description;
            blog.UpdatedDate = DateTime.Now;
            blog.Title = model.Title;
            blog.DisplayOrder = model.DisplayOrder;
            blog.Url = model.Url;

            var selectedBlogCategories = _unitOfWork.Repository<SelectedBlogCategory>()
                .Where(x => x.BlogId == model.Id).ToList();

            _unitOfWork.Repository<SelectedBlogCategory>().DeleteRange(selectedBlogCategories);

            if (model.BlogCategories != null)
            {
                foreach (var blogCategoryId in model.BlogCategories)
                {
                    var selectedBlogCategory = new SelectedBlogCategory()
                    {
                        BlogCategoryId = blogCategoryId,
                        BlogId = blog.Id
                    };
                    _unitOfWork.Repository<SelectedBlogCategory>().Add(selectedBlogCategory);
                }
            }
            _unitOfWork.Save();
            return result;
        }

        public ServiceResult Seen(int id)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            var blog = _unitOfWork.Repository<Blog>().FirstOrDefault(x => !x.Deleted && x.Published && x.IsActive && x.Id == id);
            if (blog == null)
            {
                throw new NotFoundException("Kayıt bulunamadı");
            }
            blog.NumberOfView++;
            _unitOfWork.Save();
            return result;
        }
    }
}
