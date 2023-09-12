using CMS.Service;
using CMS.Service.Attributes;
using CMS.Storage.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccessRightEndpointController : Controller
    {        
        private readonly IAccessRightEndpointService _accessRightEndpointService;
        
        public AccessRightEndpointController(IAccessRightEndpointService accessRightEndpointService)
        {
            _accessRightEndpointService = accessRightEndpointService;
        }

        [CMSAuthorize(RouteLevel = 3)]
        [HttpGet("admin/accessrightendpoint/getbyaccessrightid/{accessRightId}")]
        public async Task<IActionResult> GetByAccessRightId(int accessRightId)
        {
            var result = await _accessRightEndpointService.GetByAccessRightId(accessRightId);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 3)]
        [HttpGet("admin/accessrightendpoint/getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _accessRightEndpointService.GetById(id);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPost("admin/accessrightendpoint")]
        public async Task<IActionResult> Post([FromBody] AccessRightEndpoint model)
        {
            var result = await _accessRightEndpointService.Post(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPut("admin/accessrightendpoint")]
        public async Task<IActionResult> Put([FromBody]AccessRightEndpoint model)
        {
            var result = await _accessRightEndpointService.Put(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpDelete("admin/accessrightendpoint/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _accessRightEndpointService.Delete(id);
            return Ok(result);
        }
    }
}
