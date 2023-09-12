using CMS.Service;
using CMS.Service.Attributes;
using CMS.Storage.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace CMS.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserFileService _userFileService;

        public AccountController(
            IUserService userService,
            IUserFileService userFileService)
        {
            _userService = userService;
            _userFileService = userFileService;
        }

        #region Views
        [Route("login")]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [Route("register")]
        public IActionResult Register() => View();


        [Route("forgot-password")]
        public IActionResult ForgotPassword() => View();


        [Route("change-password")]
        [CMSAuthorize(CheckAccessRight = false, IsView = true)]
        public IActionResult ChangePassword() => View();

        [Route("profile")]
        [CMSAuthorize(CheckAccessRight = false, IsView = true)]
        public IActionResult Profile() => View();

        [Route("comments")]
        [CMSAuthorize(CheckAccessRight = false, IsView = true)]
        public IActionResult Comments() => View();

        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/");
        }

        [Route("email-verify/{code}")]
        public async Task<IActionResult> EmailVerify(string code)
        {
            var result = await _userService.EmailVerified(code);
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
        public async Task<IActionResult> SetPassword(string code)
        {
            var user = await _userService.GetUserByCode(code);
            return View(user);
        }

        [Route("cv")]
        [CMSAuthorize(CheckAccessRight = false, IsView = true)]
        public IActionResult Cv() => View();

        [Route("applied-jobs")]
        [CMSAuthorize(CheckAccessRight = false, IsView = true)]
        public IActionResult AppliedJobs() => View();
        #endregion


        [HttpPost("api/account/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.Authenticate(model);
            if (user != null)
            {
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
            return Ok(user);
        }

        [HttpGet("account/profile")]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _userService.GetProfile();
            return Ok(result);
        }

        [HttpPut("account/profile")]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileModel model)
        {
            var result = await _userService.UpdateProfile(model);
            return Ok(result);
        }

        [HttpPost("api/account/forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var result = await _userService.ForgotPassword(model);
            return Ok(result);
        }

        [HttpPut("api/account/reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var result = await _userService.ResetPassword(model);
            return Ok(result);
        }

        [HttpPost("api/account/register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _userService.Register(model);
            return Ok(result);
        }

        [HttpPut("account/change-password")]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var result = await _userService.ChangePassword(model);
            return Ok(result);
        }

        [HttpPut("api/account/email-verified/{code}")]
        public async Task<IActionResult> EmailVerified(string code)
        {
            var result = await _userService.EmailVerified(code);
            return Ok(result);
        }

        [HttpGet("/account/user-files/{type}")]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> GetUserFiles(int type)
        {
            var result = await _userFileService.GetByType(type);
            return Ok(result);
        }

        [HttpPost("/account/user-files")]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> PostUserFiles(UserFilePostModel model)
        {
            var result = await _userFileService.Post(model);
            return Ok(result);
        }

        [HttpPut("/account/user-files/set-default/{id}")]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> SetDefaultUserFiles(int id)
        {
            var result = await _userFileService.SetDefault(id);
            return Ok(result);
        }

        [HttpDelete("/account/user-files/{id}")]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> DeleteUserFiles(int id)
        {
            var result = await _userFileService.Delete(id);
            return Ok(result);
        }


    }
}
