using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.Tag;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class TagController(ITagService tagService) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = tagService.Get();
            return Ok(result);
        }   

        [HttpPost]
        public async Task<IActionResult> GetSourceTags(GetSourceTagDto dto)
        {
            var result = await tagService.GetSourceTags(dto);
            return Ok(result);
        }
    }
}
