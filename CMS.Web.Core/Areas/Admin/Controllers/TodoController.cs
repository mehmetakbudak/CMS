using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class TodoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("UserAssign")]
        public IActionResult UserAssign()
        {
            return View();
        }
    }
}
