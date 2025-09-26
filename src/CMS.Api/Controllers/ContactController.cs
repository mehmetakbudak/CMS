using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.Contact;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class ContactController(IContactService contactService) : BaseController
    {
        [HttpGet]  
        [CMSAuthorize]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = contactService.Get();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContactDto dto)
        {
            var result = await contactService.Create(dto);
            return Ok(result);
        }

        [CMSAuthorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await contactService.Delete(id);
            return Ok(result);
        }
    }
}
