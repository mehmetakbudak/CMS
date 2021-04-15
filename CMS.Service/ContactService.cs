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
        private readonly IUnitOfWork<CMSContext> unitOfWork;
        public ContactService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ServiceResult Post(ContactModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var contact = new Contact
                {
                    ContactCategoryId = model.Subject.Value,
                    EmailAddress = model.EmailAddress,
                    Message = model.Message,
                    NameSurname = model.NameSurname,
                    InsertDate = DateTime.Now
                };
                unitOfWork.Repository<Contact>().Add(contact);
                unitOfWork.Save();
                result.Message = "Mesajınız başarıyla kaydedilmiştir.";
            }
            catch (Exception ex)
            {
                result.Exceptions.Add(ex.Message);
                result.StatusCode = HttpStatusCode.InternalServerError;
            }
            return result;
        }
    }
}
