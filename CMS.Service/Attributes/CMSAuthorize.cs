using CMS.Storage.Consts;
using CMS.Service.Exceptions;
using CMS.Service.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Service.Attributes
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
            try
            {
                var userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));
                var httpContextAccessor = (IHttpContextAccessor)context.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor));


                //var request = context.HttpContext.Request;
                //var token = request.Headers["Authorization"].ToString();

                //if (string.IsNullOrEmpty(token))
                //{
                //    throw new NotFoundException("Token bulunamadı.");
                //}

                //string tokenString = token.Split(' ')[1];

                //if (string.IsNullOrEmpty(tokenString))
                //{
                //    throw new BadRequestException("Token formatı uygun değil.");
                //}

                //var key = Encoding.ASCII.GetBytes(Global.Secret);

                //var handler = new JwtSecurityTokenHandler();

                //try
                //{
                //    handler.ValidateToken(tokenString, new TokenValidationParameters
                //    {
                //        ValidateIssuerSigningKey = true,
                //        IssuerSigningKey = new SymmetricSecurityKey(key),
                //        ValidateIssuer = false,
                //        ValidateAudience = false
                //    }, out SecurityToken securityToken);
                //}
                //catch
                //{
                //    throw new BadRequestException("Kimlik doğrulanamadı.");
                //}

                //var tokenDescrypt = handler.ReadJwtToken(tokenString);

                //var strUserId = tokenDescrypt.Claims.FirstOrDefault(x => x.Type == JwtTokenPayload.UserId);

                //if (strUserId == null || !Int32.TryParse(strUserId.Value, out int userId))
                //{
                //    throw new BadRequestException("Token hatalı.");
                //}

                //var user = await userService.GetById(userId);

                //if (user == null)
                //{
                //    throw new NotFoundException("Kullanıcı bulunamadı.");
                //}

                //if (!string.IsNullOrEmpty(user.Token))
                //{
                //    if (user.Token.ToLower() != tokenString.ToLower())
                //    {
                //        throw new UnAuthorizedException("Token süresi dolmuş. Tekrar giriş yapınız.");
                //    }
                //}
                //else
                //{
                //    throw new UnAuthorizedException("Lütfen giriş yapınız.");
                //}

                //if (!user.TokenExpireDate.HasValue)
                //{
                //    throw new UnAuthorizedException("Token süresi dolmuş. Tekrar giriş yapınız.");
                //}

                //var tokenStartDate = user.TokenExpireDate.Value.AddHours(-2);
                //var tokenEndDate = user.TokenExpireDate.Value;

                //if (!((tokenStartDate <= DateTime.Now) && (tokenEndDate >= DateTime.Now)))
                //{
                //    throw new UnAuthorizedException("Token süresi dolmuş. Tekrar giriş yapınız.");
                //}

            }
            catch (Exception ex)
            {
                throw new UnAuthorizedException(ex.Message);
            }
        }      
    }
}
