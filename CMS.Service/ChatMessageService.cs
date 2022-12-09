using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using CMS.Storage.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IChatMessageService
    {
        Task<List<ChatMessageModel>> Get(Guid guid);
        Task<ServiceResult> PostForClient(ChatMessageModel model);
        Task<ServiceResult> PostForUser(ChatMessageModel model);
    }

    public class ChatMessageService : IChatMessageService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public ChatMessageService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ChatMessageModel>> Get(Guid code)
        {
            return await _unitOfWork.Repository<ChatMessage>()
                .Where(x => x.Chat.Code == code)
                .Include(x => x.Chat)
                .Select(x => new ChatMessageModel
                {
                    Code = x.Chat.Code,
                    NameSurname = x.UserId == null ? x.Chat.Name : (x.User.Name + " " + x.User.Surname),
                    Message = x.Message,
                    InsertDate = x.InsertedDate
                }).ToListAsync();
        }

        public async Task<ServiceResult> PostForClient(ChatMessageModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var chat = await _unitOfWork.Repository<Chat>()
                .FirstOrDefault(x => x.Code == model.Code);

            if (chat != null)
            {
                var chatMessage = new ChatMessage
                {
                    ChatId = chat.Id,
                    InsertedDate = DateTime.Now,
                    Message = model.Message
                };

                await _unitOfWork.Repository<ChatMessage>().Add(chatMessage);

                await _unitOfWork.Save();
            }

            return result;
        }

        public async Task<ServiceResult> PostForUser(ChatMessageModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var chat = await _unitOfWork.Repository<Chat>()
                .FirstOrDefault(x => x.Code == model.Code);

            if (chat != null)
            {
                var chatMessage = new ChatMessage
                {
                    ChatId = chat.Id,
                    UserId = model.UserId,
                    InsertedDate = DateTime.Now,
                    Message = model.Message
                };
                await _unitOfWork.Repository<ChatMessage>().Add(chatMessage);

                chat.Status = ChatStatus.Started;

                await _unitOfWork.Save();
            }

            return result;
        }
    }
}
