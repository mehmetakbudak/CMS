using CMS.Business.Exceptions;
using CMS.Business.Helper;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.Page;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IPageService
    {
        IQueryable<Page> Get();
        Task<PageDto> GetById(int id);
        Task<PageDetailDto> GetByUrl(string url);
        Task<ServiceResult> Create(PageDto dto);
        Task<ServiceResult> Update(PageDto dto);
        Task<ServiceResult> Delete(int id);
    }

    public class PageService(
        IUnitOfWork<CMSContext> unitOfWork,
        IMenuItemService menuItemService) : IPageService
    {
        public IQueryable<Page> Get()
        {
            var list = unitOfWork.Repository<Page>()
                .Where(x => !x.Deleted)
                .AsNoTracking()
                .AsQueryable();
            return list;
        }

        public async Task<PageDto> GetById(int id)
        {
            var page = await unitOfWork.Repository<Page>()
                .Where(x => !x.Deleted && x.Id == id)
                .Include(x => x.MenuItem)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (page is null)
                throw new NotFoundException("Page.Notfound");

            return new PageDto
            {
                Content = page.Content,
                Id = page.Id,
                IsActive = page.IsActive,
                MenuId = page.MenuItem?.MenuId,
                MenuItemId = page.MenuItemId,
                Name = page.Name,
                Published = page.Published,
                Title = page.Title
            };
        }

        public async Task<PageDetailDto> GetByUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new NotFoundException("Page.Notfound");

            var page = await unitOfWork.Repository<Page>()
                .Where(x => !x.Deleted && x.Published && x.IsActive && x.Url == url)
                .Include(x => x.MenuItem)
                .ThenInclude(x => x.Menu)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (page is null)
                throw new NotFoundException("Page.Notfound");

            var result = new PageDetailDto
            {
                Id = page.Id,
                Name = page.Name,
                Title = page.Title,
                Content = page.Content,
                MenuItemId = page.MenuItemId,
                Url = page.Url
            };

            if (page.MenuItemId != null)
                result.MenuItems = await menuItemService.GetMenuItemsByMenuId(page.MenuItem.MenuId);

            return result;
        }

        public async Task<ServiceResult> Create(PageDto dto)
        {
            var url = await CreateUrl(dto);

            var isExist = await unitOfWork.Repository<Page>()
                .Any(x => !x.Deleted && x.Url == url);

            if (isExist)
                throw new FoundException("Page.AlreadyExist");

            var page = new Page
            {
                IsActive = dto.IsActive,
                Name = dto.Name,
                Title = dto.Title,
                Content = dto.Content,
                Published = dto.Published,
                Url = url,
                Deleted = false,
                MenuItemId = dto.MenuItemId
            };

            await unitOfWork.Repository<Page>().Add(page);
            await unitOfWork.Save();

            return ServiceResult.Success(204);
        }

        public async Task<ServiceResult> Update(PageDto dto)
        {
            var url = await CreateUrl(dto);

            var page = await unitOfWork.Repository<Page>()
                .FirstOrDefault(x => x.Id == dto.Id);

            if (page is null)
                throw new NotFoundException("Page.Notfound");

            var isExist = await unitOfWork.Repository<Page>()
                .Any(x => !x.Deleted && x.Id != dto.Id && x.Url == url);

            if (isExist)
                throw new FoundException();

            page.Content = dto.Content;
            page.IsActive = dto.IsActive;
            page.Name = dto.Name;
            page.Published = dto.Published;
            page.Title = dto.Title;
            page.Url = url;
            page.MenuItemId = dto.MenuItemId;

            unitOfWork.Repository<Page>().Update(page);
            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }

        private async Task<string> CreateUrl(PageDto dto)
        {
            string url = string.Empty;

            if (dto.MenuItemId.HasValue)
            {
                var menuItem = await unitOfWork.Repository<MenuItem>()
                    .FirstOrDefault(x => !x.Deleted && x.Id == dto.MenuItemId.Value);

                if (menuItem != null)
                    url = menuItem.Url;
            }
            if (string.IsNullOrEmpty(url))
                url = $"/page/{UrlHelper.FriendlyUrl(dto.Title)}";

            return url;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var page = await unitOfWork.Repository<Page>()
                .FirstOrDefault(x => x.Id == id);

            if (page is null)
                throw new NotFoundException("Page.Notfound");

            page.Deleted = true;

            unitOfWork.Repository<Page>().Update(page);
            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }
    }
}
