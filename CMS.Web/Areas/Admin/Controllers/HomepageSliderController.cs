using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomepageSliderController : Controller
    {
        [CMSAuthorize(RouteLevel = 2, IsView = true)]
        [Route("admin/homepage-slider")]
        public IActionResult Index() => View();



    }
}
