using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.Role;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    [CMSAuthorize]
    public class RoleController(IRoleService roleService) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = roleService.Get();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await roleService.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleDto dto)
        {
            var response = await roleService.Create(dto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RoleDto dto)
        {
            var response = await roleService.Update(dto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await roleService.Delete(id);
            return Ok(response);
        }
    }
}
