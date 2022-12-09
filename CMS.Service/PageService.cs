using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Consts;
using CMS.Model.Entity;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IPageService
    {
        IQueryable<Page> GetAll();
        Task<Page> GetById(int id);
        Task<Page> GetByUrl(string url);
        Task<ServiceResult> Post(Page model);
        Task<ServiceResult> Put(Page model);
        Task<ServiceResult> Delete(int id);
    }

    public class PageService : IPageService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public PageService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Page> GetAll()
        {
            var list = _unitOfWork.Repository<Page>()
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.Id)
                .AsQueryable();
            return list;
        }

        public async Task<Page> GetById(int id)
        {
            var page = await _unitOfWork.Repository<Page>()
                .FirstOrDefault(x => !x.Deleted && x.Id == id);

            return page;
        }

        public async Task<Page> GetByUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            var page = await _unitOfWork.Repository<Page>()
                .FirstOrDefault(x => !x.Deleted && x.Published && x.IsActive && x.Url == url);

            return page;
        }

        public async Task<ServiceResult> Post(Page model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var isExist = await _unitOfWork.Repository<Page>()
                .Any(x => !x.Deleted && x.Url == model.Url);

            if (isExist)
            {
                throw new FoundException(AlertMessages.UrlAlreadyExist);
            }

            model.Deleted = false;

            await _unitOfWork.Repository<Page>().Add(model);
            await _unitOfWork.Save();

            return serviceResult;
        }

        public async Task<ServiceResult> Put(Page model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var page = await _unitOfWork.Repository<Page>()
                .FirstOrDefault(x => x.Id == model.Id);

            if (page == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            var isExist = await _unitOfWork.Repository<Page>()
                .Any(x => !x.Deleted && x.Id != model.Id && x.Url == model.Url);

            if (isExist)
            {
                throw new FoundException(AlertMessages.UrlAlreadyExist);
            }

            page.Content = model.Content;
            page.IsActive = model.IsActive;
            page.Name = model.Name;
            page.Published = model.Published;
            page.Title = model.Title;
            page.Url = model.Url;

            await _unitOfWork.Save();

            return serviceResult;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var page = await _unitOfWork.Repository<Page>()
                .FirstOrDefault(x => x.Id == id);

            if (page == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            page.Deleted = true;

            await _unitOfWork.Save();

            return serviceResult;
        }
    }
}
