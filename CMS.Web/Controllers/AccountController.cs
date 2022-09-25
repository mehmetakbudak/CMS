using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace CMS.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #region Views
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }


        [Route("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("change-password")]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Route("profile")]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult Profile()
        {
            return View();
        }

        [Route("comments")]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult Comments()
        {
            return View();
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/");
        }

        [Route("email-verify/{code}")]
        public IActionResult EmailVerify(string code)
        {
            var result = _userService.EmailVerified(code);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.IsSuccess = true;
            }
            else
            {
                ViewBag.IsSuccess = false;
            }
            return View();
        }

        [Route("set-password/{code}")]
        public IActionResult SetPassword(string code)
        {
            var user = _userService.GetUserByCode(code);
            return View(user);
        }
        #endregion

        #region APIs
        [HttpPost("api/account/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _userService.Authenticate(model);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var user = (User)result.Data;

                var userClaims = new List<Claim>();

                userClaims.Add(new Claim("UserId", user.Id.ToString()));
                userClaims.Add(new Claim("FullName", user.FullName));
                userClaims.Add(new Claim("Name", user.Name));
                userClaims.Add(new Claim("Surname", user.Surname.ToString()));
                userClaims.Add(new Claim("UserType", ((int)user.UserType).ToString()));

                var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);                
            }
            return Ok(result);
        }

        [HttpGet("api/account/profile")]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult GetProfile()
        {
            var result = _userService.GetProfile();
            return Ok(result);
        }

        [HttpPut("api/account/profile")]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult UpdateProfile([FromBody] UserProfileModel model)
        {
            var result = _userService.UpdateProfile(model);
            return Ok(result);
        }

        [HttpPost("api/account/forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var result = await _userService.ForgotPassword(model);
            return Ok(result);
        }

        [HttpPut("api/account/reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
        {
            var result = _userService.ResetPassword(model);
            return Ok(result);
        }

        [HttpPost("api/account/add-member")]
        public async Task<IActionResult> AddMember([FromBody] AddMemberModel model)
        {
            var result = await _userService.AddMember(model);
            return Ok(result);
        }

        [HttpPut("api/account/change-password")]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult ChangePassword([FromBody] ChangePasswordModel model)
        {
            var result = _userService.ChangePassword(model);
            return Ok(result);
        }

        [HttpPut("api/account/email-verified/{code}")]
        public IActionResult EmailVerified(string code)
        {
            var result = _userService.EmailVerified(code);
            return Ok(result);
        }
        #endregion
    }
}
