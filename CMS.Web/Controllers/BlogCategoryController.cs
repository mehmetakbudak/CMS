using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        
        [HttpGet("blog-category/list")]
        public async Task<IActionResult> Get()
        {
            var list = await _blogCategoryService.GetAllActive();
            return Ok(list);
        }

        [HttpGet("blog-category/{url}")]
        public async Task<IActionResult> GetByUrl(string url)
        {
            var result = await _blogCategoryService.GetByUrl(url);
            return Ok(result);
        }
    }
}
