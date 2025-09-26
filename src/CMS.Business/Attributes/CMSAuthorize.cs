using CMS.Business.Infrastructure;
using CMS.Business.Services;
using CMS.Storage.Dtos.Auth;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Attributes
{
    public class CMSAuthorize : Attribute, IAsyncAuthorizationFilter
    {
        public bool CheckAccessRight { get; set; }

        public CMSAuthorize()
        {
            CheckAccessRight = true;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userTokenService = context.HttpContext.RequestServices.GetService(typeof(IUserTokenService)) as IUserTokenService;
            var authService = context.HttpContext.RequestServices.GetService(typeof(IAuthService)) as IAuthService;

            var request = context.HttpContext.Request;

            var token = request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                var result = ServiceResult.Fail(StatusCodes.Status404NotFound, "Token.Notfound");
                context.Result = new NotFoundObjectResult(result);
                return;
            }

            var handler = new JwtSecurityTokenHandler();

            var tokenDescrypt = handler.ReadJwtToken(token);

            var key = Encoding.ASCII.GetBytes(Global.Secret);

            try
            {
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = Global.Issuer,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken securityToken);
            }
            catch
            {
                var result = ServiceResult.Fail(StatusCodes.Status401Unauthorized, "Token.Invalid");
                context.Result = new UnauthorizedObjectResult(result);
                return;
            }

            var strUserId = tokenDescrypt.Payload.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId)?.Value;

            if (!Int32.TryParse(strUserId, out int userId))
            {
                var result = ServiceResult.Fail(StatusCodes.Status401Unauthorized, "Token.Invalid");
                context.Result = new UnauthorizedObjectResult(result);
                return;
            }
            var userToken = await userTokenService.GetByUserId(userId);

            if (userToken is null || string.IsNullOrEmpty(userToken.AccessToken))
            {
                var result = ServiceResult.Fail(StatusCodes.Status404NotFound, "Token.Notfound");
                context.Result = new NotFoundObjectResult(result);
                return;
            }

            if (userToken.AccessToken.ToLower() != token.ToLower())
            {
                var result = ServiceResult.Fail(StatusCodes.Status401Unauthorized, "Token.Expired");
                context.Result = new UnauthorizedObjectResult(result);
                return;
            }

            if (userToken.AccessTokenExpireDate < DateTime.Now)
            {
                var result = ServiceResult.Fail(StatusCodes.Status401Unauthorized, "Token.Expired");
                context.Result = new NotFoundObjectResult(result);
                return;
            }

            if (CheckAccessRight && userToken.User.UserType == UserType.Admin)
            {
                var method = context.HttpContext.Request.Method;
                var path = context.HttpContext.Request.Path.Value;

                await authService.Authorize(new AuthorizeDto
                {
                    Method = method,
                    Path = path,
                    UserId = userId
                });
            }
        }
    }
}
