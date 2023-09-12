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
    public class BlogCategoryController : Controller
    {
        private readonly IBlogCategoryService _blogCategoryService;

        public BlogCategoryController(IBlogCategoryService blogCategoryService)
        {
            _blogCategoryService = blogCategoryService;
        }

        [CMSAuthorize(IsView = true, RouteLevel = 2)]
        public IActionResult Index() => View();

        [CMSAuthorize(RouteLevel = 3)]
        [HttpGet("admin/blog-category/list")]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var list = await _blogCategoryService.GetAll();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpGet("admin/blog-category/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _blogCategoryService.GetById(id);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPost("admin/blog-category")]
        public async Task<IActionResult> Post([FromBody] BlogCategoryModel model)
        {
            var result = await _blogCategoryService.Post(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPut("admin/blog-category")]
        public async Task<IActionResult> Put([FromBody] BlogCategoryModel model)
        {
            var result = await _blogCategoryService.Put(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpDelete("admin/blog-category/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _blogCategoryService.Delete(id);
            return Ok(result);
        }
    }
}
