using CMS.Storage.Model;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly ILookupService _lookupService;

        public LookupController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        [HttpGet("ContactCategories")]
        [ProducesResponseType(typeof(List<LookupModel>), 200)] //OK
        public IActionResult ContactCategories()
        {
            var list = _lookupService.ContactCategories();
            return Ok(list);
        }
    }
}
