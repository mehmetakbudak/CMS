using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Consts;
using CMS.Storage.Entity;
using CMS.Storage.Model;
using CMS.Service.Exceptions;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IAuthorService
    {
        IQueryable<Author> GetAll();
        Task<ServiceResult> Post(AuthorModel model);
        Task<ServiceResult> Put(AuthorModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public AuthorService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Author> GetAll()
        {
            var entity = _unitOfWork.Repository<Author>()
                .Where(x =>
                !x.Deleted && x.IsActive);
            return entity;
        }

        public async Task<ServiceResult> Post(AuthorModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Post };

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
                await _unitOfWork.Repository<Author>().Add(entity);
                await _unitOfWork.Save();
            }
            return serviceResult;
        }

        public async Task<ServiceResult> Put(AuthorModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Put };

            var entity = await _unitOfWork.Repository<Author>()
                .FirstOrDefault(x => x.Id == model.Id && !x.Deleted);

            if (entity != null)
            {
                entity.Name = model.Name;
                entity.Surname = model.Surname;
                entity.Resume = model.Resume;
                entity.IsActive = model.IsActive;

                _unitOfWork.Repository<Author>().Update(entity);
                await _unitOfWork.Save();
            }
            else
            {
                throw new NotFoundException("Kayıt buluınamadı.");
            }
            return serviceResult;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Delete };

            var entity = await _unitOfWork.Repository<Author>().FirstOrDefault(x => x.Id == id && !x.Deleted);

            if (entity != null)
            {
                entity.Deleted = true;

                _unitOfWork.Repository<Author>().Update(entity);
                await _unitOfWork.Save();
            }
            else
            {
                throw new NotFoundException("Kayıt buluınamadı.");
            }
            return serviceResult;
        }
    }
}
