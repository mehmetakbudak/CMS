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
    public interface IAuthorService
    {
        IQueryable<Author> GetAll();
        ServiceResult Post(AuthorModel model);
        ServiceResult Put(AuthorModel model);
        ServiceResult Delete(int id);
    }

    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork<CMSContext> unitOfWork;
        public AuthorService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Author> GetAll()
        {
            var entity = unitOfWork.Repository<Author>()
                .GetAll(x =>
                !x.Deleted && x.IsActive);
            return entity;
        }

        public ServiceResult Post(AuthorModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK, Message = AlertMessages.Post };

            if (model.Id == 0)
            {
                if (model.File != null)
                {

                }

                var entity = new Author()
                {
                    Deleted = false,
                    IsActive = model.IsActive,
                    Name = model.Name,
                    Surname = model.Surname,
                    Resume = model.Resume,

                };
                unitOfWork.Repository<Author>().Add(entity);
                unitOfWork.Save();
            }
            return serviceResult;
        }

        public ServiceResult Put(AuthorModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK, Message = AlertMessages.Put };

            var entity = unitOfWork.Repository<Author>()
                .Find(x => x.Id == model.Id && !x.Deleted);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.Surname = model.Surname;
                entity.Resume = model.Resume;
                entity.IsActive = model.IsActive;
                unitOfWork.Repository<Author>().Update(entity);
                unitOfWork.Save();
            }
            else
            {
                throw new NotFoundException("Kayıt buluınamadı.");
            }
            return serviceResult;
        }

        public ServiceResult Delete(int id)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = (int)HttpStatusCode.OK, Message = AlertMessages.Delete };

            var entity = unitOfWork.Repository<Author>()
                .Find(x => x.Id == id && !x.Deleted);
            if (entity != null)
            {
                entity.Deleted = true;
                unitOfWork.Repository<Author>().Update(entity);
                unitOfWork.Save();
            }
            else
            {
                throw new NotFoundException("Kayıt buluınamadı.");
            }
            return serviceResult;
        }
    }
}
