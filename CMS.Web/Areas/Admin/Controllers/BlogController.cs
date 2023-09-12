using CMS.Service;
using CMS.Service.Attributes;
using CMS.Storage.Model;
using CMS.Web.Models;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [Route("admin/blog")]
        [CMSAuthorize(RouteLevel = 2, IsView = true)]
        public IActionResult Index() => View();

        [CMSAuthorize(RouteLevel = 3)]
        [HttpGet("admin/blog/list")]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var list = await _blogService.GetAll();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpGet("admin/blog/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _blogService.GetById(id);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPost("admin/blog")]
        public async Task<IActionResult> Post(BlogPostModel model)
        {
            var result = await _blogService.Post(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPut("admin/blog")]
        public async Task<IActionResult> Put(BlogPutModel model)
        {
            var result = await _blogService.Put(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpDelete("admin/blog/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _blogService.Delete(id);
            return Ok(result);
        }
    }
}
