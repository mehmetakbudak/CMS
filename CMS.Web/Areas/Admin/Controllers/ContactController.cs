using CMS.Service;
using CMS.Service.Attributes;
using CMS.Web.Models;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
        [CMSAuthorize(RouteLevel = 2, IsView = true)]
        public IActionResult Index() => View();


        [HttpGet("admin/contact/list")]
        [CMSAuthorize(RouteLevel = 3)]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var list = await _contactService.Get().ToListAsync();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpDelete("admin/contact/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _contactService.Delete(id);
            return Ok(result);
        }
    }
}
