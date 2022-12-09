using CMS.Storage.Model;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService chatService;

        public ChatController(IChatService chatService)
        {
            this.chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChatModel model)
        {
            var result = await chatService.Post(model);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}