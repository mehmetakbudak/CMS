using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService blogService;

        public BlogController(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        [HttpGet("GetByText")]
        public IActionResult GetByText(string? text)
        {
            var list = blogService.GetBlogList(text);
            return Ok(list);
        }

        [HttpGet("GetBlogByUrl")]
        public IActionResult GetBlogByUrl(string url)
        {
            var result = blogService.GetByUrl(url);
            return StatusCode(result.StatusCode, result);
        }
    }
}
