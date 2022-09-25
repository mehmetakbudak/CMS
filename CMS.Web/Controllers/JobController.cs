using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Controllers
{
    public class JobController : Controller
    {
        #region Constructor
        public JobController()
        {

        }
        #endregion

        #region Views
        public IActionResult Index()
        {
            return View();
        }
        #endregion
    }
}
