using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Core.Controllers
{
    public class LoginController : Controller
    {
        [Route("giris")]
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (HttpContext.Request.QueryString.HasValue)
                {
                    var returnUrl = HttpContext.Request.Query["returnUrl"].ToString();
                    ViewBag.ReturnUrl = returnUrl;
                }
            }
            return View();
        }
    }
}