using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Consts;
using CMS.Model.Entity;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface IPageService
    {
        IQueryable<Page> GetAll();
        Page GetById(int id);
        Page GetByUrl(string url);
        ServiceResult Post(Page model);
        ServiceResult Put(Page model);
        ServiceResult Delete(int id);
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

        public Page GetById(int id)
        {
            var page = _unitOfWork.Repository<Page>().FirstOrDefault(x => !x.Deleted && x.Id == id);
            return page;
        }

        public Page GetByUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            var page = _unitOfWork.Repository<Page>().FirstOrDefault(x => !x.Deleted && x.Published && x.IsActive && x.Url == url);            
            return page;
        }

        public ServiceResult Post(Page model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var isExist = _unitOfWork.Repository<Page>().Any(x => !x.Deleted && x.Url == model.Url);
            if (isExist)
            {
                throw new FoundException(AlertMessages.UrlAlreadyExist);
            }

            model.Deleted = false;
            _unitOfWork.Repository<Page>().Add(model);
            _unitOfWork.Save();

            return serviceResult;
        }

        public ServiceResult Put(Page model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var page = _unitOfWork.Repository<Page>().FirstOrDefault(x => x.Id == model.Id);
            
            if (page == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            var isExist = _unitOfWork.Repository<Page>().Any(x => !x.Deleted && x.Id != model.Id && x.Url == model.Url);

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
            _unitOfWork.Save();

            return serviceResult;
        }

        public ServiceResult Delete(int id)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var page = _unitOfWork.Repository<Page>().FirstOrDefault(x => x.Id == id);

            if (page != null)
            {
                page.Deleted = true;
                _unitOfWork.Save();
            }
            else
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }
            return serviceResult;
        }
    }
}
