using CMS.Model.Model.ViewModel;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Controllers
{
    public class BlogController : Controller
    {
        #region Constructor
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        #endregion

        #region Views
        public IActionResult Index(string text)
        {
            ViewBag.SearchText = text;

            return View();
        }

        [Route("blog/{categoryUrl}")]
        public IActionResult BlogCategory(string categoryUrl)
        {
            ViewBag.CategoryUrl = categoryUrl;
            return View();
        }

        [Route("blog/{url}/{id}")]
        public IActionResult BlogDetail(string url, int id)
        {
            ViewBag.Url = url;
            ViewBag.Id = id;
            return View();
        }

        [Route("blog/tag/{name}")]
        public IActionResult BlogTag(string name)
        {
            ViewBag.Name = name;            
            return View();
        }
        #endregion

        #region APIs
        [HttpGet("api/blog")]
        public IActionResult Get(string text, int? top)
        {
            var list = _blogService.GetBlogs(text, top);
            return Ok(list);
        }

        [HttpGet("api/blog/{id}")]
        public IActionResult Get(int id)
        {
            var model = _blogService.GetDetailById(id);
            return Ok(model);
        }

        [HttpGet("api/blog/seen/{id}")]
        public IActionResult Seen(int id)
        {
            var model = _blogService.Seen(id);
            return Ok(model);
        }

        [HttpGet("api/blog/by-category/{blogCategoryUrl}")]
        public IActionResult GetBlogsByCategoryUrl(string blogCategoryUrl)
        {
            var list = _blogService.GetBlogsByCategoryUrl(blogCategoryUrl);
            return Ok(list);
        }

        [HttpGet("api/blog/most-read")]
        public IActionResult MostRead()
        {
            var list = _blogService.MostRead();
            return Ok(list);
        }

        [HttpGet("api/blog/most-read-by-category/{blogCategoryUrl}")]
        [ProducesResponseType(typeof(List<MostReadBlogViewModel>), 200)] //OK
        public IActionResult MostReadByBlogCategoryId(string blogCategoryUrl)
        {
            var list = _blogService.MostRead(blogCategoryUrl);
            return Ok(list);
        }
        #endregion
    }
}
