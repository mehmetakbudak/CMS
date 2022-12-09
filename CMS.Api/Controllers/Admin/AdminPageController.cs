using CMS.Model.Entity;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Get(int id)
        {
            var page = await _pageService.GetById(id);
            return Ok(page);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Page model)
        {
            var result = await _pageService.Post(model);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Page model)
        {
            var result = await _pageService.Put(model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _pageService.Delete(id);
            return Ok(result);
        }
    }
}
