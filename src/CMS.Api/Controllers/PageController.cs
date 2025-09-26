using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.Page;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class PageController(IPageService pageService) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = pageService.Get();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await pageService.GetById(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByUrl(string url)
        {
            var result = await pageService.GetByUrl(url);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PageDto dto)
        {
            var result = await pageService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PageDto dto)
        {
            var result = await pageService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await pageService.Delete(id);
            return Ok(result);
        }
    }
}
