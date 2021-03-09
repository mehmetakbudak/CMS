using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Core.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Id = null;
            return View();
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View("Create");
        }
    }
}