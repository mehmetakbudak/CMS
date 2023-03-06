using CMS.Service;
using CMS.Storage.Model;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        #region Views
        [Route("admin/blog")]
        public IActionResult Index() => View();

        [Route("admin/blog/add")]
        public IActionResult Add()
        {
            ViewBag.Id = 0;
            return View("CreateOrUpdate");
        }

        [Route("admin/blog/edit/{id}")]
        public IActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View("CreateOrUpdate");
        }
        #endregion

        #region APIs

        [HttpGet("api/admin/blog")]
        public IActionResult Get()
        {
            var list = _blogService.GetAll();
            return Ok(list);
        }

        [HttpGet("api/admin/blog/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _blogService.GetById(id);
            return Ok(result);
        }

        [HttpPost("api/admin/blog")]
        public async Task<IActionResult> Post(BlogPostModel model)
        {
            var result = await _blogService.Post(model);
            return Ok(result);
        }

        [HttpPut("api/admin/blog")]
        public async Task<IActionResult> Put(BlogPutModel model)
        {
            var result = await _blogService.Put(model);
            return Ok(result);
        }

        #endregion
    }
}
