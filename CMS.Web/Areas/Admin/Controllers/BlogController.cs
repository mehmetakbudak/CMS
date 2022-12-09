using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            ViewBag.Id = 0;
            return View("CreateOrUpdate");
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View("CreateOrUpdate");
        }
    }
}
