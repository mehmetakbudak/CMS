using CMS.Service;
using CMS.Service.Attributes;
using CMS.Storage.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccessRightController : Controller
    {
        private readonly IAccessRightService _accessRightService;

        public AccessRightController(IAccessRightService accessRightService)
        {
            _accessRightService = accessRightService;
        }

        [Route("admin/access-right")]
        [CMSAuthorize(IsView = true, RouteLevel = 2)]
        public IActionResult Index() => View();

        [CMSAuthorize(RouteLevel = 3)]
        [HttpGet("admin/access-right/list")]
        public async Task<IActionResult> Get()
        {
            var list = await _accessRightService.Get();
            return Ok(list);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpGet("admin/access-right/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var list = await _accessRightService.GetById(id);
            return Ok(list);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPost("admin/access-right")]
        public async Task<IActionResult> Post([FromBody] AccessRightModel model)
        {
            var list = await _accessRightService.Post(model);
            return Ok(list);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPut("admin/access-right")]
        public async Task<IActionResult> Put([FromBody] AccessRightModel model)
        {
            var list = await _accessRightService.Put(model);
            return Ok(list);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpDelete("admin/access-right/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var list = await _accessRightService.Delete(id);
            return Ok(list);
        }
    }
}
