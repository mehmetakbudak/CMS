using CMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Controllers
{
    public class LookupController : Controller
    {
        private readonly ILookupService _lookupService;

        public LookupController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        [Route("api/Lookup/ContactCategory")]
        public async Task<IActionResult> ContactCategory()
        {
            var result = await _lookupService.ContactCategories();
            return Ok(result);
        }
    }
}
