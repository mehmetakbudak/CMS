using CMS.Service.Exceptions;
using CMS.Service.Helper;
using CMS.Storage.Enum;
using CMS.Storage.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Service.Attributes
{
    public class CMSAuthorize : Attribute, IAsyncAuthorizationFilter
    {
        public bool CheckAccessRight { get; set; }
        public bool IsView { get; set; }
        public int RouteLevel { get; set; }

        public CMSAuthorize()
        {
            CheckAccessRight = true;
            IsView = false;
            RouteLevel = 2;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));
                var httpContextAccessor = (IHttpContextAccessor)context.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor));

                var isAuthenticated = httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (!isAuthenticated)
                {
                    context.Result = new RedirectResult("/login");
                    return;
                }
                //erişim hakkı kontrolü yoksa içeri al.
                if (!CheckAccessRight)
                {
                    return;
                }

                var loginUser = httpContextAccessor.HttpContext.User.Parse();

                // süper admin kontrole takılmaz.
                if (loginUser.UserType == (int)UserType.SuperAdmin)
                {
                    return;
                }

                // üye admin paneline giremez.
                if (loginUser.UserType == (int)UserType.Member)
                {
                    context.Result = new RedirectResult("/login");
                    return;
                }

                if (loginUser.UserType == (int)UserType.Admin)
                {
                    var result = await userService.Authorize(new AuthorizeModel
                    {
                        IsView = IsView,
                        RouteLevel = RouteLevel,
                        UserId = loginUser.UserId,
                        Endpoint = context.HttpContext.Request.Path,
                        Method = context.HttpContext.Request.Method
                    });

                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        return;
                    }
                    else
                    {
                        context.Result = IsView ? new RedirectResult("/login") : new UnauthorizedObjectResult(new ServiceResult
                        {
                            StatusCode = HttpStatusCode.Unauthorized,
                            Message = "Yetkisiz İşlem"
                        });
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UnAuthorizedException(ex.Message);
            }
        }
    }
}
