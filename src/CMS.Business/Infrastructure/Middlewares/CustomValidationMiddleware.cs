using CMS.Storage.Dtos.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace CMS.Business.Infrastructure.Middlewares
{
    public static class CustomValidationMiddleware
    {
        public static void UseCustomValidationMiddleware(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values
                        .Where(x => x.Errors.Count > 0)
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage).ToList();

                    var response = ServiceResult.Fail(StatusCodes.Status400BadRequest, errors);
                    return new BadRequestObjectResult(response);
                };
            });
        }
    }
}
