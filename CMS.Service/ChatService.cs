using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Model;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IChatService
    {
        Task<ServiceResult> Post(ChatModel model);
    }

    public class ChatService : IChatService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public ChatService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Post(ChatModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = HttpStatusCode.OK };

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

            await _unitOfWork.Repository<Chat>().Add(chat);

            await _unitOfWork.Save();

            result.Data = new
            {
                Guid = chat.Code
            };

            result.Message = "Canlı destek kaydınız başarıyla alınmıştır.";

            return result;
        }
    }
}
