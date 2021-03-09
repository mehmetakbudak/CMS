using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Core.Controllers
{
    public class AuthorController : Controller
    {
        [Route("yazarlar")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("yazarlar/{author}")]
        public IActionResult AuthorDetails(string author)
        {
            return View();
        }

        [Route("yazarlar/{author}/{url}/{id:int}")]
        public IActionResult ArticleDetails(string author, string url, int id)
        {
            return View();
        }
    }
}