using CMS.Business.Exceptions;
using CMS.Storage.Dtos.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CMS.Business.Middlewares
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleException(ex, httpContext);
            }
        }

        private async Task HandleException(Exception ex, HttpContext context)
        {
            string result = "";
            int statusCode = 500;

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            BaseResult error = new BaseResult();

            if (ex is ApiExceptionBase apiExceptionBase)
            {
                statusCode = apiExceptionBase.StatusCode;
                error = apiExceptionBase.Error;
                result = JsonConvert.SerializeObject(error, serializerSettings);
            }
            else
            {
                error = new BaseResult
                {
                    StatusCode = 500,
                    Message = ex.Message
                };
                result = JsonConvert.SerializeObject(error, serializerSettings);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(result);
        }
    }
    public static class ExceptionHandleMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandleMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandleMiddleware>();
        }
    }
}
