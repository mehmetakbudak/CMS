using CMS.Model.Entity;
using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Post([FromBody] BlogCategoryDtoModel model)
        {
            var result = _blogCategoryService.Post(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public IActionResult Put([FromBody] BlogCategoryDtoModel model)
        {
            var result = _blogCategoryService.Put(model);
            return StatusCode(result.StatusCode, result);
        }
    }
}
