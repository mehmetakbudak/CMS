using CMS.Service;
using CMS.Storage.Enum;
using CMS.Storage.Model;
using CMS.Storage.Model.ViewModel;
using CMS.Web.Models;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Web.Controllers
{
    public class BlogController : Controller
    {
        #region Constructor
        private readonly IBlogService _blogService;
        private readonly ICommentService _commentService;
        private readonly ITagService _tagService;
        public BlogController(
            IBlogService blogService,
            ICommentService commentService,
            ITagService tagService)
        {
            _blogService = blogService;
            _commentService = commentService;
            _tagService = tagService;
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

        [Route("blog/tag/{url}")]
        public IActionResult BlogTag(string url)
        {
            ViewBag.Url = url;
            return View();
        }

        [HttpGet("blog/list")]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var list = await _blogService.GetBlogs();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        [HttpGet("blog/detail/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var model = await _blogService.GetDetailById(id);
            return Ok(model);
        }

        [HttpGet("blog/seen/{id}")]
        public async Task<IActionResult> Seen(int id)
        {
            var model = await _blogService.Seen(id);
            return Ok(model);
        }

        [HttpGet("blog/by-category")]
        public async Task<IActionResult> GetBlogsByCategoryUrl(DataSourceLoadOptions loadOptions, string url)
        {
            var list = await _blogService.GetBlogsByCategoryUrl(url);
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        [HttpGet("blog/most-read")]
        public async Task<IActionResult> MostRead()
        {
            var list = await _blogService.MostRead();
            return Ok(list);
        }

        [HttpGet("blog/most-read-by-category/{blogCategoryUrl}")]
        [ProducesResponseType(typeof(List<MostReadBlogViewModel>), 200)] //OK
        public async Task<IActionResult> MostReadByBlogCategoryId(string blogCategoryUrl)
        {
            var list = await _blogService.MostRead(blogCategoryUrl);
            return Ok(list);
        }

        [HttpGet("blog/tag/list")]
        [ProducesResponseType(typeof(List<BlogTagCountModel>), 200)] //OK
        public async Task<IActionResult> GetTags()
        {
            var list = await _tagService.GetSourceTags(SourceType.Blog, 10);
            return Ok(list);
        }

        [HttpGet("blog/list-by-tag-url")]
        public async Task<IActionResult> GetTagBlogsByUrl(DataSourceLoadOptions loadOptions, string url)
        {
            var list = await _blogService.GetTagBlogsByUrl(url);
            return Json(DataSourceLoader.Load(list, loadOptions));
        }
    }
}
