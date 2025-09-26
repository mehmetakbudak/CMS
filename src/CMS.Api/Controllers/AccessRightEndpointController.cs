using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.AccessRightEndpoint;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    [CMSAuthorize]
    public class AccessRightEndpointController(
        IAccessRightEndpointService accessRightEndpointService) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = accessRightEndpointService.Get();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await accessRightEndpointService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccessRightEndpointDto dto)
        {
            var result = await accessRightEndpointService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(AccessRightEndpointDto dto)
        {
            var result = await accessRightEndpointService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await accessRightEndpointService.Delete(id);
            return Ok(result);
        }
    }
}
