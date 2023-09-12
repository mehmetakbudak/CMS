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
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("admin/user")]
        [CMSAuthorize(IsView = true)]
        public IActionResult Index() => View();

        [HttpGet("admin/user/list")]
        [CMSAuthorize(RouteLevel = 3)]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var list = await _userService.Get();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        [HttpGet("admin/user/{id}")]
        [CMSAuthorize(RouteLevel = 2)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _userService.GetById(id);
            return Ok(result);
        }

        [HttpPost("admin/user")]
        [CMSAuthorize(RouteLevel = 2)]
        public async Task<IActionResult> Post([FromBody] UserModel model)
        {
            var result = await _userService.Post(model);
            return Ok(result);
        }

        [HttpPut("admin/user")]
        [CMSAuthorize(RouteLevel = 2)]
        public async Task<IActionResult> Put([FromBody] UserModel model)
        {
            var result = await _userService.Put(model);
            return Ok(result);
        }

        [HttpDelete("admin/user/{id}")]
        [CMSAuthorize(RouteLevel = 2)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.Delete(id);
            return Ok(result);
        }
    }
}
