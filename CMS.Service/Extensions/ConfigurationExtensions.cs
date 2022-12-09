using CMS.Storage.Model;
using CMS.Service.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;

namespace CMS.Service.Middlewares
{
    public static class ConfigurationExtensions
    {
        public static IApplicationBuilder ErrorHandler(this IApplicationBuilder app)
        {
            return
                app.Use(async (context, next) =>
                {
                    try
                    {
                        await next.Invoke();
                    }
                    catch (Exception ex)
                    {
                        string result = "";
                        int statusCode = 500;

                        //model labels must be lowcase
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
                                StatusCode = HttpStatusCode.InternalServerError,
                                Message = ex.Message
                            };
                            result = JsonConvert.SerializeObject(error, serializerSettings);
                        }

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = statusCode;
                        await context.Response.WriteAsync(result);
                    }
                });
        }
    }
}
