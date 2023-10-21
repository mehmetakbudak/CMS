using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Web.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet("/tag/by-url/{url}")]
        public async Task<IActionResult> GetByUrl(string url)
        {
            var model = await _tagService.GetByUrl(url);
            return Ok(model);
        }
    }
}
