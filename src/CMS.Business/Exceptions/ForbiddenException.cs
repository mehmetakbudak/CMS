using CMS.Storage.Dtos.Response;
using System.Net;

namespace CMS.Business.Exceptions
{
    public class ForbiddenException : ApiExceptionBase
    {
        public ForbiddenException() : base()
        {
            Error = new BaseResult { StatusCode = 403, Message = "Yetkisiz Giriş." };
        }

        public ForbiddenException(string message) : base(message)
        {
            Error = new BaseResult { StatusCode = 403, Message = message };
        }

        protected override HttpStatusCode HttpStatusCode => HttpStatusCode.Forbidden;
    }
}
