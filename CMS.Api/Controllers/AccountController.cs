using CMS.Model.Entity;
using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using CMS.Service.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMemoryCache _memoryCache;
        private readonly IJwtHelper _jwtHelper;

        public AccountController(IUserService userService,
            IMemoryCache memoryCache,
            IJwtHelper jwtHelper)
        {
            _userService = userService;
            _memoryCache = memoryCache;
            _jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Kullanıcı hesabı giriş işlemlerini yapar.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var result = await _userService.Authenticate(login);
            return Ok(result);
        }

        /// <summary>
        /// Kullanıcı hesabı çıkış işlemlerini yapar.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Logout")]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult Logout()
        {
            //string key = $"userMenu_{AuthTokenContent.Current.UserId}";
            //_memoryCache.Remove(key);
            return Ok();
        }

        /// <summary>
        /// Kullanıcının şifresini belirlemesi için yeni istek oluşturur.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var result = await _userService.ForgotPassword(model);
            return Ok(result);
        }

        /// <summary>
        /// Şifresi belirlenmemiş kullanıcı hesabı için bilgileri döner.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("ResetPassword/{code}")]
        [ProducesResponseType(typeof(ResetPasswordInfoModel), 200)] //OK
        public async Task<IActionResult> ResetPassword(string code)
        {
            var result = await _userService.GetUserByCode(code);
            return Ok(result);
        }

        /// <summary>
        /// Şifresi belirlenmemiş kullanıcı hesabının yeni şifresini günceller.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var result = await _userService.ResetPassword(model);
            return Ok(result);
        }

        /// <summary>
        /// Kullanıcının hesap bilgilerini döner.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Profile")]
        [CMSAuthorize(CheckAccessRight = false)]
        [ProducesResponseType(typeof(UserProfileModel), 200)] //OK
        public async Task<IActionResult> GetProfile()
        {
            var result = await _userService.GetProfile();
            return Ok(result);
        }

        /// <summary>
        /// Kullanıcının hesap bilgilerini günceller.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Profile")]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileModel model)
        {
            var result = await _userService.UpdateProfile(model);
            return Ok(result);
        }

        /// <summary>
        /// Yeni üye kaydetmek için kullanılır.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _userService.Register(model);
            return Ok(result);
        }

        /// <summary>
        /// Şifre değişikliğini gerçekleştirir.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("ChangePassword")]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var result = await _userService.ChangePassword(model);
            return Ok(result);
        }

        /// <summary>
        /// Email doğrulaması yapar.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPut("EmailVerified/{code}")]
        public async Task<IActionResult> EmailVerified(string code)
        {
            var result = await _userService.EmailVerified(code);
            return Ok(result);
        }
    }
}
