using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        [Route("admin/dashboard")]
        [CMSAuthorize(IsView = true, RouteLevel = 2)]
        public IActionResult Index() => View();
    }
}
