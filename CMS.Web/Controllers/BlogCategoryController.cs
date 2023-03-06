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
        public async Task<IActionResult> Get()
        {
            var list = await _blogCategoryService.GetAllActive();
            return Ok(list);
        }

        [HttpGet("api/blog-category/{url}")]
        public async Task<IActionResult> GetByUrl(string url)
        {
            var result = await _blogCategoryService.GetByUrl(url);
            return Ok(result);
        }
        #endregion
    }
}
