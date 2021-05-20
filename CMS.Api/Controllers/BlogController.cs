using CMS.Model.Helper;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService blogService;
        public BlogController(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        [HttpGet("GetBlogByUrl")]
        public IActionResult GetBlogByUrl(string url)
        {
            var result = blogService.GetByUrl(url);
            return StatusCode(result.StatusCode, result);
        }
    }
}
