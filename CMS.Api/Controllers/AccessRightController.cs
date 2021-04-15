using CMS.Model.Enum;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessRightController : ControllerBase
    {
        private readonly IAccessRightService accessRightService;
        public AccessRightController(IAccessRightService accessRightService)
        {
            this.accessRightService = accessRightService;
        }

        [HttpGet("api")]
        public IActionResult GetApiAccessRights()
        {
            var accessRights = accessRightService.GetAll(AccessRightCategoryType.Api).ToList();
            return Ok(accessRights);
        }

        [HttpGet("backend")]
        public IActionResult GetBackEndAccessRights()
        {
            var accessRights = accessRightService.GetAll(AccessRightCategoryType.Admin).ToList();
            return Ok(accessRights);
        }
    }
}