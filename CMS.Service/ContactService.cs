using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Consts;
using CMS.Model.Entity;
using CMS.Model.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface IContactService
    {
        List<Contact> GetDmo();
        ServiceResult Post(ContactModel model);
        ServiceResult Delete(int id);
    }

    public class ContactService : IContactService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public ContactService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }        

        public ServiceResult Delete(int id)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            var contactMessage = _unitOfWork.Repository<Contact>()
                .FirstOrDefault(x => x.Id == id);
            if (contactMessage == null)
            {
                throw new DllNotFoundException(AlertMessages.NotFound);
            }
            _unitOfWork.Repository<Contact>().Delete(contactMessage);
            _unitOfWork.Save();
            result.Message = AlertMessages.Delete;
            return result;
        }

        public List<Contact> GetDmo()
        {
            return _unitOfWork.Repository<Contact>()
                .Where(x => !x.ContactCategory.Deleted && x.ContactCategory.IsActive)
                .Include(x => x.ContactCategory)
                .OrderByDescending(x => x.InsertedDate)
                .ToList();
        }

        public ServiceResult Post(ContactModel model)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var contact = new Contact
            {
                ContactCategoryId = model.ContactCategoryId,
                EmailAddress = model.EmailAddress,
                Message = model.Message,
                Name = model.Name,
                Surname = model.Surname,
                InsertedDate = DateTime.Now
            };
            _unitOfWork.Repository<Contact>().Add(contact);
            _unitOfWork.Save();
            result.Message = AlertMessages.Post;

            return result;
        }
    }
}
