using CMS.Model.Model;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

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
        public IActionResult Post([FromBody] ChatModel model)
        {
            var result = chatService.Post(model);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}