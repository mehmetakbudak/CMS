using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Consts;
using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Model;
using CMS.Model.Model.ViewModel;
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
        BlogDetailModel GetById(int id);
        List<BlogModel> GetBlogs(string text = null, int? top = null);
        List<BlogModel> GetBlogsByCategoryUrl(string blogCategoryUrl);
        ServiceResult Put(BlogPutModel model);
        BlogDetailViewModel GetDetailById(int id);
        ServiceResult Seen(int id);
        List<MostReadBlogViewModel> MostRead(string blogCategoryUrl = null);
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

        public List<BlogModel> GetBlogs(string text = null, int? top = null)
        {
            var blogs = _unitOfWork.Repository<Blog>()
                .Where(x => x.IsActive && !x.Deleted)
                .Include(x => x.User).AsQueryable();

            if (!string.IsNullOrEmpty(text))
            {
                blogs = blogs.Where(x => x.Content.Contains(text) || x.Title.Contains(text) || x.Description.Contains(text));
            }

            if (top != null)
            {
                blogs = blogs.OrderByDescending(x => x.DisplayOrder).Take(top.Value);
            }

            var comments = _unitOfWork.Repository<Comment>()
                .Where(x => !x.Deleted && x.Status == CommentStatus.Approved && x.SourceType == SourceType.Blog).AsQueryable();

            var list = blogs.OrderBy(x => x.DisplayOrder)
              .Select(x => new BlogModel()
              {
                  Id = x.Id,
                  Url = x.Url,
                  Title = x.Title,
                  Content = x.Content,
                  ImageUrl = x.ImageUrl,
                  Description = x.Description,
                  InsertedDate = x.InsertedDate,
                  UserName = $"{x.User.Name} {x.User.Surname}",
                  CommentCount = comments.Count(x => x.SourceId == x.Id)
              }).ToList();

            return list;
        }

        public List<BlogModel> GetBlogsByCategoryUrl(string blogCategoryUrl)
        {
            var blogCategory = _unitOfWork.Repository<BlogCategory>().Where(x => x.Url == blogCategoryUrl && !x.Deleted).FirstOrDefault();

            if (blogCategory == null)
            {
                throw new NotFoundException("Blog kategorisi bulunamadı.");
            }
            var comments = _unitOfWork.Repository<Comment>()
                .Where(x => !x.Deleted && x.Status == CommentStatus.Approved && x.SourceType == SourceType.Blog).AsQueryable();

            var blogs = _unitOfWork.Repository<SelectedBlogCategory>()
                 .Where(x => !x.Blog.Deleted && x.Blog.Published && x.Blog.IsActive && x.BlogCategoryId == blogCategory.Id)
                 .Include(x => x.Blog)
                 .Include(x => x.BlogCategory)
                 .Select(x => x.Blog)
                 .OrderBy(x => x.DisplayOrder)
                 .Select(x => new BlogModel()
                 {
                     Id = x.Id,
                     Url = x.Url,
                     Title = x.Title,
                     Content = x.Content,
                     ImageUrl = x.ImageUrl,
                     Description = x.Description,
                     InsertedDate = x.InsertedDate,
                     UserName = $"{x.User.Name} {x.User.Surname}",
                     CommentCount = comments.Count(x => x.SourceId == x.Id)
                 }).ToList();

            return blogs;
        }

        public BlogDetailModel GetById(int id)
        {
            BlogDetailModel model = null;
            var blog = GetAll()
                .Include(x => x.SelectedBlogCategories)
                .ThenInclude(x => x.BlogCategory)
                .FirstOrDefault(x => x.Id == id);
            if (blog != null)
            {
                model = new BlogDetailModel()
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

        public BlogDetailViewModel GetDetailById(int id)
        {
            BlogDetailViewModel model = null;

            var blog = _unitOfWork.Repository<Blog>()
                .Where(x => x.Id == id && !x.Deleted && x.Published && x.IsActive)
                .Include(x => x.SelectedBlogCategories)
                .ThenInclude(x => x.BlogCategory)
                .Include(x => x.User)
                .FirstOrDefault();

            if (blog == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            var commentCount = _unitOfWork.Repository<Comment>()
                .Where(x => x.SourceType == SourceType.Blog && x.SourceId == id && !x.Deleted && x.Status == CommentStatus.Approved).Count();

            model = new BlogDetailViewModel
            {
                Content = blog.Content,
                NumberOfView = blog.NumberOfView,
                Id = blog.Id,
                InsertedDate = blog.InsertedDate,
                Title = blog.Title,
                ImageUrl = blog.ImageUrl,
                UserName = $"{blog.User.Name} {blog.User.Surname}",
                CommentCount = commentCount,
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

        public List<MostReadBlogViewModel> MostRead(string blogCategoryUrl = null)
        {
            IQueryable<Blog> data = null;

            if (!string.IsNullOrEmpty(blogCategoryUrl))
            {
                data = _unitOfWork.Repository<SelectedBlogCategory>()
                    .Where(x => !x.Blog.Deleted && x.Blog.IsActive && x.Blog.Published && x.BlogCategory.Url == blogCategoryUrl)
                    .Include(x => x.Blog)
                    .Include(x => x.BlogCategory)
                    .Select(x => x.Blog).AsQueryable();
            }
            else
            {
                data = _unitOfWork.Repository<Blog>()
                    .Where(x => !x.Deleted && x.IsActive && x.Published);
            }

            var list = data.OrderByDescending(x => x.NumberOfView).Take(5)
                .Select(x => new MostReadBlogViewModel()
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    Title = x.Title,
                    Url = x.Url,
                    InsertedDate = x.InsertedDate
                }).ToList();

            return list;
        }

        public ServiceResult Put(BlogPutModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Put };

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
            ServiceResult result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var blog = _unitOfWork.Repository<Blog>()
                .FirstOrDefault(x => !x.Deleted && x.Published && x.IsActive && x.Id == id);

            if (blog != null)
            {
                blog.NumberOfView++;
                _unitOfWork.Save();
            }
            return result;
        }
    }
}
