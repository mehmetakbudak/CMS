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
        private readonly IUnitOfWork<CMSContext> unitOfWork;
        public ChatMessageService(IUnitOfWork<CMSContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<ChatMessageModel> Get(Guid guid)
        {
            return unitOfWork.Repository<ChatMessage>()
                .GetAll(x => x.Chat.ChatGuid == guid, x => x.Include(o => o.Chat))
                .Select(x => new ChatMessageModel
                {
                    ChatGuid = x.Chat.ChatGuid,
                    NameSurname = x.UserId == null ? x.Chat.NameSurname : (x.User.Name + " " + x.User.Surname),
                    Message = x.Message,
                    InsertDate = x.InsertDate
                }).ToList();
        }

        public ServiceResult PostForClient(ChatMessageModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            try
            {
                var chat = unitOfWork.Repository<Chat>().Find(x => x.ChatGuid == model.ChatGuid);
                if (chat != null)
                {
                    var chatMessage = new ChatMessage
                    {
                        ChatId = chat.Id,
                        InsertDate = DateTime.Now,
                        Message = model.Message
                    };
                    unitOfWork.Repository<ChatMessage>().Add(chatMessage);
                    unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = (int)HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public ServiceResult PostForUser(ChatMessageModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            try
            {
                var chat = unitOfWork.Repository<Chat>().Find(x => x.ChatGuid == model.ChatGuid);
                if (chat != null)
                {
                    var chatMessage = new ChatMessage
                    {
                        ChatId = chat.Id,
                        UserId = model.UserId,
                        InsertDate = DateTime.Now,
                        Message = model.Message
                    };
                    unitOfWork.Repository<ChatMessage>().Add(chatMessage);
                    chat.ChatStatus = ChatStatus.Started;
                    unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = (int)HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
