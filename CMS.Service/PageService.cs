using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Model;
using System;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface IPageService
    {
        IQueryable<Page> GetAll();
        Page GetById(int id);
        ServiceResult GetByUrl(string url);
        ServiceResult CreateOrUpdate(Page model);
        ServiceResult Delete(int id);
    }

    public class PageService : IPageService
    {
        private readonly IUnitOfWork<CMSContext> unitOfWork;
        public PageService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Page> GetAll()
        {
            var list = unitOfWork.Repository<Page>()
                .GetAll(x => !x.Deleted)
                .OrderByDescending(x => x.Id)
                .AsQueryable();
            return list;
        }

        public Page GetById(int id)
        {
            var page = unitOfWork.Repository<Page>().Find(x => !x.Deleted && x.Id == id);
            return page;
        }

        public ServiceResult GetByUrl(string url)
        {
            try
            {
                var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

                if (string.IsNullOrEmpty(url))
                {
                    result.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.Message = "Url bulunamadı.";
                }

                var page = unitOfWork.Repository<Page>().Find(x => !x.Deleted && x.Published && x.IsActive && x.Url == url);

                if (page != null)
                {
                    result.Data = page;
                }
                else
                {
                    result.Message = $"{url} ile sayfa bulunamadı.";
                    result.StatusCode = (int)HttpStatusCode.NotFound;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ServiceResult CreateOrUpdate(Page model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            try
            {
                if (model.Id == 0)
                {
                    model.Deleted = false;
                    unitOfWork.Repository<Page>().Add(model);
                    unitOfWork.Save();
                }
                else
                {
                    var page = unitOfWork.Repository<Page>()
                        .Find(x => x.Id == model.Id);
                    if (page != null)
                    {
                        page.Content = model.Content;
                        page.IsActive = model.IsActive;
                        page.MenuId = model.MenuId;
                        page.Name = model.Name;
                        page.Published = model.Published;
                        page.Title = model.Title;
                        page.Url = model.Url;
                        unitOfWork.Save();
                    }
                    else
                    {
                        throw new Exception("Kayıt bulunamadı.");
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public ServiceResult Delete(int id)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            try
            {
                var page = unitOfWork.Repository<Page>()
                       .Find(x => x.Id == id);

                if (page != null)
                {
                    page.Deleted = true;
                    unitOfWork.Save();
                }
                else
                {
                    serviceResult.StatusCode = (int)HttpStatusCode.NotFound;
                    serviceResult.Message = "Kayıt bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }
    }
}
