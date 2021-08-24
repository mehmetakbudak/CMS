using CMS.Model.Model;
using System;
using System.Net;

namespace CMS.Service.Exceptions
{
    public class ForbiddenException : ApiExceptionBase
    {
        public ForbiddenException() : base()
        {
            Error = new BaseResult { StatusCode = (int)HttpStatusCode.Forbidden, Message = "Yetkisiz Giriş." };
        }

        public ForbiddenException(string message) : base(message)
        {
            Error = new BaseResult { StatusCode = (int)HttpStatusCode.Forbidden, Message = message };
        }

        protected override HttpStatusCode HttpStatusCode => HttpStatusCode.Forbidden;
    }
}
