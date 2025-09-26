using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.AccessRight;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    [CMSAuthorize]
    public class AccessRightController(
        IAccessRightService accessRightService) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = accessRightService.Get();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAccessRightsWithCategory(int? roleId)
        {
            var result = await accessRightService.GetAccessRightsWithCategory(roleId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await accessRightService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccessRightDto dto)
        {
            var result = await accessRightService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(AccessRightDto dto)
        {
            var result = await accessRightService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await accessRightService.Delete(id);
            return Ok(result);
        }
    }
}
