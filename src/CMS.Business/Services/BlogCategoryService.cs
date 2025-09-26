using CMS.Business.Exceptions;
using CMS.Business.Helper;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.BlogCategory;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IBlogCategoryService
    {
        IQueryable<BlogCategory> Get();
        Task<BlogCategoryDto> GetById(int id);
        Task<List<BlogCategoryWithCountDto>> GetAllWithCount();
        Task<BlogCategoryDto> GetByUrl(string url);
        Task<ServiceResult> Create(BlogCategoryDto model);
        Task<ServiceResult> Update(BlogCategoryDto model);
        Task<ServiceResult> Delete(int id);
    }

    public class BlogCategoryService(IUnitOfWork<CMSContext> unitOfWork) : IBlogCategoryService
    {
        public IQueryable<BlogCategory> Get()
        {
            var result = unitOfWork.Repository<BlogCategory>().Where(x => !x.Deleted);
            return result;
        }

        public async Task<List<BlogCategoryWithCountDto>> GetAllWithCount()
        {
            var list = await unitOfWork.Repository<BlogCategory>()
                .Where(x => !x.Deleted && x.IsActive)
                .Include(x => x.SelectedBlogCategories)
                .ThenInclude(x => x.Blog)
                .OrderByDescending(x => x.Id)
                .Select(x => new BlogCategoryWithCountDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Url = x.Url,
                    BlogCount = x.SelectedBlogCategories
                                 .Select(s => s.Blog)
                                 .Count(x => x.IsActive && x.Published && !x.Deleted)
                }).ToListAsync();

            return list;
        }

        public async Task<BlogCategoryDto> GetById(int id)
        {
            var blogCategory = await unitOfWork.Repository<BlogCategory>()
                .FirstOrDefault(x => x.Id == id);

            if (blogCategory is null)
                throw new NotFoundException("Kayıt bulunamadı.");

            var data = new BlogCategoryDto
            {
                Id = blogCategory.Id,
                Name = blogCategory.Name,
                Url = blogCategory.Url,
                IsActive = blogCategory.IsActive,
                IsShowHome = blogCategory.IsShowHome
            };
            return data;
        }

        public async Task<BlogCategoryDto> GetByUrl(string url)
        {
            var blogCategory = await unitOfWork.Repository<BlogCategory>()
                .Where(x => !x.Deleted && x.Url == url)
                .Select(x => new BlogCategoryDto
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    IsShowHome = x.IsShowHome,
                    Name = x.Name,
                    Url = x.Url
                }).FirstOrDefaultAsync();

            return blogCategory;
        }

        public async Task<ServiceResult> Create(BlogCategoryDto model)
        {
            model.Url = UrlHelper.FriendlyUrl(model.Name);

            var isExist = await unitOfWork.Repository<BlogCategory>()
                .Any(x => !x.Deleted && x.Url == model.Url);

            if (isExist)
                throw new FoundException("Url ile daha önce kayıt mevcuttur.");

            await unitOfWork.Repository<BlogCategory>()
                .Add(new BlogCategory()
                {
                    Deleted = false,
                    IsActive = model.IsActive,
                    IsShowHome = model.IsShowHome,
                    Name = model.Name,
                    Url = model.Url
                });

            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Update(BlogCategoryDto model)
        {
            var blogCategory = await unitOfWork.Repository<BlogCategory>()
                .FirstOrDefault(x => !x.Deleted && x.Id == model.Id);

            if (blogCategory is null)
                throw new NotFoundException("Kayıt bulunamadı.");

            model.Url = UrlHelper.FriendlyUrl(model.Name);

            var isExist = await unitOfWork.Repository<BlogCategory>()
                .Any(x => !x.Deleted && x.Url == model.Url && x.Id != model.Id);

            if (isExist)
                throw new FoundException("Url ile daha önce kayıt mevcuttur.");

            blogCategory.Name = model.Name;
            blogCategory.Url = model.Url;
            blogCategory.IsActive = model.IsActive;
            blogCategory.IsShowHome = model.IsShowHome;

            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var blogCategory = await unitOfWork.Repository<BlogCategory>()
                .FirstOrDefault(c => c.Id == id);

            if (blogCategory is null)
                throw new NotFoundException("Kayıt bulunamadı");

            blogCategory.Deleted = true;

            unitOfWork.Repository<BlogCategory>().Update(blogCategory);
            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }
    }
}
