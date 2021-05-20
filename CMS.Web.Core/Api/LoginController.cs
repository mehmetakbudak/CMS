using CMS.Model.Helper;
using CMS.Model.Model;
using CMS.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CMS.Web.Core.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginService loginService;
        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoginModel login)
        {
            var result = loginService.Post(login);            
            return StatusCode(result.IntStatusCode, HttpHelper.Result(result));
        }
    }
}