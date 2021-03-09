using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Core.Controllers
{
    public class BlogController : Controller
    {
        [Route("blog/{category}")]
        public IActionResult BlogCategory(string category)
        {
            return View();
        }

        [Route("blog/{category}/{url}/{id:int}")]
        public IActionResult BlogDetail(string category, string url, int id)
        {
            return View();
        }
    }
}