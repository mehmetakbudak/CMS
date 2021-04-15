using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api
{
    [CMSApiAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = userService.GetAll();
            return Ok(list);
        }

        [HttpPost("CreateOrUpdate")]
        public IActionResult CreateOrUpdate([FromBody] UserModel model)
        {
            var result = userService.CreateOrUpdate(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = userService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}