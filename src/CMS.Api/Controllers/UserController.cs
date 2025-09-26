using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    [CMSAuthorize]
    public class UserController(IUserService userService) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = userService.Get();
            return Ok(result);
        }       

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await userService.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDto dto)
        {
            var response = await userService.Create(dto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserDto dto)
        {
            var response = await userService.Update(dto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await userService.Delete(id);
            return Ok(response);
        }
    }
}
