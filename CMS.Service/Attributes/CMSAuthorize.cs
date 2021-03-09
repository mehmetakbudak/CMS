using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;

namespace CMS.Service.Attributes
{
    public class CMSAuthorize : Attribute, IAuthorizationFilter
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
                    var path = context.HttpContext.Request.Path;
                    if (path.HasValue)
                        context.HttpContext.Response.Redirect("/login?returnUrl=" + path.Value);
                    else
                        context.HttpContext.Response.Redirect("/login");
                }
                else
                {
                    var accessRights = accessRightService.GetAccessRightsByUserId(userId.Value, true);

                    var path = context.HttpContext.Request.Path;
                    if (path.HasValue)
                    {
                        var checkPath = accessRights.Any(x => x.LinkUrl == path.Value);
                        if (!checkPath)
                            context.HttpContext.Response.Redirect("/error/403");
                    }
                    else
                        context.HttpContext.Response.Redirect("/login");
                }
            }
            catch(Exception ex)
            {                
                context.HttpContext.Response.Redirect("/login");
            }
        }
    }
}