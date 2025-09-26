using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CMS.Storage.Dtos.Response
{
    public class ServiceResult : BaseResult
    {
        public object Data { get; set; }

        [JsonIgnore]
        public bool IsSuccessful { get; private set; }

        public List<string> Errors { get; private set; }

        public static ServiceResult Success()
        {
            return new ServiceResult
            {
                Data = default,
                IsSuccessful = true,
                StatusCode = StatusCodes.Status200OK
            };
        }

        public static ServiceResult Success(int statusCode)
        {
            return new ServiceResult
            {
                Data = default,
                StatusCode = statusCode,
                IsSuccessful = true
            };
        }

        public static ServiceResult Success(int statusCode, string message)
        {
            return new ServiceResult
            {
                Data = default,
                StatusCode = statusCode,
                IsSuccessful = true,
                Message = message
            };
        }


        public static ServiceResult Success(int statusCode, object data)
        {
            return new ServiceResult
            {
                Data = data,
                StatusCode = statusCode,
                IsSuccessful = true
            };
        }

        public static ServiceResult Success(int statusCode, object data, string message)
        {
            return new ServiceResult
            {
                Data = data,
                StatusCode = statusCode,
                IsSuccessful = true,
                Message = message
            };
        }

        public static ServiceResult Fail(int statusCode, string message)
        {
            return new ServiceResult
            {
                Errors = new List<string> { message },
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }

        public static ServiceResult Fail(int statusCode, List<string> errors)
        {
            return new ServiceResult
            {
                Errors = errors,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }       
    }

}
