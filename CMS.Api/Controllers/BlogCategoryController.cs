using CMS.Storage.Model;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogCategoryController : ControllerBase
    {
        private readonly IBlogCategoryService blogCategoryService;

        public BlogCategoryController(IBlogCategoryService blogCategoryService)
        {
            this.blogCategoryService = blogCategoryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<BlogCategoryWithCountModel>), 200)]
        public async Task<IActionResult> Get()
        {
            var list = await blogCategoryService.GetAllActive();
            return Ok(list);
        }
    }
}