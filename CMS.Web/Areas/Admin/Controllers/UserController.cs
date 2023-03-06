using CMS.Service;
using CMS.Storage.Model;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index() => View();
        
        [HttpPost("api/admin/user")]
        public IActionResult Get([FromBody] UserFilterModel model)
        {
            var result = _userService.GetByFilter(model);
            return Ok(result);
        }
    }
}
