using CMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Controllers
{
    public class Lookup : Controller
    {
        private readonly ILookupService _lookupService;

        public Lookup(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        [Route("api/Lookup/ContactCategory")]
        public IActionResult ContactCategory()
        {
            var result = _lookupService.ContactCategories();
            return Ok(result);
        }
    }
}
