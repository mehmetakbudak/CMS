using CMS.Model.Entity;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers.Admin
{
    [CMSAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminPageController : ControllerBase
    {
        private readonly IPageService _pageService;

        public AdminPageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = _pageService.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var page = _pageService.GetById(id);
            return Ok(page);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Page model)
        {
            var result = _pageService.Post(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Page model)
        {
            var result = _pageService.Put(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _pageService.Delete(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
