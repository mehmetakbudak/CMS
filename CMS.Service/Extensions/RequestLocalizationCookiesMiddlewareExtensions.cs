using CMS.Service.Infrastructure;
using Microsoft.AspNetCore.Builder;

namespace CMS.Service.Extensions
{
    public static class RequestLocalizationCookiesMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLocalizationCookies(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLocalizationCookiesMiddleware>();
        }
    }
}
