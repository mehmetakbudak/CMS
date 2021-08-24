using CMS.Model.Enum;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using CMS.Service.Helper;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace CMS.Service.Attributes
{
    public class CMSAuthorize : Attribute, IAuthorizationFilter
    {
        public bool CheckAccessRight { get; set; }
        public CMSAuthorize()
        {
            CheckAccessRight = true;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            ServiceResult result = new ServiceResult();
            var userAccessRightService = (IUserAccessRightService)context.HttpContext.RequestServices.GetService(typeof(IUserAccessRightService));

            var jwtHelper = (IJwtHelper)context.HttpContext.RequestServices.GetService(typeof(IJwtHelper));

            var userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));

            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                throw new NotFoundException("Token bulunamadı.");
            }

            var jwtToken = jwtHelper.ValidateToken(token);

            if (jwtToken == null)
            {
                throw new BadRequestException("Token doğrulanamadı.");
            }

            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);

            var user = userService.GetTokenInfo(userId, token);

            if (user != null)
            {
                if (CheckAccessRight && user.UserType != UserType.SuperAdmin)
                {
                    var userAccessRights = userAccessRightService.GetByUserId(userId);
                    if (userAccessRights != null && userAccessRights.Any())
                    {
                        var accessRights = userAccessRights.Select(x => x.AccessRight).ToList();

                        var endpoint = context.HttpContext.Request.Path.Value.Replace("/api", "");
                        var method = context.HttpContext.Request.Method;

                        var check = accessRights.Any(x => x.Endpoint.ToLower() == endpoint.ToLower() && x.Method == method && x.Type == AccessRightType.Operation);

                        if (!check)
                        {
                            throw new UnAuthorizedException("Yetkisiz İşlem");
                        }
                    }
                    else
                    {
                        throw new UnAuthorizedException("Yetkisiz İşlem");
                    }
                }

                AuthTokenContent.Current = new AuthTokenContent
                {
                    UserId = userId,
                    UserType = user.UserType
                };
            }
            else
            {
                throw new NotAcceptableException("Token geçerliliğini yitirmiş olabilir. Lütfen yeniden giriş yapınız.");
            }

        }
    }
}
