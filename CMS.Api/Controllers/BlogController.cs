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
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var model = _blogService.GetDetailById(id);
            return Ok(model);
        }

        [HttpGet("GetByText")]
        public IActionResult GetByText(string categoryUrl, string? text)
        {
            var list = _blogService.GetBlogList(categoryUrl, text);
            return Ok(list);
        }


        [HttpPut("Seen/{id}")]
        public IActionResult Seen(int id)
        {
            var result = _blogService.Seen(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("MostRead")]
        public IActionResult MostRead()
        {
            var list = _blogService.MostRead();
            return Ok(list);
        }

        [HttpGet("MostReadByBlogCategoryId/{blogCategoryId}")]
        public IActionResult MostReadByBlogCategoryId(int blogCategoryId)
        {
            var list = _blogService.MostRead(blogCategoryId);
            return Ok(list);
        }
    }
}
