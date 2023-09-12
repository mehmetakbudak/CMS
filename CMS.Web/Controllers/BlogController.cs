using CMS.Storage.Model.ViewModel;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using CMS.Storage.Model;
using CMS.Storage.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMS.Web.Models;
using DevExtreme.AspNet.Data;
using System.Xml.Linq;
using System.Linq;

namespace CMS.Web.Controllers
{
    public class BlogController : Controller
    {
        #region Constructor
        private readonly IBlogService _blogService;
        private readonly ICommentService _commentService;
        public BlogController(
            IBlogService blogService,
            ICommentService commentService)
        {
            _blogService = blogService;
            _commentService = commentService;
        }
        #endregion

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
        public async Task<IActionResult> BlogDetail(string url, int id)
        {
            ViewBag.Url = url;
            ViewBag.Id = id;

            var model = await _commentService.GetSourceComments(new SourceCommentModel
            {
                SourceId = id,
                SourceType = SourceType.Blog
            });

            return View(model);
        }

        [Route("blog/tag/{name}")]
        public IActionResult BlogTag(string name)
        {
            ViewBag.Name = name;
            return View();
        }


        [HttpGet("blog/list")]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var list = await _blogService.GetBlogs();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        #region APIs
        [HttpGet("api/blog")]
        public async Task<IActionResult> Get(string text, int? top)
        {
            var list = await _blogService.GetBlogs();
            return Ok(list);
        }

        [HttpGet("api/blog/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var model = await _blogService.GetDetailById(id);
            return Ok(model);
        }

        [HttpGet("api/blog/seen/{id}")]
        public async Task<IActionResult> Seen(int id)
        {
            var model = await _blogService.Seen(id);
            return Ok(model);
        }

        [HttpGet("api/blog/by-category/{blogCategoryUrl}")]
        public async Task<IActionResult> GetBlogsByCategoryUrl(string blogCategoryUrl)
        {
            var list = await _blogService.GetBlogsByCategoryUrl(blogCategoryUrl);
            return Ok(list);
        }

        [HttpGet("api/blog/most-read")]
        public async Task<IActionResult> MostRead()
        {
            var list = await _blogService.MostRead();
            return Ok(list);
        }

        [HttpGet("api/blog/most-read-by-category/{blogCategoryUrl}")]
        [ProducesResponseType(typeof(List<MostReadBlogViewModel>), 200)] //OK
        public async Task<IActionResult> MostReadByBlogCategoryId(string blogCategoryUrl)
        {
            var list = await _blogService.MostRead(blogCategoryUrl);
            return Ok(list);
        }
        #endregion
    }
}
