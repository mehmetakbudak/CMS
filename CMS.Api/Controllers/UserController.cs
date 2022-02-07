using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            var result = _userService.Authenticate(login);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var result = _userService.ForgotPassword(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetProfile")]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult GetProfile()
        {
            var result = _userService.GetProfile();
            return Ok(result);
        }

        [HttpGet("GetMemberComments")]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult GetMemberComments()
        {
            var result = _userService.GetMemberComments();
            return Ok(result);
        }

        [HttpPost("UpdateProfile")]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult UpdateProfile([FromBody] UserProfileModel model)
        {
            var result = _userService.UpdateProfile(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("AddMember")]
        public IActionResult AddMember([FromBody]AddMemberModel model)
        {
            var result = _userService.AddMember(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("ChangePassword")]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult ChangePassword([FromBody]ChangePasswordModel model)
        {
            var result = _userService.ChangePassword(model);
            return StatusCode(result.StatusCode, result);
        }       
    }
}
