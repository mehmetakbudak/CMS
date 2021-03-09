using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Core.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error/{code:int}")]
        public IActionResult Index(int code)
        {
            ViewBag.StatusCode = code;
            return View();
        }
    }
}