using CMS.Business.Exceptions;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.Chat;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IChatMessageService
    {
        Task<List<ChatMessageDto>> Get(Guid guid);
        Task<ServiceResult> PostForClient(ChatMessageDto model);
        Task<ServiceResult> PostForUser(ChatMessageDto model);
    }

    public class ChatMessageService(IUnitOfWork<CMSContext> unitOfWork) : IChatMessageService
    {
        public async Task<List<ChatMessageDto>> Get(Guid code)
        {
            return await unitOfWork.Repository<ChatMessage>()
                .Where(x => x.Chat.Code == code)
                .Include(x => x.Chat)
                .Select(x => new ChatMessageDto
                {
                    Code = x.Chat.Code,
                    NameSurname = x.UserId == null ? x.Chat.Name : x.User.Name + " " + x.User.Surname,
                    Message = x.Message,
                    InsertDate = x.InsertedDate
                }).ToListAsync();
        }

        public async Task<ServiceResult> PostForClient(ChatMessageDto model)
        {
            var chat = await unitOfWork.Repository<Chat>()
                .FirstOrDefault(x => x.Code == model.Code);

            if (chat != null)
            {
                var chatMessage = new ChatMessage
                {
                    ChatId = chat.Id,
                    InsertedDate = DateTime.Now,
                    Message = model.Message
                };

                await unitOfWork.Repository<ChatMessage>().Add(chatMessage);

                await unitOfWork.Save();
            }
            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> PostForUser(ChatMessageDto model)
        {
            var chat = await unitOfWork.Repository<Chat>()
                .FirstOrDefault(x => x.Code == model.Code);

            if (chat is null)
                throw new NotFoundException("Chat.Notfound");

            var chatMessage = new ChatMessage
            {
                ChatId = chat.Id,
                UserId = model.UserId,
                InsertedDate = DateTime.Now,
                Message = model.Message
            };
            await unitOfWork.Repository<ChatMessage>().Add(chatMessage);

            chat.Status = ChatStatus.Started;

            await unitOfWork.Save();


            return ServiceResult.Success(200);
        }
    }
}
