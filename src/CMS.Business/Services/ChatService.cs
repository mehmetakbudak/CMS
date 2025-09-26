using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.Chat;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using System;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IChatService
    {
        Task<ServiceResult> Create(ChatDto model);
    }

    public class ChatService(IUnitOfWork<CMSContext> unitOfWork) : IChatService
    {
        public async Task<ServiceResult> Create(ChatDto model)
        {
            var chat = new Chat
            {
                Code = Guid.NewGuid(),
                Status = ChatStatus.NotStarted,
                EmailAddress = model.EmailAddress,
                InsertedDate = DateTime.Now,
                Name = model.Name,
                Surname = model.Surname,
                Phone = model.Phone
            };

            await unitOfWork.Repository<Chat>().Add(chat);

            await unitOfWork.Save();

            var data = new
            {
                Guid = chat.Code
            };
            return ServiceResult.Success(200, data, "Canlı destek kaydınız başarıyla alınmıştır.");
        }
    }
}
