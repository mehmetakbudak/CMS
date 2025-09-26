using System;
using System.Linq;
using CMS.Data.Context;
using CMS.Storage.Entity;
using CMS.Data.Repository;
using System.Threading.Tasks;
using CMS.Business.Exceptions;
using CMS.Storage.Dtos.Contact;
using CMS.Storage.Dtos.Response;

namespace CMS.Business.Services
{
    public interface IContactService
    {
        IQueryable<ContactListDto> Get();
        Task<ServiceResult> Create(ContactDto model);
        Task<ServiceResult> Delete(int id);
    }

    public class ContactService(IUnitOfWork<CMSContext> unitOfWork) : IContactService
    {
        public IQueryable<ContactListDto> Get()
        {
            return unitOfWork.Repository<Contact>()
                .Where(x => !x.ContactCategory.Deleted && x.ContactCategory.IsActive && !x.ContactCategory.Deleted)
                .Select(x => new ContactListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname,
                    Message = x.Message,
                    EmailAddress = x.EmailAddress,
                    ContactCategoryId = x.ContactCategoryId,
                    ContactCategoryName = x.ContactCategory.Name,
                    InsertedDate = DateTime.Now
                }).AsQueryable();
        }

        public async Task<ServiceResult> Create(ContactDto model)
        {
            var contact = new Contact
            {
                ContactCategoryId = model.ContactCategoryId,
                EmailAddress = model.EmailAddress,
                Message = model.Message,
                Name = model.Name,
                Surname = model.Surname,
                InsertedDate = DateTime.Now
            };

            await unitOfWork.Repository<Contact>().Add(contact);
            await unitOfWork.Save();

            return ServiceResult.Success(204);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var contactMessage = await unitOfWork.Repository<Contact>()
                .FirstOrDefault(x => x.Id == id);

            if (contactMessage is null)
                throw new NotFoundException("");

            await unitOfWork.Repository<Contact>().Delete(contactMessage);
            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }
    }
}
