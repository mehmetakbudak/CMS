using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.Blog;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class BlogController(IBlogService blogService) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        [CMSAuthorize]
        public IActionResult Get()
        {
            var result = blogService.Get();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await blogService.GetAllActive();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [CMSAuthorize]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await blogService.GetById(id);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDetailById(int id)
        {
            var result = await blogService.GetDetailById(id);
            return Ok(result);
        }

        [HttpGet("{url}")]
        [EnableQueryWithMetadata]
        public async Task<IActionResult> GetBlogsByCategoryUrl(string url)
        {
            var result = await blogService.GetBlogsByCategoryUrl(url);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult List([FromBody] BlogListFilterDto dto)
        {
            var result = blogService.GetBlogs(dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> MostRead(string url)
        {
            var result = await blogService.MostRead(url);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Seen([FromBody] BlogSeenDto dto)
        {
            var result = await blogService.Seen(dto);
            return Ok(result);
        }

        [HttpPost]
        [CMSAuthorize]
        public async Task<IActionResult> Create([FromForm] BlogCreateOrUpdateDto dto)
        {
            var result = await blogService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        [CMSAuthorize]
        public async Task<IActionResult> Update([FromForm] BlogCreateOrUpdateDto dto)
        {
            var result = await blogService.Update(dto);
            return Ok(result);
        }

        [CMSAuthorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await blogService.Delete(id);
            return Ok(result);
        }
    }
}
