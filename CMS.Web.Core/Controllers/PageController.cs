using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Core.Controllers
{
    public class PageController : Controller
    {
        [Route("sayfalar/{url}")]
        public IActionResult Index(string url)
        {
            return View();
        }
    }
}