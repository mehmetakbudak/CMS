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
        Task<List<BlogModel>> GetTagBlogsByUrl(string url);
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
                  CommentCount = comments.Count(a => a.SourceId == x.Id)
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
                     CommentCount = comments.Count(a => a.SourceId == x.Id)
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

            if (blog is null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            var blogTags = await _unitOfWork.Repository<SourceTag>()
                .Where(x => x.SourceId == blog.Id && x.SourceType == SourceType.Blog)
                .Include(x => x.Tag)
                .ToListAsync();

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
                BlogCategories = blog.SelectedBlogCategories.Select(x => x.BlogCategory.Id).ToList(),
                SelectedTags = blogTags.Select(x => x.Tag.Name).ToList()
            };
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

            var blogTags = await _unitOfWork.Repository<SourceTag>()
                .Where(x => x.SourceType == SourceType.Blog && x.SourceId == id)
                .Include(x => x.Tag)
                .Select(x => x.Tag).ToListAsync();

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
                                     }).ToList(),
                BlogTags = blogTags.Select(x => new BlogDetailTagModel
                {
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

                    #region Add File
                    var extension = Path.GetExtension(model.Image.FileName);
                    string imageUrl = $"/images/blogs/{Guid.NewGuid()}{extension}";
                    var fileUploadUrl = $"{_environment.WebRootPath}{imageUrl}";
                    model.Image.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));
                    #endregion

                    var loginUser = _httpContextAccessor.HttpContext.User.Parse();

                    await _unitOfWork.CreateTransaction();

                    #region Add Blog 
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
                    #endregion

                    #region Add SelectedBlogCategory
                    foreach (var blogCategoryId in model.BlogCategories)
                    {
                        await _unitOfWork.Repository<SelectedBlogCategory>()
                            .Add(new SelectedBlogCategory
                            {
                                BlogCategoryId = blogCategoryId,
                                BlogId = blog.Id
                            });
                    }
                    #endregion

                    await AddNewTags(model.SelectedTags);

                    #region Add SourceTag
                    var tags = await _unitOfWork.Repository<Tag>()
                    .Where(x => model.SelectedTags.Contains(x.Name)).ToListAsync();

                    foreach (var tag in tags)
                    {
                        await _unitOfWork.Repository<SourceTag>().Add(new SourceTag
                        {
                            SourceId = blog.Id,
                            TagId = tag.Id,
                            SourceType = SourceType.Blog
                        });
                    }
                    #endregion

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

        private async Task AddNewTags(List<string> tags)
        {
            var tagNames = await _unitOfWork.Repository<Tag>().Where()
                    .Select(x => x.Name).ToListAsync();

            var addingTagList = tags.Where(x => !string.IsNullOrEmpty(x) && !tagNames.Contains(x)).ToList();

            if (addingTagList != null && addingTagList.Count > 0)
            {
                foreach (var tagName in addingTagList)
                {
                    await _unitOfWork.Repository<Tag>().Add(new Tag
                    {
                        Name = tagName,
                        Url = UrlHelper.FriendlyUrl(tagName)
                    });
                }
                await _unitOfWork.Save();
            }
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

                    #region Update File
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
                    #endregion

                    #region Update Blog
                    blog.Content = model.Content;
                    blog.Published = model.Published;
                    blog.IsActive = model.IsActive;
                    blog.Description = model.Description;
                    blog.UpdatedDate = DateTime.Now;
                    blog.Title = model.Title;
                    blog.DisplayOrder = model.DisplayOrder;
                    blog.Url = UrlHelper.FriendlyUrl(model.Title);
                    #endregion

                    #region Update SelectedBlogCategory
                    var selectedBlogCategories = await _unitOfWork.Repository<SelectedBlogCategory>()
                        .Where(x => x.BlogId == model.Id).ToListAsync();

                    var addingBlogCategoryList = model.BlogCategories
                               .Where(x => !selectedBlogCategories.Select(x => x.BlogCategoryId).Contains(x)).ToList();

                    if (addingBlogCategoryList != null && addingBlogCategoryList != null)
                    {
                        foreach (var blogCategoryId in addingBlogCategoryList)
                        {
                            await _unitOfWork.Repository<SelectedBlogCategory>()
                                 .Add(new SelectedBlogCategory()
                                 {
                                     BlogCategoryId = blogCategoryId,
                                     BlogId = blog.Id
                                 });
                        }
                    }

                    var deletingBlogCategoryList = selectedBlogCategories.Where(x => !model.BlogCategories.Contains(x.BlogCategoryId)).ToList();

                    if (deletingBlogCategoryList != null && deletingBlogCategoryList.Any())
                    {
                        foreach (var selectedBlogCategory in deletingBlogCategoryList)
                        {
                            await _unitOfWork.Repository<SelectedBlogCategory>().Delete(selectedBlogCategory);
                        }
                    }
                    #endregion

                    await AddNewTags(model.SelectedTags);

                    #region Update SourceTags
                    var sourceTags = await _unitOfWork.Repository<SourceTag>()
                    .Where(x => x.SourceId == model.Id && x.SourceType == SourceType.Blog)
                    .Include(x => x.Tag)
                    .ToListAsync();

                    var addingTagNameList = model.SelectedTags
                               .Where(x => !sourceTags.Select(x => x.Tag.Name).Contains(x)).ToList();

                    var addingTagList = await _unitOfWork.Repository<Tag>()
                    .Where(x => addingTagNameList.Contains(x.Name)).ToListAsync();

                    if (addingTagList != null && addingTagList != null)
                    {
                        foreach (var tag in addingTagList)
                        {
                            await _unitOfWork.Repository<SourceTag>().Add(
                                    new SourceTag
                                    {
                                        SourceId = blog.Id,
                                        TagId = tag.Id,
                                        SourceType = SourceType.Blog
                                    });
                        }
                    }

                    var deletingSourceTagList = sourceTags.Where(x => !model.SelectedTags.Contains(x.Tag.Name)).ToList();

                    if (deletingSourceTagList != null && deletingSourceTagList.Any())
                    {
                        foreach (var sourceTag in deletingSourceTagList)
                        {
                            await _unitOfWork.Repository<SourceTag>().Delete(sourceTag);
                        }
                    }
                    #endregion

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

        public async Task<List<BlogModel>> GetTagBlogsByUrl(string url)
        {
            var sourceTags = await _unitOfWork.Repository<SourceTag>()
                .Where(x => x.Tag.Url == url && x.SourceType == SourceType.Blog)
                .Include(x => x.Tag)
                .ToListAsync();

            var comments = _unitOfWork.Repository<Comment>()
                .Where(x => !x.Deleted && x.Status == CommentStatus.Approved && x.SourceType == SourceType.Blog).AsQueryable();

            var blogs = await _unitOfWork.Repository<Blog>()
                .Where(x => sourceTags.Select(a => a.SourceId).Contains(x.Id) && x.Published && !x.Deleted && x.IsActive)
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
                  CommentCount = comments.Count(a => a.SourceId == x.Id)
              }).ToListAsync();

            return blogs;
        }
    }
}
