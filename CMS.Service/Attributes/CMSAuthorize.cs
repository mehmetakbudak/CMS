using CMS.Model.Enum;
using CMS.Model.Model;
using CMS.Service.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;

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
            try
            {
                ServiceResult result = new ServiceResult();
                var userAccessRightService = (IUserAccessRightService)context.HttpContext.RequestServices.GetService(typeof(IUserAccessRightService));

                var jwtHelper = (IJwtHelper)context.HttpContext.RequestServices.GetService(typeof(IJwtHelper));

                var userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));

                var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    result.StatusCode = (int)HttpStatusCode.NotFound;
                    result.Message = "Token bulunamadı.";
                    context.Result = new ObjectResult(result);
                    return;
                }

                try
                {
                    var jwtToken = jwtHelper.ValidateToken(token);

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
                                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                                    result.StatusCode = (int)HttpStatusCode.Forbidden;
                                    result.Message = "Yetkisiz İşlem";
                                    context.Result = new ObjectResult(result);
                                    return;
                                }
                            }
                            else
                            {
                                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                result.StatusCode = (int)HttpStatusCode.Unauthorized;
                                result.Message = "Yetkisiz İşlem";
                                context.Result = new ObjectResult(result);
                                return;
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
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                        result.StatusCode = (int)HttpStatusCode.NotAcceptable;
                        result.Message = "Token geçerliliğini yitirmiş olabilir. Lütfen yeniden giriş yapınız.";
                        context.Result = new ObjectResult(result);
                        return;
                    }
                }
                catch
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.Message = "Token doğrulanamadı.";
                    context.Result = new ObjectResult(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                context.Result = new ObjectResult(new ServiceResult { Message = ex.Message });
            }
        }
    }
}
