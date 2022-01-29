using CMS.Model.Model;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService contactService;
        public ContactController(IContactService contactService)
        {
            this.contactService = contactService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] ContactModel model)
        {
            var result = contactService.Post(model);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}