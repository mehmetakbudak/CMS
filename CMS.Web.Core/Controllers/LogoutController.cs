using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Core.Controllers
{
    public class LogoutController : Controller
    {
        [Route("cikis")]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}