using CMS.Model.Helper;
using CMS.Model.Model;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CMS.Web.Core.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogCategoryController : ControllerBase
    {
        private readonly IBlogCategoryService blogCategoryService;
        public BlogCategoryController(IBlogCategoryService blogCategoryService)
        {
            this.blogCategoryService = blogCategoryService;
        }

        [HttpGet("GetBlogByCategoryUrl")]
        [ProducesResponseType(typeof(List<BlogCategoryModel>), 200)]
        public IActionResult GetBlogByCategoryUrl(string url, int page)
        {
            var result = blogCategoryService.GetBlogByCategoryUrl(url, page);
            return StatusCode(result.IntStatusCode, HttpHelper.Result(result));
        }
    }
}