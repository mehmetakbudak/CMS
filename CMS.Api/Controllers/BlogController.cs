using CMS.Model.Helper;
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

        [HttpGet]
        public IActionResult Get()
        {
            var list = blogService.GetAll()
                .Where(x => x.IsActive && x.Published)
                .OrderBy(x => x.DisplayOrder).ToList();
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
