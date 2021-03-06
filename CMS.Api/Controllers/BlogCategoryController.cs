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
        public IActionResult Get()
        {
            var list = blogCategoryService.GetAll();
            return Ok(list);
        }        
    }
}