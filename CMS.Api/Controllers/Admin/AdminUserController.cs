using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Api.Admin
{
    [CMSAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminUserController : ControllerBase
    {
        private IUserService userService;

        public AdminUserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = userService.GetAll();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserModel model)
        {
            var result = await userService.Post(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public IActionResult Put([FromBody] UserModel model)
        {
            var result = userService.Put(model);
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