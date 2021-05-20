using CMS.Model.Entity;
using CMS.Model.Helper;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageController : ControllerBase
    {
        private readonly IPageService pageService;
        public PageController(IPageService pageService)
        {
            this.pageService = pageService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = pageService.GetAll().ToList();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var page = pageService.GetById(id);
            return Ok(page);
        }

        [HttpGet("GetByUrl")]
        public IActionResult GetByUrl(string url)
        {
            var result = pageService.GetByUrl(url);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("CreateOrUpdate")]
        public IActionResult CreateOrUpdate([FromBody] Page model)
        {
            var result = pageService.CreateOrUpdate(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = pageService.Delete(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}