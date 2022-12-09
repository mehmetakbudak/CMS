using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Api.Controllers.Admin
{
    [ApiController]
    [CMSAuthorize]
    [Route("api/[controller]")]
    public class AdminBlogCategoryController : ControllerBase
    {
        private readonly IBlogCategoryService _blogCategoryService;

        public AdminBlogCategoryController(IBlogCategoryService blogCategoryService)
        {
            _blogCategoryService = blogCategoryService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = _blogCategoryService.GetAll();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BlogCategoryModel model)
        {
            var result = await _blogCategoryService.Post(model);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] BlogCategoryModel model)
        {
            var result = await _blogCategoryService.Put(model);
            return Ok(result);
        }
    }
}
