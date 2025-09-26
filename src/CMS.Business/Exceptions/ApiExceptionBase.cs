using CMS.Storage.Dtos.Response;
using System;
using System.Net;

namespace CMS.Business.Exceptions
{
    public abstract class ApiExceptionBase : Exception
    {
        public ApiExceptionBase()
        {
        }

        public ApiExceptionBase(string message) : base(message)
        {

        }

        public BaseResult Error { get; set; }

        protected abstract HttpStatusCode HttpStatusCode { get; }

        public int StatusCode => (int)HttpStatusCode;
    }   
}
