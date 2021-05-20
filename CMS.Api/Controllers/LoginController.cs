using CMS.Model.Helper;
using CMS.Model.Model;
using CMS.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CMS.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IUserService userService;
        public LoginController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoginModel login)
        {
            var result = userService.Authenticate(login);
            return StatusCode(result.StatusCode, result);
        }
    }
}