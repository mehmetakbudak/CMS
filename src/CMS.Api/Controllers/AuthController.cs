using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class AuthController(IAuthService authService) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var response = await authService.Login(model);
            return Ok(response);
        }

        [HttpGet]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> GetProfile()
        {
            var response = await authService.GetProfile();
            return Ok(response);
        }

        [HttpPut]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> UpdateProfile(UserProfileDto model)
        {
            var response = await authService.UpdateProfile(model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var response = await authService.Register(model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model)
        {
            var response = await authService.ForgotPassword(model);
            return Ok(response);
        }

        [HttpPost]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            var response = await authService.ChangePassword(model);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> EmailVerified(string code)
        {
            var response = await authService.EmailVerified(code);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByCode(string code)
        {
            var response = await authService.GetUserByCode(code);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            var response = await authService.ResetPassword(model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto model)
        {
            var response = await authService.CreateTokenByRefreshToken(model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto model)
        {
            var response = await authService.RevokeRefreshToken(model);
            return Ok(response);
        }
    }
}
