using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TaskDefinitionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
