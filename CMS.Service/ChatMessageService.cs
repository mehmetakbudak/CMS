using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface IChatMessageService
    {
        List<ChatMessageModel> Get(Guid guid);
        ServiceResult PostForClient(ChatMessageModel model);
        ServiceResult PostForUser(ChatMessageModel model);
    }

    public class ChatMessageService : IChatMessageService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public ChatMessageService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<ChatMessageModel> Get(Guid code)
        {
            return _unitOfWork.Repository<ChatMessage>()
                .Where(x => x.Chat.Code == code, x => x.Include(o => o.Chat))
                .Select(x => new ChatMessageModel
                {
                    Code = x.Chat.Code,
                    NameSurname = x.UserId == null ? x.Chat.Name : (x.User.Name + " " + x.User.Surname),
                    Message = x.Message,
                    InsertDate = x.InsertedDate
                }).ToList();
        }

        public ServiceResult PostForClient(ChatMessageModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var chat = _unitOfWork.Repository<Chat>().FirstOrDefault(x => x.Code == model.Code);
            if (chat != null)
            {
                var chatMessage = new ChatMessage
                {
                    ChatId = chat.Id,
                    InsertedDate = DateTime.Now,
                    Message = model.Message
                };
                _unitOfWork.Repository<ChatMessage>().Add(chatMessage);
                _unitOfWork.Save();
            }
            return result;
        }

        public ServiceResult PostForUser(ChatMessageModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var chat = _unitOfWork.Repository<Chat>().FirstOrDefault(x => x.Code == model.Code);
            if (chat != null)
            {
                var chatMessage = new ChatMessage
                {
                    ChatId = chat.Id,
                    UserId = model.UserId,
                    InsertedDate = DateTime.Now,
                    Message = model.Message
                };
                _unitOfWork.Repository<ChatMessage>().Add(chatMessage);
                chat.Status = ChatStatus.Started;
                _unitOfWork.Save();
            }

            return result;
        }
    }
}
