using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Service.Exceptions;
using CMS.Service.Helper;
using CMS.Storage.Consts;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using CMS.Storage.Model;
using CMS.Storage.Model.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IBlogService
    {
        Task<List<Blog>> GetAll();
        Task<BlogDetailModel> GetById(int id);
        Task<List<BlogModel>> GetBlogs(string text = null, int? top = null);
        Task<List<BlogModel>> GetBlogsByCategoryUrl(string blogCategoryUrl);
        Task<BlogDetailViewModel> GetDetailById(int id);
        Task<ServiceResult> Post(BlogPostModel model);
        Task<List<MostReadBlogViewModel>> MostRead(string blogCategoryUrl = null);
        Task<ServiceResult> Put(BlogPutModel model);
        Task<ServiceResult> Seen(int id);
        Task<ServiceResult> Delete(int id);
    }

    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _environment;

        public BlogService(
            IUnitOfWork<CMSContext> unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        public async Task<List<Blog>> GetAll()
        {
            return await _unitOfWork.Repository<Blog>()
                .Where(x => !x.Deleted).ToListAsync();
        }

        public async Task<List<BlogModel>> GetBlogs(string text = null, int? top = null)
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

            var list = await blogs.OrderBy(x => x.DisplayOrder)
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
              }).ToListAsync();

            return list;
        }

        public async Task<List<BlogModel>> GetBlogsByCategoryUrl(string blogCategoryUrl)
        {
            var blogCategory = await _unitOfWork.Repository<BlogCategory>()
                .Where(x => x.Url == blogCategoryUrl && !x.Deleted).FirstOrDefaultAsync();

            if (blogCategory == null)
            {
                throw new NotFoundException("Blog kategorisi bulunamadı.");
            }

            var comments = _unitOfWork.Repository<Comment>()
                .Where(x => !x.Deleted && x.Status == CommentStatus.Approved && x.SourceType == SourceType.Blog).AsQueryable();

            var blogs = await _unitOfWork.Repository<SelectedBlogCategory>()
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
                 }).ToListAsync();

            return blogs;
        }

        public async Task<BlogDetailModel> GetById(int id)
        {
            BlogDetailModel model = null;
            var blog = await _unitOfWork.Repository<Blog>()
                .Where(x => !x.Deleted && x.Id == id)
                .Include(x => x.SelectedBlogCategories)
                .ThenInclude(x => x.BlogCategory)
                .FirstOrDefaultAsync();

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
                    ImageUrl = blog.ImageUrl,
                    BlogCategories = blog.SelectedBlogCategories.Select(x => x.BlogCategory.Id).ToList()
                };
            }
            return model;
        }

        public async Task<BlogDetailViewModel> GetDetailById(int id)
        {
            BlogDetailViewModel model = null;

            var blog = await _unitOfWork.Repository<Blog>()
                .Where(x => x.Id == id && !x.Deleted && x.Published && x.IsActive)
                .Include(x => x.SelectedBlogCategories)
                .ThenInclude(x => x.BlogCategory)
                .Include(x => x.User)
                .FirstOrDefaultAsync();

            if (blog == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            var commentCount = await _unitOfWork.Repository<Comment>()
                .Where(x => x.SourceType == SourceType.Blog && x.SourceId == id && !x.Deleted && x.Status == CommentStatus.Approved).CountAsync();

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
                Url = blog.Url,
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

        public async Task<List<MostReadBlogViewModel>> MostRead(string blogCategoryUrl = null)
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

            var list = await data.OrderByDescending(x => x.NumberOfView).Take(5)
                .Select(x => new MostReadBlogViewModel()
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    Title = x.Title,
                    Url = x.Url,
                    InsertedDate = x.InsertedDate
                }).ToListAsync();

            return list;
        }

        public async Task<ServiceResult> Post(BlogPostModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var strategy = _unitOfWork.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                try
                {
                    if (model == null || model.Id != 0)
                    {
                        throw new NotFoundException("Model null olamaz.");
                    }
                    if (model.Image == null)
                    {
                        throw new BadRequestException("Resim ekleyiniz.");
                    }

                    var extension = Path.GetExtension(model.Image.FileName);
                    string imageUrl = $"/images/blogs/{Guid.NewGuid()}{extension}";
                    var fileUploadUrl = $"{_environment.WebRootPath}{imageUrl}";
                    model.Image.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));

                    var loginUser = _httpContextAccessor.HttpContext.User.Parse();

                    await _unitOfWork.CreateTransaction();

                    var blog = new Blog
                    {
                        Content = model.Content,
                        Deleted = false,
                        Description = model.Description,
                        DisplayOrder = model.DisplayOrder,
                        InsertedDate = DateTime.Now,
                        IsActive = model.IsActive,
                        Published = model.Published,
                        NumberOfView = 0,
                        Title = model.Title,
                        UserId = loginUser.UserId,
                        ImageUrl = imageUrl,
                        Url = UrlHelper.FriendlyUrl(model.Title)
                    };

                    await _unitOfWork.Repository<Blog>().Add(blog);
                    await _unitOfWork.Save();

                    foreach (var blogCategoryId in model.BlogCategories)
                    {
                        await _unitOfWork.Repository<SelectedBlogCategory>()
                            .Add(new SelectedBlogCategory
                            {
                                BlogCategoryId = blogCategoryId,
                                BlogId = blog.Id
                            });
                    }

                    await _unitOfWork.Save();

                    result.Message = AlertMessages.Post;
                }
                catch (Exception ex)
                {
                    await _unitOfWork.Rollback();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    await _unitOfWork.Commit();
                }
            });
            return result;
        }

        public async Task<ServiceResult> Put(BlogPutModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var strategy = _unitOfWork.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                try
                {
                    var blog = await _unitOfWork.Repository<Blog>()
                        .FirstOrDefault(x => x.Id == model.Id && !x.Deleted);

                    if (blog == null)
                    {
                        throw new NotFoundException("Kayıt bulunamadı.");
                    }

                    await _unitOfWork.CreateTransaction();

                    if (model.Image != null)
                    {
                        var currentFileUrl = Path.Combine(_environment.WebRootPath, blog.ImageUrl);

                        if (File.Exists(currentFileUrl))
                        {
                            File.Delete(currentFileUrl);
                        }
                        var extension = Path.GetExtension(model.Image.FileName);
                        string imageUrl = $"/images/blogs/{Guid.NewGuid()}{extension}";
                        var fileUploadUrl = $"{_environment.WebRootPath}{imageUrl}";
                        model.Image.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));

                        blog.ImageUrl = imageUrl;
                    }

                    blog.Content = model.Content;
                    blog.Published = model.Published;
                    blog.IsActive = model.IsActive;
                    blog.Description = model.Description;
                    blog.UpdatedDate = DateTime.Now;
                    blog.Title = model.Title;
                    blog.DisplayOrder = model.DisplayOrder;
                    blog.Url = UrlHelper.FriendlyUrl(model.Title);

                    var selectedBlogCategories = await _unitOfWork.Repository<SelectedBlogCategory>()
                        .Where(x => x.BlogId == model.Id).ToListAsync();

                    _unitOfWork.Repository<SelectedBlogCategory>().DeleteRange(selectedBlogCategories);

                    if (model.BlogCategories != null)
                    {
                        foreach (var blogCategoryId in model.BlogCategories)
                        {
                            await _unitOfWork.Repository<SelectedBlogCategory>()
                                .Add(new SelectedBlogCategory()
                                {
                                    BlogCategoryId = blogCategoryId,
                                    BlogId = blog.Id
                                });
                        }
                    }

                    await _unitOfWork.Save();

                    result.Message = AlertMessages.Put;
                }
                catch (Exception ex)
                {
                    await _unitOfWork.Rollback();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    await _unitOfWork.Commit();
                }
            });
            return result;
        }

        public async Task<ServiceResult> Seen(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var blog = await _unitOfWork.Repository<Blog>()
                .FirstOrDefault(x => !x.Deleted && x.Published && x.IsActive && x.Id == id);

            if (blog != null)
            {
                blog.NumberOfView++;

                await _unitOfWork.Save();
            }
            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Delete };

            var blog = await _unitOfWork.Repository<Blog>()
                      .FirstOrDefault(x => x.Id == id && !x.Deleted);

            if (blog == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            blog.Deleted = true;

            await _unitOfWork.Save();

            var currentFileUrl = Path.Combine(_environment.WebRootPath, blog.ImageUrl);

            if (File.Exists(currentFileUrl))
            {
                File.Delete(currentFileUrl);
            }

            return result;
        }
    }
}
