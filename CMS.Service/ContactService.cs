using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Consts;
using CMS.Storage.Entity;
using CMS.Storage.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IContactService
    {
        IQueryable<Contact> Get();
        Task<ServiceResult> Post(ContactModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class ContactService : IContactService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public ContactService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var contactMessage = await _unitOfWork.Repository<Contact>()
                .FirstOrDefault(x => x.Id == id);

            if (contactMessage == null)
            {
                throw new DllNotFoundException(AlertMessages.NotFound);
            }

            await _unitOfWork.Repository<Contact>().Delete(contactMessage);
            await _unitOfWork.Save();

            result.Message = AlertMessages.Delete;

            return result;
        }

        public IQueryable<Contact> Get()
        {
            return _unitOfWork.Repository<Contact>()
                .Where(x => !x.ContactCategory.Deleted && x.ContactCategory.IsActive)
                .Include(x => x.ContactCategory)
                .OrderByDescending(x => x.InsertedDate).AsQueryable();
        }

        public async Task<ServiceResult> Post(ContactModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var contact = new Contact
            {
                ContactCategoryId = model.ContactCategoryId,
                EmailAddress = model.EmailAddress,
                Message = model.Message,
                Name = model.Name,
                Surname = model.Surname,
                InsertedDate = DateTime.Now
            };

            await _unitOfWork.Repository<Contact>().Add(contact);
            await _unitOfWork.Save();

            result.Message = AlertMessages.Post;

            return result;
        }
    }
}
