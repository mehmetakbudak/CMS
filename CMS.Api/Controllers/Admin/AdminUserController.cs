using CMS.Model.Enum;
using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using CMS.Service.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CMS.Api.Admin
{
    [CMSAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminUserController : ControllerBase
    {
        private readonly IUserService userService;

        public AdminUserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] PaginationFilterModel filter)
        {
            var list = userService.GetAll();
            var pagedReponse = PaginationHelper.CreatePagedReponse<UserModel>(list, filter);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await userService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserModel model)
        {
            var result = await userService.Post(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserModel model)
        {
            var result = await userService.Put(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await userService.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}