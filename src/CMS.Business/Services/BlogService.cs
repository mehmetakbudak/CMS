using CMS.Business.Exceptions;
using CMS.Business.Extensions;
using CMS.Business.Helper;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.Blog;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IBlogService
    {
        IQueryable<BlogGridDto> Get();
        Task<IEnumerable<BlogGridDto>> GetAllActive();
        Task<BlogDto> GetById(int id);
        PagedResponse<List<BlogListDto>> GetBlogs(BlogListFilterDto dto);
        Task<IQueryable<BlogListDto>> GetBlogsByCategoryUrl(string blogCategoryUrl);
        Task<BlogDetailByIdDto> GetDetailById(int id);
        Task<List<BlogListDto>> GetTagBlogsByUrl(string url);
        Task<List<MostReadBlogDto>> MostRead(string blogCategoryUrl = null);
        Task<ServiceResult> Create(BlogCreateOrUpdateDto model);
        Task<ServiceResult> Update(BlogCreateOrUpdateDto model);
        Task<ServiceResult> Seen(BlogSeenDto dto);
        Task<ServiceResult> Delete(int id);
    }

    public class BlogService(
            IUnitOfWork<CMSContext> unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IDbConnection dbConnection,
            IWebHostEnvironment environment) : IBlogService
    {
        public IQueryable<BlogGridDto> Get()
        {
            return unitOfWork.Repository<Blog>()
                .Where(x => !x.Deleted)
                .Include(x => x.User)
                .Select(x => new BlogGridDto
                {
                    DisplayOrder = x.DisplayOrder,
                    Id = x.Id,
                    Title = x.Title,
                    Url = x.Url,
                    IsActive = x.IsActive,
                    Published = x.Published,
                    ImageUrl = x.ImageUrl,
                    InsertedDate = x.InsertedDate,
                    UpdatedDate = x.UpdatedDate,
                    NumberOfView = x.NumberOfView,
                    UserName = $"{x.User.Name} {x.User.Surname}"
                });
        }

        public Task<IEnumerable<BlogGridDto>> GetAllActive()
        {
            var list = dbConnection.QueryAsync<BlogGridDto>("select B.[Id], B.[DisplayOrder], B.[Title], B.[Url], B.[IsActive], B.[Published], B.[ImageUrl], B.[InsertedDate], B.[UpdatedDate], B.[NumberOfView], U.[Name] + ' ' + U.[Surname] as UserName from Blogs as B inner join Users U on B.UserId=U.Id where B.Deleted=0 and B.IsActive=1 and B.Published=1 order by B.DisplayOrder");
            return list;
        }

        public PagedResponse<List<BlogListDto>> GetBlogs(BlogListFilterDto dto)
        {
            var blogs = unitOfWork.Repository<Blog>()
                .Where(x => x.IsActive && !x.Deleted)
                .Include(x => x.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(dto.SearchText))
            {
                blogs = blogs.Where(x => x.Content.Contains(dto.SearchText) ||
                                         x.Title.Contains(dto.SearchText) ||
                                         x.Description.Contains(dto.SearchText));
            }

            var comments = unitOfWork.Repository<Comment>()
                .Where(x => !x.Deleted && x.Status == CommentStatus.Approved && x.SourceType == SourceType.Blog)
                .AsQueryable();

            var data = blogs.OrderBy(x => x.DisplayOrder)
                .Select(x => new BlogListDto()
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
                });

            var list = PaginationHelper.CreatePagedReponse(data, dto);
            return list;
        }

        public async Task<BlogDto> GetById(int id)
        {
            var blog = await unitOfWork.Repository<Blog>()
                .Where(x => !x.Deleted && x.Id == id)
                .Include(x => x.SelectedBlogCategories)
                .ThenInclude(x => x.BlogCategory)
                .FirstOrDefaultAsync();

            if (blog is null)
                throw new NotFoundException();

            var blogTags = await unitOfWork.Repository<SourceTag>()
                .Where(x => x.SourceId == blog.Id && x.SourceType == SourceType.Blog)
                .ToListAsync();

            var model = new BlogDto()
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
                BlogCategoryIds = blog.SelectedBlogCategories.Select(x => x.BlogCategoryId).ToList(),
                SelectedTagIds = blogTags.Select(x => x.TagId).ToList()
            };
            return model;
        }

        public async Task<IQueryable<BlogListDto>> GetBlogsByCategoryUrl(string blogCategoryUrl)
        {
            var blogCategory = await unitOfWork.Repository<BlogCategory>()
                .Where(x => x.Url == blogCategoryUrl && !x.Deleted).FirstOrDefaultAsync();

            if (blogCategory is null)
                throw new NotFoundException("Blog kategorisi bulunamadı.");

            var comments = unitOfWork.Repository<Comment>()
                .Where(x => !x.Deleted && x.Status == CommentStatus.Approved &&
                            x.SourceType == SourceType.Blog)
                .AsQueryable();

            var data = unitOfWork.Repository<SelectedBlogCategory>()
                 .Where(x => !x.Blog.Deleted &&
                             x.Blog.Published &&
                             x.Blog.IsActive && x
                             .BlogCategoryId == blogCategory.Id)
                 .Include(x => x.Blog)
                 .Include(x => x.BlogCategory)
                 .Select(x => x.Blog)
                 .OrderBy(x => x.DisplayOrder)
                 .Select(x => new BlogListDto()
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
                 }).AsQueryable();

            return data;
        }

        public async Task<BlogDetailByIdDto> GetDetailById(int id)
        {
            var blog = await unitOfWork.Repository<Blog>()
                .Where(x => x.Id == id && !x.Deleted && x.Published && x.IsActive)
                .Include(x => x.SelectedBlogCategories)
                .ThenInclude(x => x.BlogCategory)
                .Include(x => x.User)
                .FirstOrDefaultAsync();

            if (blog is null)
                throw new NotFoundException("Kayıt bulunamadı.");

            var commentCount = await unitOfWork.Repository<Comment>()
                .Where(x => x.SourceType == SourceType.Blog && x.SourceId == id && !x.Deleted && x.Status == CommentStatus.Approved).CountAsync();

            var blogTags = await unitOfWork.Repository<SourceTag>()
                .Where(x => x.SourceType == SourceType.Blog && x.SourceId == id)
                .Include(x => x.Tag)
                .Select(x => x.Tag).ToListAsync();

            var data = new BlogDetailByIdDto
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
                                     .Select(x => new BlogDetailCategoryDto()
                                     {
                                         Id = x.Id,
                                         Name = x.Name,
                                         Url = x.Url
                                     }).ToList(),
                BlogTags = blogTags.Select(x => new BlogDetailTagDto
                {
                    Name = x.Name,
                    Url = x.Url
                }).ToList()
            };

            return data;
        }

        public async Task<List<BlogListDto>> GetTagBlogsByUrl(string url)
        {
            var sourceTags = await unitOfWork.Repository<SourceTag>()
                .Where(x => x.Tag.Url == url && x.SourceType == SourceType.Blog)
                .Include(x => x.Tag)
                .ToListAsync();

            var comments = unitOfWork.Repository<Comment>()
                .Where(x => !x.Deleted &&
                            x.Status == CommentStatus.Approved &&
                            x.SourceType == SourceType.Blog)
                .AsQueryable();

            var list = await unitOfWork.Repository<Blog>()
                .Where(x => sourceTags.Select(a => a.SourceId).Contains(x.Id) &&
                            x.Published && !x.Deleted && x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new BlogListDto()
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

        public async Task<List<MostReadBlogDto>> MostRead(string blogCategoryUrl = null)
        {
            IQueryable<Blog> data = null;

            if (!string.IsNullOrEmpty(blogCategoryUrl))
            {
                data = unitOfWork.Repository<SelectedBlogCategory>()
                    .Where(x => !x.Blog.Deleted && x.Blog.IsActive && x.Blog.Published && x.BlogCategory.Url == blogCategoryUrl)
                    .Include(x => x.Blog)
                    .Include(x => x.BlogCategory)
                    .Select(x => x.Blog).AsQueryable();
            }
            else
            {
                data = unitOfWork.Repository<Blog>()
                    .Where(x => !x.Deleted && x.IsActive && x.Published);
            }

            var list = await data.OrderByDescending(x => x.NumberOfView)
                .Take(5)
                .Select(x => new MostReadBlogDto()
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    Title = x.Title,
                    Url = x.Url,
                    InsertedDate = x.InsertedDate
                }).ToListAsync();

            return list;
        }

        public async Task<ServiceResult> Create(BlogCreateOrUpdateDto model)
        {
            var strategy = unitOfWork.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                try
                {
                    if (model is null)
                        throw new NotFoundException("Model null olamaz.");

                    if (model.Image is null)
                        throw new BadRequestException("Resim ekleyiniz.");

                    #region Add File
                    var extension = Path.GetExtension(model.Image.FileName);
                    string imageUrl = $"/images/blogs/{Guid.NewGuid()}{extension}";
                    var fileUploadUrl = $"{environment.WebRootPath}{imageUrl}";
                    model.Image.CopyTo(new FileStream(fileUploadUrl, FileMode.Create));
                    #endregion

                    var loginUser = httpContextAccessor.HttpContext.User.Parse();

                    await unitOfWork.CreateTransaction();

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

                    await unitOfWork.Repository<Blog>().Add(blog);
                    await unitOfWork.Save();
                    #endregion

                    #region Add SelectedBlogCategory
                    foreach (var blogCategoryId in model.BlogCategoryIds)
                    {
                        await unitOfWork.Repository<SelectedBlogCategory>()
                            .Add(new SelectedBlogCategory
                            {
                                BlogCategoryId = blogCategoryId,
                                BlogId = blog.Id
                            });
                    }
                    #endregion

                    #region Add SourceTag
                    var tags = await unitOfWork.Repository<Tag>()
                        .Where(x => model.SelectedTagIds.Contains(x.Id)).ToListAsync();

                    foreach (var tag in tags)
                    {
                        await unitOfWork.Repository<SourceTag>().Add(new SourceTag
                        {
                            SourceId = blog.Id,
                            TagId = tag.Id,
                            SourceType = SourceType.Blog
                        });
                    }
                    #endregion

                    await unitOfWork.Save();
                    await unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    await unitOfWork.Rollback();
                    throw new Exception(ex.Message);
                }
            });
            return ServiceResult.Success(200);
        }        

        public async Task<ServiceResult> Update(BlogCreateOrUpdateDto model)
        {
            var strategy = unitOfWork.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                try
                {
                    var blog = await unitOfWork.Repository<Blog>()
                        .FirstOrDefault(x => x.Id == model.Id && !x.Deleted);

                    if (blog is null)
                        throw new NotFoundException("Kayıt bulunamadı.");

                    await unitOfWork.CreateTransaction();

                    #region Update File
                    if (model.Image != null)
                    {
                        var currentFileUrl = Path.Combine(environment.WebRootPath, blog.ImageUrl);

                        if (File.Exists(currentFileUrl))
                        {
                            File.Delete(currentFileUrl);
                        }
                        var extension = Path.GetExtension(model.Image.FileName);
                        string imageUrl = $"/images/blogs/{Guid.NewGuid()}{extension}";
                        var fileUploadUrl = $"{environment.WebRootPath}{imageUrl}";
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
                    var selectedBlogCategories = await unitOfWork.Repository<SelectedBlogCategory>()
                        .Where(x => x.BlogId == model.Id).ToListAsync();

                    var addingBlogCategoryList = model.BlogCategoryIds
                               .Where(x => !selectedBlogCategories.Select(x => x.BlogCategoryId).Contains(x)).ToList();

                    if (addingBlogCategoryList != null && addingBlogCategoryList != null)
                    {
                        foreach (var blogCategoryId in addingBlogCategoryList)
                        {
                            await unitOfWork.Repository<SelectedBlogCategory>()
                                 .Add(new SelectedBlogCategory()
                                 {
                                     BlogCategoryId = blogCategoryId,
                                     BlogId = blog.Id
                                 });
                        }
                    }

                    var deletingBlogCategoryList = selectedBlogCategories.Where(x => !model.BlogCategoryIds.Contains(x.BlogCategoryId)).ToList();

                    if (deletingBlogCategoryList != null && deletingBlogCategoryList.Any())
                    {
                        foreach (var selectedBlogCategory in deletingBlogCategoryList)
                        {
                            await unitOfWork.Repository<SelectedBlogCategory>().Delete(selectedBlogCategory);
                        }
                    }
                    #endregion

                    #region Update SourceTags
                    var sourceTags = await unitOfWork.Repository<SourceTag>()
                    .Where(x => x.SourceId == model.Id && x.SourceType == SourceType.Blog)
                    .Include(x => x.Tag)
                    .ToListAsync();

                    var addingTagIds = model.SelectedTagIds
                               .Where(x => !sourceTags.Select(x => x.TagId).Contains(x)).ToList();

                    var addingTagList = await unitOfWork.Repository<Tag>()
                    .Where(x => addingTagIds.Contains(x.Id)).ToListAsync();

                    if (addingTagList != null && addingTagList != null)
                    {
                        foreach (var tag in addingTagList)
                        {
                            await unitOfWork.Repository<SourceTag>().Add(
                                    new SourceTag
                                    {
                                        SourceId = blog.Id,
                                        TagId = tag.Id,
                                        SourceType = SourceType.Blog
                                    });
                        }
                    }

                    var deletingSourceTagList = sourceTags.Where(x => !model.SelectedTagIds.Contains(x.TagId)).ToList();

                    if (deletingSourceTagList != null && deletingSourceTagList.Any())
                    {
                        foreach (var sourceTag in deletingSourceTagList)
                        {
                            await unitOfWork.Repository<SourceTag>().Delete(sourceTag);
                        }
                    }
                    #endregion

                    await unitOfWork.Save();
                    await unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    await unitOfWork.Rollback();
                    throw new Exception(ex.Message);
                }
            });
            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Seen(BlogSeenDto dto)
        {
            var blog = await unitOfWork.Repository<Blog>()
                .FirstOrDefault(x => !x.Deleted && x.Published && x.IsActive && x.Id == dto.Id);

            if (blog is null)
                throw new NotFoundException();

            blog.NumberOfView++;

            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var blog = await unitOfWork.Repository<Blog>()
                      .FirstOrDefault(x => x.Id == id && !x.Deleted);

            if (blog is null)
                throw new NotFoundException("Kayıt bulunamadı.");

            blog.Deleted = true;
            await unitOfWork.Save();

            var currentFileUrl = Path.Combine(environment.WebRootPath, blog.ImageUrl);

            if (File.Exists(currentFileUrl))
                File.Delete(currentFileUrl);

            return ServiceResult.Success(200);
        }
    }
}
