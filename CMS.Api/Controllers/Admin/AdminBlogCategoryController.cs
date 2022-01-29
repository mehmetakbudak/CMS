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
        private readonly IBlogCategoryService blogCategoryService;

        public AdminBlogCategoryController(IBlogCategoryService blogCategoryService)
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
