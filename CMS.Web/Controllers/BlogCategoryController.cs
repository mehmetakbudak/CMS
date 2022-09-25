using CMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Controllers
{
    public class BlogCategoryController : Controller
    {
        #region Constructor
        private readonly IBlogCategoryService _blogCategoryService;

        public BlogCategoryController(IBlogCategoryService blogCategoryService)
        {
            _blogCategoryService = blogCategoryService;
        }
        #endregion

        #region APIs
        [HttpGet("api/blog-category")]
        public IActionResult Get()
        {
            var list = _blogCategoryService.GetAllActive();
            return Ok(list);
        }

        [HttpGet("api/blog-category/{url}")]
        public IActionResult GetByUrl(string url)
        {
            var result = _blogCategoryService.GetByUrl(url);
            return Ok(result);
        }
        #endregion
    }
}
