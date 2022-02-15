using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Model;
using System;
using System.Net;

namespace CMS.Service
{
    public interface IContactService
    {
        ServiceResult Post(ContactModel model);
    }

    public class ContactService : IContactService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public ContactService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceResult Post(ContactModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

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
            result.Message = "Mesajınız başarıyla kaydedilmiştir.";

            return result;
        }
    }
}
