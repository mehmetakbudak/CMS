using CMS.Model.Enum;
using Microsoft.AspNetCore.Mvc;
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
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult("/login");
                return;
            }

            var userAccessRightService = (IUserAccessRightService)context.HttpContext.RequestServices.GetService(typeof(IUserAccessRightService));

            var userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));

            var _userId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId").Value;

            Int32.TryParse(_userId, out int userId);

            var user = userService.GetById(userId);

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

                        var check = accessRights.Any(x => x.AccessRightEndpoints.Any(a => a.Endpoint.ToLower() == endpoint.ToLower() && a.Method == method) && x.Type == AccessRightType.Operation);

                        if (!check)
                        {
                            context.Result = new RedirectResult("/login");
                            return;
                        }
                    }
                    else
                    {
                        context.Result = new RedirectResult("/login");
                        return;
                    }
                }
            }
            else
            {
                context.Result = new RedirectResult("/login");
                return;
            }

        }
    }
}
