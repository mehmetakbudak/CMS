using CMS.Service;
using CMS.Service.Attributes;
using CMS.Storage.Model;
using CMS.Web.Models;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [CMSAuthorize(RouteLevel = 2)]
        public IActionResult Index() => View();        

        [HttpGet("/admin/role/list")]
        [CMSAuthorize(RouteLevel = 3)]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var list = await _roleService.Get();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        [HttpGet("admin/role/{id}")]
        [CMSAuthorize(RouteLevel = 2)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _roleService.GetById(id);
            return Ok(result);
        }

        [HttpPost("admin/role")]
        [CMSAuthorize(RouteLevel = 2)]
        public async Task<IActionResult> Post([FromBody] RoleModel model)
        {
            var result = await _roleService.Post(model);
            return Ok(result);
        }

        [HttpPut("admin/role")]
        [CMSAuthorize(RouteLevel = 2)]
        public async Task<IActionResult> Put([FromBody] RoleModel model)
        {
            var result = await _roleService.Put(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpDelete("admin/role/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roleService.Delete(id);
            return Ok(result);
        }
    }
}
