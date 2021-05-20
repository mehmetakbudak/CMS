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
            try
            {
                var chat = new Chat
                {
                    ChatGuid = Guid.NewGuid(),
                    ChatStatus = ChatStatus.NotStarted,
                    Deleted = false,
                    EmailAddress = model.EmailAddress,
                    InsertDate = DateTime.Now,
                    NameSurname = model.NameSurname,
                    PhoneNumber = model.PhoneNumber
                };
                unitOfWork.Repository<Chat>().Add(chat);
                unitOfWork.Save();
                result.Data = new
                {
                    Guid = chat.ChatGuid
                };
                result.Message = "Canlı destek kaydınız başarıyla alınmıştır.";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            return result;
        }
    }
}
