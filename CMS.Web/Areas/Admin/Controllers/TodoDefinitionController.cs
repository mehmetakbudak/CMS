using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TodoDefinitionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
