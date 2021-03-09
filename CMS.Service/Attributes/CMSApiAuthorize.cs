using CMS.Model.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;

namespace CMS.Service.Attributes
{
    public class CMSApiAuthorize : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var accessRightService = (IAccessRightService)context.HttpContext.RequestServices.GetService(typeof(IAccessRightService));

                var userId = context.HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.HttpContext.Response.Redirect("/error/403");
                }
                else
                {
                    var accessRights = accessRightService.GetAccessRightsByUserId(userId.Value, false);

                    var path = context.HttpContext.Request.Path;
                    var method = context.HttpContext.Request.Method;
                    var httpStatusType = (HttpStatusType)Enum.Parse(typeof(HttpStatusType), method);

                    if (path.HasValue)
                    {
                        var checkPath = accessRights.Any(x => x.LinkUrl == path.Value && x.HttpStatusType == httpStatusType && x.AccessRightCategory.Type == AccessRightCategoryType.Api);
                        if (!checkPath)
                        {
                            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.HttpContext.Response.Redirect("/error/403");
                        }
                    }
                    else
                    {
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.HttpContext.Response.Redirect("/error/403");
                    }
                }
            }
            catch (Exception ex)
            {
                context.Result = new ObjectResult(new { ex.Message, Ok = false });
            }
        }
    }
}
