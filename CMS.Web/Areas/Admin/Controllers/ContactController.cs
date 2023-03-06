using CMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [Route("admin/contact")]
        public IActionResult Index() => View();


        [HttpGet("api/admin/contact")]
        public IActionResult Get()
        {
            var list = _contactService.Get();
            return Ok(list);
        }
    }
}
