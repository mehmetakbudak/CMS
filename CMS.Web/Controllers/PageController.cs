using CMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Controllers
{
    public class PageController : Controller
    {
        #region Constructor
        private readonly IPageService _pageService;

        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }
        #endregion

        #region Views
        [Route("page/{url}")]
        public IActionResult Index(string url)
        {
            var result = _pageService.GetByUrl(url);
            return View(result);
        }
        #endregion
    }
}
