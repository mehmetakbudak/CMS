using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Web.Controllers
{
    [Route("[controller]")]
    public class LookupController : Controller
    {
        private readonly ILookupService _lookupService;

        public LookupController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        [HttpGet("contact-category")]
        public async Task<IActionResult> ContactCategory()
        {
            var result = await _lookupService.ContactCategories();
            return Ok(result);
        }

        [HttpGet("work-types")]
        public IActionResult WorkTypes()
        {
            var result = _lookupService.WorkTypes();
            return Ok(result);
        }

        [HttpGet("job-locations")]
        public async Task<IActionResult> GetJobLocations()
        {
            var result = await _lookupService.JobLocations();
            return Ok(result);
        }

        [HttpGet("blog-categories")]
        public async Task<IActionResult> GetBlogCategories()
        {
            var result = await _lookupService.BlogCategories();
            return Ok(result);
        }
    }
}
