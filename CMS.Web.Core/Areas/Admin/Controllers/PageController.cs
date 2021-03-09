using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Web.Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class PageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Create")]
        public IActionResult CreateOrUpdate()
        {
            ViewBag.Id = null;
            return View("CreateOrUpdate");
        }

        [Route("Update/{id}")]
        public IActionResult CreateOrUpdate(int id)
        {
            ViewBag.Id = id;
            return View("CreateOrUpdate");
        }
    }
}
