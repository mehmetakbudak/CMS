using CMS.Storage.Entity;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetByUrl(string url)
        {
            var result = await _pageService.GetByUrl(url);
            return Ok(result);
        }        
    }
}