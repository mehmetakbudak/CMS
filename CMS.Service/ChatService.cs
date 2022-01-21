using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Model;
using System;
using System.Net;

namespace CMS.Service
{
    public interface IChatService
    {
        ServiceResult Post(ChatModel model);
    }

    public class ChatService : IChatService
    {
        private readonly IUnitOfWork<CMSContext> unitOfWork;
        public ChatService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ServiceResult Post(ChatModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var chat = new Chat
            {
                Code = Guid.NewGuid(),
                Status = ChatStatus.NotStarted,
                EmailAddress = model.EmailAddress,
                InsertedDate = DateTime.Now,
                Name= model.Name,
                Surname=model.Surname,
                Phone = model.Phone
            };
            unitOfWork.Repository<Chat>().Add(chat);
            unitOfWork.Save();
            result.Data = new
            {
                Guid = chat.Code
            };
            result.Message = "Canlı destek kaydınız başarıyla alınmıştır.";

            return result;
        }
    }
}
