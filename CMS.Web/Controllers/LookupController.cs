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

        [HttpGet("ContactCategory")]
        public async Task<IActionResult> ContactCategory()
        {
            var result = await _lookupService.ContactCategories();
            return Ok(result);
        }

        [HttpGet("WorkTypes")]
        public IActionResult WorkTypes()
        {
            var result = _lookupService.WorkTypes();
            return Ok(result);
        }

        [HttpGet("JobLocations")]
        public async Task<IActionResult> GetJobLocations()
        {
            var result = await _lookupService.JobLocations();
            return Ok(result);
        }
    }
}
