using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [CMSAuthorize]
        public IActionResult Get()
        {
            var list = userService.GetAll();
            return Ok(list);
        }

        [HttpPost]
        [CMSAuthorize]
        public IActionResult Post([FromBody] UserModel model)
        {
            var result = userService.Post(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        [CMSAuthorize]
        public IActionResult Put([FromBody] UserModel model)
        {
            var result = userService.Put(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [CMSAuthorize]
        public IActionResult Delete(int id)
        {
            var result = userService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }       
    }
}