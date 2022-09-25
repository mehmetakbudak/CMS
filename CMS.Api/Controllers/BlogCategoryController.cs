using CMS.Model.Model;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        public IActionResult Get()
        {
            var list = blogCategoryService.GetAllActive();
            return Ok(list);
        }
    }
}