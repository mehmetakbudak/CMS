using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.BlogCategory;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class BlogCategoryController(IBlogCategoryService blogCategoryService) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = blogCategoryService.Get();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWithCount()
        {
            var result = await blogCategoryService.GetAllWithCount();
            return Ok(result);
        }

        [HttpGet("{url}")]
        public async Task<IActionResult> GetByUrl(string url)
        {
            var result = await blogCategoryService.GetByUrl(url);
            return Ok(result);
        }

        [CMSAuthorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await blogCategoryService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        [CMSAuthorize]
        public async Task<IActionResult> Create(BlogCategoryDto dto)
        {
            var result = await blogCategoryService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        [CMSAuthorize]
        public async Task<IActionResult> Update(BlogCategoryDto dto)
        {
            var result = await blogCategoryService.Update(dto);
            return Ok(result);
        }

        [CMSAuthorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await blogCategoryService.Delete(id);
            return Ok(result);
        }
    }
}
