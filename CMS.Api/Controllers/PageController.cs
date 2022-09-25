using CMS.Model.Entity;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageController : ControllerBase
    {
        private readonly IPageService _pageService;

        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }       

        /// <summary>
        /// Url bilgisine göre sayfa bilgilerini döner.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet("GetByUrl/{url}")]
        [ProducesResponseType(typeof(Page), 200)] //OK
        public IActionResult GetByUrl(string url)
        {
            var result = _pageService.GetByUrl(url);
            return Ok(result);
        }        
    }
}