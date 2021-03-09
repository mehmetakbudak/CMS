using CMS.Model.Helper;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Core.Api
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

        [Route("GetBlogByUrl")]
        public IActionResult GetBlogByUrl(string url)
        {
            var result = blogService.GetByUrl(url);
            return StatusCode(result.IntStatusCode, HttpHelper.Result(result));
        }
    }
}
