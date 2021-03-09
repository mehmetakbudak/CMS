using CMS.Model.Model;
using CMS.Service;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CMS.Web.Core.Hubs
{
    public class ChatHub : Hub
    {
        private IHubContext<ChatHub> hub;
        private readonly IChatMessageService chatMessageService;
        public ChatHub(IChatMessageService chatMessageService, IHubContext<ChatHub> hub)
        {
            this.chatMessageService = chatMessageService;
            this.hub = hub;
        }

        public async Task SendMessage(ChatMessageModel model)
        {
            chatMessageService.PostForClient(model);
            var list = chatMessageService.Get(model.ChatGuid);
            await hub.Clients.All.SendAsync("ReceiveMessage", list);
        }
    }
}
